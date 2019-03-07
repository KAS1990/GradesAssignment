using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSExcel = Microsoft.Office.Interop.Excel;

namespace GradesAssignment
{
    public class WorksheetInfo
    {
        /// <summary>
        /// Максимальное число строк, которые могут разделять части протокола на одном листе
        /// </summary>
        private const int MAX_GAP_BETWEEN_REPORT_PARTS_IN_ROWS = 15;

        public const int DefaultFirstRow = 8;

        public const int DefaultPlaceCol = 1;
        public const int DefaultNameSurnameCol = 2;
        public const int DefaultYearOfBirthCol = 4;
        public const int DefaultInitGradeCol = 5;

        public string Name { get; set; }

        /// <summary>
        /// Первая строка с результатом. Нумерация начинается с 1!!!
        /// </summary>
        public int? FirstRow { get; set; }

        /// <summary>
        /// Нумерация начинается с 1!!!
        /// </summary>
        public int? PlaceCol { get; set; }

        /// <summary>
        /// Нумерация начинается с 1!!!
        /// </summary>
        public int? NameSurnameCol { get; set; }

        /// <summary>
        /// Нумерация начинается с 1!!!
        /// </summary>
        public int? YearOfBirthCol { get; set; }

        /// <summary>
        /// Нумерация начинается с 1!!!
        /// </summary>
        public int? InitGradeCol { get; set; }

        public FillMembersFromSheetResult LastFillMembersFromSheetResult { get; } = new FillMembersFromSheetResult();

        public List<MemberInfo> Members { get; } = new List<MemberInfo>();

        public bool AllColAndRowIndexesAreFilled => FirstRow.HasValue && PlaceCol.HasValue && NameSurnameCol.HasValue && YearOfBirthCol.HasValue && InitGradeCol.HasValue;

        public FillMembersFromSheetResult FillMembersFromSheet(MSExcel.Worksheet wsh, CalculationSettings calculationSettings)
        {
            LastFillMembersFromSheetResult.Reset();

            if (!AllColAndRowIndexesAreFilled)
            {
                LastFillMembersFromSheetResult.Set(enFillMembersFromSheetResult.SkipWorksheet,
                                                   "Не заполнены настройки, связанные с автоматическим анализом итоговых протоколов. Данные из этого листа не будут отображены.");
                return LastFillMembersFromSheetResult;
            }

            Members.Clear();

            string source = null;
            int toNextTableHeaderRowsLeft = -1;
                        
            for (int row = FirstRow.Value; toNextTableHeaderRowsLeft != 0; row++)
            {
                var memberInfo = new MemberInfo();

                try
                {
                    if (toNextTableHeaderRowsLeft > 0)
                    {   // Проверяем, есть ли шапка в строке
                        if (ExcelHelper.IsTableHeaderExistInRow(wsh, this, row))
                        {   // Есть => ищем следующую за ней строку со списком участников
                            int? nextDataRow = ExcelHelper.SearchFirstDataRowAfterHeader(wsh, row, 1);
                            if (nextDataRow.HasValue)
                            {   // Нашли продолжение протокола
                                row = nextDataRow.Value - 1; // компенсируем автоинкремент

                                toNextTableHeaderRowsLeft = -1;
                            }
                            else
                            {   // Оставляем попытки что-либо найти
                                toNextTableHeaderRowsLeft = 0;
                            }
                        }
                        else
                        {
                            toNextTableHeaderRowsLeft--;
                        }
                        continue;
                    }

                    source = wsh.Cells[row, NameSurnameCol].Value?.ToString();
                    if (string.IsNullOrEmpty(source))
                    {   // Ищем следующую шапку
                        toNextTableHeaderRowsLeft = MAX_GAP_BETWEEN_REPORT_PARTS_IN_ROWS;
                        continue;
                    }
                    memberInfo.SurnameAndName = source;

                    source = wsh.Cells[row, PlaceCol].Value?.ToString();

                    memberInfo.Place = source?.ParsePlace();
                    if (string.IsNullOrEmpty(source))
                        continue;
                    else if (memberInfo.Place == null)
                    {
                        LastFillMembersFromSheetResult.Set(enFillMembersFromSheetResult.SkipWorksheet,
                                                           $"В строке {row} неверно указано место: {source ?? "<пусто>"}. Должно быть: цифра, или \"I\", \"II\", \"III\", или \"в/к\". Данные из этого листа не будут отображены.");
                        break;
                    }
                    else if (memberInfo.Place < 0)
                        continue;

                    source = wsh.Cells[row, YearOfBirthCol].Value?.ToString();
                    if (source != null)
                    {
                        int yob;
                        if (int.TryParse(source.Trim(), out yob))
                        {
                            if (yob < 1900)
                            {
                                LastFillMembersFromSheetResult.Set(enFillMembersFromSheetResult.SkipWorksheet,
                                                                    $"В строке {row} неверно указан год рождения: {source}. Должно быть: 4-хзначная цифра. Данные из этого листа не будут отображены.");
                                break;
                            }
                            memberInfo.YearOfBirth = yob;
                        }
                        else
                        {
                            LastFillMembersFromSheetResult.Set(enFillMembersFromSheetResult.SkipWorksheet,
                                                                $"В строке {row} неверно указан год рождения: {source}. Должно быть: 4-хзначная цифра. Данные из этого листа не будут отображены.");
                            break;
                        }
                    }
                    else
                    {
                        LastFillMembersFromSheetResult.Set(enFillMembersFromSheetResult.SkipWorksheet,
                                                        $"В строке {row} не указан год рождения. Должно быть: 4-хзначная цифра. Данные из этого листа не будут отображены.");
                        break;
                    }

                    source = wsh.Cells[row, InitGradeCol].Value?.ToString();
                    memberInfo.InitGrade = source?.ParseGrade();
                    if (memberInfo.InitGrade == null || memberInfo.InitGrade == enGrade.None)
                    {
                        LastFillMembersFromSheetResult.Set(enFillMembersFromSheetResult.SkipWorksheet,
                                                        $"В строке {row} неверно указан разряд: {source ?? "<пусто>"}. Данные из этого листа не будут отображены.");
                        break;
                    }
                    if (calculationSettings.ChangeJuniorGradesToAdultForAdults && memberInfo.IsAdult.Value && memberInfo.InitGrade < enGrade.Young1)
                    {
                        memberInfo.InitGrade = enGrade.Young1;
                    }

                    Members.Add(memberInfo);
                }
                catch (Exception ex)
                {
                    Members.RemoveAt(Members.Count - 1);

                    source = null;

                    LastFillMembersFromSheetResult.Set(enFillMembersFromSheetResult.CriticalError,
                                                        $"Ошибка при анализе строки {row}: \"{ex.Message}\"");
                    break;
                }
            }

            return LastFillMembersFromSheetResult;
        }

        public void CheckAndSetToDefault()
        {
            if (FirstRow == null)
                FirstRow = DefaultFirstRow;

            if (PlaceCol == null)
                PlaceCol = DefaultPlaceCol;

            if (NameSurnameCol == null)
                NameSurnameCol = DefaultNameSurnameCol;

            if (YearOfBirthCol == null)
                YearOfBirthCol = DefaultYearOfBirthCol;

            if (InitGradeCol == null)
                InitGradeCol = DefaultInitGradeCol;
        }
    }

    public class FillMembersFromSheetResult
    {
        public enFillMembersFromSheetResult Result { get; private set; } = enFillMembersFromSheetResult.OK;
        public string Message { get; private set; } = null;

        public FillMembersFromSheetResult()
        {
            Reset();
        }

        public void Reset()
        {
            Set(enFillMembersFromSheetResult.OK);
        }

        public void Set(enFillMembersFromSheetResult result, string message = null)
        {
            Result = result;
            Message = message;
        }
    }

    public enum enFillMembersFromSheetResult
    {
        OK,
        CriticalError,
        SkipWorksheet
    }
}
