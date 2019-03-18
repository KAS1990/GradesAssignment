using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using MSExcel = Microsoft.Office.Interop.Excel;

namespace GradesAssignment
{
    public static class ExcelHelper
    {
        private const int MAX_ROW_TO_SEARCH = 20;
        private const int MAX_COL_TO_SEARCH = 10;

        #region Start and close Excel methods

        public static ExcelApplicationEx StartExcel()
        {
            var res = new ExcelApplicationEx();
            res.App = StartExcel(out res.NewAppCreated);
            return res;
        }

        public static MSExcel.Application StartExcel(out bool NewAppCreated)
        {
            MSExcel.Application instance = null;
            try
            {
                instance = (MSExcel.Application)Marshal.GetActiveObject("Excel.Application");
                NewAppCreated = false;
            }
            catch (Exception ex)
            {
                ex.ToString();
                instance = new MSExcel.Application();
                /*instance.WindowState = MSExcel.XlWindowState.xlMinimized;
				instance.Visible = true;*/
                NewAppCreated = true;
            }

            return instance;
        }
                
        private static void SafelyCloseExcelApp(ref ExcelApplicationEx app)
        {
            if (app != null)
            {
                app.App.DisplayAlerts = true;

                if (app.NewAppCreated)
                    app.App.Quit();

                app = null;
            }
        }

        #endregion

        #region Open and close workbook methods

        public static ExcelWorkbookEx OpenWbkEx(ExcelApplicationEx excelApp, string wbkFullPath)
        {
            var res = new ExcelWorkbookEx();
            res.Wbk = OpenWbk(excelApp, wbkFullPath, out res.WbkOpened);
            return res;
        }

        public static MSExcel.Workbook OpenWbk(ExcelApplicationEx excelApp, string wbkFullPath)
        {
            bool WbkOpened;
            return OpenWbk(excelApp, wbkFullPath, out WbkOpened);
        }
                
        public static MSExcel.Workbook OpenWbk(ExcelApplicationEx excelApp, string wbkFullPath, out bool WbkOpened)
        {
            MSExcel.Workbook result = null;

            WbkOpened = false;

            // Открываем книгу
            if (excelApp.NewAppCreated)
            {   // Без этого книга не открывается
                excelApp.App.Visible = true;
                excelApp.App.WindowState = MSExcel.XlWindowState.xlMinimized;
            }
            foreach (MSExcel.Workbook book in excelApp.App.Workbooks)
            {
                if (book.FullName == wbkFullPath)
                {   // Книга уже открыта => используем её
                    result = book;
                    break;
                }
            }
            if (result == null)
            {
                try
                {
                    result = excelApp.App.Workbooks.Open(wbkFullPath);
                    WbkOpened = true;
                }
                catch
                {}
            }
            if (excelApp.NewAppCreated)
            {
                excelApp.App.Visible = false;
                excelApp.App.WindowState = MSExcel.XlWindowState.xlNormal;
            }

            return result;
        }
                
        private static void SafelyCloseWorkbook(ref ExcelWorkbookEx wbk)
        {
            if (wbk != null)
            {
                if (wbk.WbkOpened)
                {
                    wbk.Wbk.Save();
                    wbk.Wbk.Close();
                }

                wbk = null;
            }
        }

        #endregion

        public static int? SearchFirstDataRowAfterHeader(MSExcel.Worksheet wsh, int headerRow, int col)
        {
            for (int i = headerRow + 1; i < headerRow + MAX_ROW_TO_SEARCH; i++)
            {
                try
                {
                    if (!string.IsNullOrEmpty(wsh.Cells[i, col].Value?.ToString()))
                    {
                        return i;
                    }
                }
                catch
                { }
            }

            return null;
        }

        public static List<WorksheetInfo> GetWorksheetInfo(string wbkFullPath, CalculationSettings calculationSettings, out string message)
        {
            message = null;

            List<WorksheetInfo> result = new List<WorksheetInfo>();
            using (var excelApp = new DisposableWrapper<ExcelApplicationEx>(StartExcel(), app => SafelyCloseExcelApp(ref app)))
            {
                if (excelApp.Object != null)
                {
                    excelApp.Object.App.DisplayAlerts = false; // Отключаем различные сообщения

                    using (var excelWbk = new DisposableWrapper<ExcelWorkbookEx>(OpenWbkEx(excelApp.Object, wbkFullPath),
                                                                                 wbk => SafelyCloseWorkbook(ref wbk)))
                    {
                        if (excelWbk.Object != null && excelWbk.Object.Wbk != null)
                        {
                            foreach (MSExcel.Worksheet wsh in excelWbk.Object.Wbk.Worksheets)
                            {
                                if (wsh.Visible == MSExcel.XlSheetVisibility.xlSheetVisible)
                                {
                                    var info = SearchColAndRows(wsh, out message);
                                    info.Name = wsh.Name;

                                    if (string.IsNullOrEmpty(message))
                                    {
                                        if (info.AllColAndRowIndexesAreFilled)
                                        {   // Читаем сведения об участниках из листа 
                                            var fillMembersResult = info.FillMembersFromSheet(wsh, calculationSettings);

                                            if (fillMembersResult.Result != enFillMembersFromSheetResult.OK)
                                            {
                                                switch (fillMembersResult.Result)
                                                {
                                                    case enFillMembersFromSheetResult.CriticalError:
                                                        return new List<WorksheetInfo>();
                                                    case enFillMembersFromSheetResult.SkipWorksheet:
                                                        info.Members.Clear();
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        return new List<WorksheetInfo>();
                                    }

                                    result.Add(info);
                                }
                            }
                        }
                        else
                        {
                            message = $"Не удалось открыть книгу {wbkFullPath}.\n\rИли книга находится в режиме защищённого просмотра. Если это так, то откройте её с помощью приложение MS Excel и отключите этот режим.";
                        }
                    }
                }
                else
                {
                    message = $"Не удалось запустить Excel";
                }
            }
            
            return result;
        }

        public static WorksheetInfo SearchColAndRows(MSExcel.Worksheet wsh, out string message)
        {
            var result = new WorksheetInfo();

            message = "";

            var valuesToSearch = new HashSet<enSearchResult>()
            {
                enSearchResult.Place, enSearchResult.SurnameAndName, enSearchResult.YearOfBirth, enSearchResult.Grade
            };

            try
            {
                int col = 1;
                for (int row = 1; row < MAX_ROW_TO_SEARCH; row++)
                {
                    if (wsh.Rows[row].Hidden)
                        continue;

                    while (wsh.Columns[col].Hidden)
                        col++;

                    var source = wsh.Cells[row, col].Value?.ToString();

                    var searchResult = SearchStrings.Check(source);
                    if (searchResult != enSearchResult.NotFound)
                    {
                        // Пропускаем строки, чтобы исключить объединённые строки
                        result.FirstRow = SearchFirstDataRowAfterHeader(wsh, row, col);
                        if (result.FirstRow.HasValue)
                        {
                            // Ищем столбцы
                            do
                            {
                                switch (searchResult)
                                {
                                    case enSearchResult.Place:
                                        result.PlaceCol = col;
                                        break;

                                    case enSearchResult.SurnameAndName:
                                        result.NameSurnameCol = col;
                                        break;

                                    case enSearchResult.YearOfBirth:
                                        result.YearOfBirthCol = col;
                                        break;

                                    case enSearchResult.Grade:
                                        result.InitGradeCol = col;
                                        break;
                                }
                                valuesToSearch.Remove(searchResult);

                                col++;

                                if (wsh.Columns[col].Hidden)
                                    continue;

                                source = wsh.Cells[row, col].Value?.ToString();
                                searchResult = SearchStrings.Check(source);
                            }
                            while (valuesToSearch.Count > 0 && col < MAX_COL_TO_SEARCH);
                        }
                                                
                        break;
                    }
                }

            }
            catch
            {
                message = $"Не удалось проанализировать лист {wsh.Name}";
            }

            return result;
        }

        public static bool IsTableHeaderExistInRow(MSExcel.Worksheet wsh, WorksheetInfo infoToCompare, int row)
        {
            if (!infoToCompare.AllColAndRowIndexesAreFilled)
                return false;

            var columnsToCheck = new List<KeyValuePair<int, enSearchResult>>()
            {
                new KeyValuePair<int, enSearchResult>(infoToCompare.PlaceCol.Value, enSearchResult.Place),
                new KeyValuePair<int, enSearchResult>(infoToCompare.NameSurnameCol.Value, enSearchResult.SurnameAndName),
                new KeyValuePair<int, enSearchResult>(infoToCompare.YearOfBirthCol.Value, enSearchResult.YearOfBirth),
                new KeyValuePair<int, enSearchResult>(infoToCompare.InitGradeCol.Value, enSearchResult.Grade)
            };

            try
            {
                foreach (var colInfo in columnsToCheck)
                {
                    var source = wsh.Cells[row, colInfo.Key].Value?.ToString();
                    var searchResult = SearchStrings.Check(source);
                    if (searchResult != colInfo.Value)
                        return false;
                }
            }
            catch
            {
                return false;
            }
                        
            return true;
        }

        public static FillMembersFromSheetResult FillMembersFromSheet(string wbkFullPath, int worksheetIndex,
            CalculationSettings calculationSettings, WorksheetInfo worksheetInfo)
        {
            worksheetInfo.LastFillMembersFromSheetResult.Reset();

            using (var excelApp = new DisposableWrapper<ExcelApplicationEx>(StartExcel(), app => SafelyCloseExcelApp(ref app)))
            {
                if (excelApp.Object != null)
                {
                    excelApp.Object.App.DisplayAlerts = false; // Отключаем различные сообщения

                    using (var excelWbk = new DisposableWrapper<ExcelWorkbookEx>(OpenWbkEx(excelApp.Object, wbkFullPath),
                                                                                 wbk => SafelyCloseWorkbook(ref wbk)))
                    {
                        if (excelWbk.Object != null && excelWbk.Object.Wbk != null)
                        {
                            if (worksheetIndex > 0 && worksheetIndex <= excelWbk.Object.Wbk.Worksheets.Count)
                            {
                                try
                                {
                                    MSExcel.Worksheet wsh = excelWbk.Object.Wbk.Worksheets[worksheetIndex];
                                    if (wsh.Visible == MSExcel.XlSheetVisibility.xlSheetVisible)
                                    {
                                        worksheetInfo.FillMembersFromSheet(wsh, calculationSettings);
                                        if (worksheetInfo.LastFillMembersFromSheetResult.Result == enFillMembersFromSheetResult.SkipWorksheet)
                                            worksheetInfo.Members.Clear();
                                    }
                                }
                                catch
                                {
                                    worksheetInfo.LastFillMembersFromSheetResult.Set(enFillMembersFromSheetResult.SkipWorksheet,
                                        $"Не удалось прочитать сведения об участниках из листа {worksheetInfo.Name}. Данные из этого листа не будут отображены.");
                                }
                            }
                            else
                            {
                                worksheetInfo.LastFillMembersFromSheetResult.Set(enFillMembersFromSheetResult.SkipWorksheet,
                                        $"Лист {worksheetInfo.Name} с номером {worksheetIndex} не найден. Данные из этого листа не будут отображены.");
                            }
                        }
                        else
                        {
                            worksheetInfo.LastFillMembersFromSheetResult.Set(enFillMembersFromSheetResult.CriticalError,
                                                                            $"Не удалось открыть книгу {wbkFullPath}");
                        }
                    }
                }
                else
                {
                    worksheetInfo.LastFillMembersFromSheetResult.Set(enFillMembersFromSheetResult.CriticalError,
                                                                            $"Не удалось запустить Excel");
                }
            }

            return worksheetInfo.LastFillMembersFromSheetResult;
        }
    }

    public class ExcelApplicationEx
    {
        public MSExcel.Application App;
        public bool NewAppCreated;
    }

    public class ExcelWorkbookEx
    {
        public MSExcel.Workbook Wbk;
        public bool WbkOpened;
    }
}
