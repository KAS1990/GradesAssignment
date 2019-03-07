using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MSExcel = Microsoft.Office.Interop.Excel;

namespace GradesAssignment
{
    public partial class frmMain : Form
    {
        CalculationSettings _settings = new CalculationSettings();

        public static frmMain Instance { get; private set; }

        List<WorksheetInfo> _currentWorkbookSheetInfos = null;

        #region GradesAreActual

        bool _gradesAreActual = false;
        public bool GradesAreActual
        {
            get { return _gradesAreActual; }
            set
            {
                if (_gradesAreActual != value)
                {
                    _gradesAreActual = value;
                    RefreshRecalculatingControlsStates();
                }
            }
        }

        #endregion

        #region MemberInfosAreActual

        bool _memberInfosAreActual = false;
        public bool MemberInfosAreActual
        {
            get { return _memberInfosAreActual; }
            set
            {
                if (_memberInfosAreActual != value)
                {
                    _memberInfosAreActual = value;
                    RefreshRecalculatingControlsStates();
                }
            }
        }

        #endregion

        #region SelectedWorksheetInfo

        WorksheetInfo SelectedWorksheetInfo
        {
            get
            {
                if (tbctrlWorksheets.SelectedIndex >= 0 && tbctrlWorksheets.SelectedIndex < _currentWorkbookSheetInfos.Count)
                    return _currentWorkbookSheetInfos[tbctrlWorksheets.SelectedIndex];
                else
                    return null;
            }
        }

        #endregion

        #region Modified

        bool _modified = false;
        bool _modifiedLocked = false;
        public bool Modified
        {
            get { return _modified; }
            set
            {
                if (_modifiedLocked)
                    return;

                if (_modified != value)
                {
                    _modified = value;
                    RefreshRecalculatingControlsStates();
                }
            }
        }

        #endregion

        public int CompetitionYear => _settings.CompetitionYear;

        public frmMain()
        {
            InitializeComponent();

            Instance = this;

            _modifiedLocked = true;

            SetControlsState(false);
            ClearExcelSpecificControls();

            nudCompetitionYear.Minimum = DateTime.Today.Year - 20;
            nudCompetitionYear.Maximum = DateTime.Today.Year;
            nudCompetitionYear.Value = DateTime.Today.Year;

            chk75Percent_CheckedChanged(chk75Percent, new EventArgs());
            nudCompetitionYear_ValueChanged(nudCompetitionYear, new EventArgs());
            nudMinAge_ValueChanged(nudMinAge, new EventArgs());

            cmbCalcingMethod.SelectedIndex = 0;
            cmbCompetitionStatus.SelectedIndex = 0;
            GradesAreActual = true;

            _modifiedLocked = false;
            Modified = true;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog()
            {
                AddExtension = true,
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "*.xls",
                Multiselect = false,
                Filter = "Все файлы MS Excel (*.xls, *.xlsx, *.xlsm)|*.xls;*.xlsx;*.xlsm| Файлы MS Excel 1997-2003 (*.xls)|*.xls|Файлы MS Excel 2007 (*.xlsx)|*.xlsx|Файлы MS Excel 2007 с поддержкой макросов (*.xlsm)|*.xlsm"
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtWbkFullPath.Text = dlg.FileName;
                FillWorksheetsComboBoxAsync();
            }
        }

        private void RefreshRecalculatingControlsStates()
        {
            btnCalcGrades.Enabled = MemberInfosAreActual && Modified && !GradesAreActual;
            lblRecalcGrades.Visible = btnCalcGrades.Enabled;
        }

        private void SetControlsState(bool enable)
        {
            btnAnalyseWorkbook.Enabled =
                chk75Percent.Enabled =
                nudMinAge.Enabled =
                nudCompetitionYear.Enabled =
                cmbCalcingMethod.Enabled =
                cmbCompetitionStatus.Enabled =
                tbctrlWorksheets.Enabled =
                chkChangeJuniorGradesToAdult.Enabled =
                grbColRowIndexes.Enabled =
                btnGetMembers.Enabled =
                drgdResults.Enabled = enable;

            RefreshRecalculatingControlsStates();
            btnCalcGrades.Enabled &= enable;
            lblRecalcGrades.Visible &= enable;
        }

        private void StartStopExcelOperation(bool start)
        {
            Enabled = !start;
            TopMost = start;
            RefreshRecalculatingControlsStates();
        }

        private void ClearExcelSpecificControls()
        {
            tbctrlWorksheets.SelectedIndexChanged -= tbctrlWorksheets_SelectedIndexChanged;

            MoveTableToTab(0); // Переносим таблицу на первую вкладку
            while (tbctrlWorksheets.TabPages.Count > 1)
                tbctrlWorksheets.TabPages.RemoveAt(1);
            tbctrlWorksheets.TabPages[0].Text = "";

            tbctrlWorksheets.SelectedIndexChanged += tbctrlWorksheets_SelectedIndexChanged;

            drgdResults.Rows.Clear();
        }

        private bool CheckSettings()
        {
            if (SelectedWorksheetInfo == null)
            {
                MessageBox.Show(this, "Не выбран лист", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (_settings.MaxYear < 0)
            {
                MessageBox.Show(this, "Не выбран минимальный возраст", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (_settings.CalcMethod == enResultGradeCalcMethod.None)
            {
                MessageBox.Show(this, "Не выбран метод для вычисления мест", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (_settings.CompetitionStatus == enCompetitionStatus.None)
            {
                MessageBox.Show(this, "Не выбран статус соревнований", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        #region Show members

        private async void FillWorksheetsComboBoxAsync()
        {
            StartStopExcelOperation(true);

            _modifiedLocked = true;

            SetControlsState(false);
            ClearExcelSpecificControls();

            var waitingForm = new frmWaiting(this, 0);

            var wbkFullPath = txtWbkFullPath.Text;
            var result = await Task.Factory.StartNew(() =>
            {
                string message;
                var members = ExcelHelper.GetWorksheetInfo(wbkFullPath, _settings, out message);
                
                return new { members, message };
            });

            _currentWorkbookSheetInfos = result.members;

            if (_currentWorkbookSheetInfos.Count > 0)
            {
                tbctrlWorksheets.TabPages[0].Text = CreateTabHeader(_currentWorkbookSheetInfos[0]);
                for (int i = 1; i < _currentWorkbookSheetInfos.Count; i++)
                {
                    tbctrlWorksheets.TabPages.Add(CreateTabHeader(_currentWorkbookSheetInfos[i]));
                }
            }

            if (!string.IsNullOrEmpty(result.message))
            {
                MessageBox.Show(waitingForm, $"Не удалось проанализировать книгу. Ошибка: {result.message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            if (tbctrlWorksheets.SelectedIndex != 0)
                tbctrlWorksheets.SelectedIndex = 0;
            else
                tbctrlWorksheets_SelectedIndexChanged(tbctrlWorksheets, new EventArgs());

            _modifiedLocked = false;

            GradesAreActual = false;
            Modified = true;
            SetControlsState(true);
                        
            waitingForm.AllowClose = true;
            waitingForm.Close();

            StartStopExcelOperation(false);

            Activate();
        }

        private async void FillMembersFromSheetAsync()
        {
            if (SelectedWorksheetInfo == null)
            {
                MessageBox.Show(this, $"Не выбран лист", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
                        
            StartStopExcelOperation(true);

            _modifiedLocked = true;

            SetControlsState(false);
            drgdResults.Rows.Clear();

            var waitingForm = new frmWaiting(this, 0);

            SelectedWorksheetInfo.CheckAndSetToDefault();

            var wbkFullPath = txtWbkFullPath.Text;
            var selectedSheetIndex = tbctrlWorksheets.SelectedIndex;
            var selectedWorksheetInfo = SelectedWorksheetInfo;

            await Task.Factory.StartNew(() =>
            {
                ExcelHelper.FillMembersFromSheet(wbkFullPath, selectedSheetIndex + 1, _settings, selectedWorksheetInfo);
            });

            switch (selectedWorksheetInfo.LastFillMembersFromSheetResult.Result)
            {
                case enFillMembersFromSheetResult.CriticalError:
                    MessageBox.Show(waitingForm, $"Не удалось прочитать сведения о спортсменах из книги. Ошибка: {selectedWorksheetInfo.LastFillMembersFromSheetResult.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                case enFillMembersFromSheetResult.SkipWorksheet:
                    MessageBox.Show(waitingForm, $"При анализе листа {selectedWorksheetInfo.Name} произошла ошибка: {selectedWorksheetInfo.LastFillMembersFromSheetResult.Message}",
                        "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
            }
            
            ShowMembers(SelectedWorksheetInfo.Members);

            tbctrlWorksheets.SelectedTab.Text = CreateTabHeader(SelectedWorksheetInfo);
            
            _modifiedLocked = false;

            GradesAreActual = false;
            Modified = true;
            SetControlsState(true);

            waitingForm.AllowClose = true;
            waitingForm.Close();

            StartStopExcelOperation(false);

            Activate();
        }

        private void ShowMembers(List<MemberInfo> members)
        {
            drgdResults.Rows.Clear();

            int row = 0;
            foreach (var member in members.OrderBy(member => member.Place))
            {
                drgdResults.Rows.Add(member.Place, member.SurnameAndName, member.YearOfBirth, member.InitGradeForShow,
                                        member.ResultGradeForShow, member.ResultGradeBefore2018ForShow, member.AssignmentStatusForShow);
                if (member.UsedInCalculating)
                    drgdResults.Rows[row].DefaultCellStyle.BackColor = Color.AliceBlue;
                else
                    drgdResults.Rows[row].DefaultCellStyle.BackColor = Color.White;

                if (member.AssignmentStatus == enAssignmentStatus.Improve)
                    drgdResults.Rows[row].Cells[4].Style.Font = new Font(drgdResults.Font, FontStyle.Bold);

                row++;
            }

            MemberInfosAreActual = true;
        }

        private string CreateTabHeader(WorksheetInfo worksheetInfo)
        {
            var result = worksheetInfo.Name;
            if (worksheetInfo.LastFillMembersFromSheetResult.Result != enFillMembersFromSheetResult.OK
                 || !worksheetInfo.AllColAndRowIndexesAreFilled)
            {
                result += "*";
            }

            return result;
        }
                
        private void MoveTableToTab(int tabIndex)
        {
            drgdResults.Parent = tbctrlWorksheets.TabPages[tabIndex];
        }

        private void tbctrlWorksheets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbctrlWorksheets.SelectedIndex >= 0)
            {
                // Перемещаем таблицу на отображаемую вкладку, чтобы не создавать отдельную таблицу на каждой вкладке
                MoveTableToTab(tbctrlWorksheets.SelectedIndex);

                if (tbctrlWorksheets.SelectedIndex < (_currentWorkbookSheetInfos?.Count ?? 0))
                {
                    int parsedValue = 0;
                    List<string> messages = new List<string>();

                    #region mtxtFirstRow

                    if (SelectedWorksheetInfo.FirstRow.HasValue)
                        parsedValue = SelectedWorksheetInfo.FirstRow.Value;
                    else
                    {
                        parsedValue = WorksheetInfo.DefaultFirstRow;
                        messages.Add($"Не удалось автоматически найти шапку листа \"{SelectedWorksheetInfo.Name}\". Номер первой строки установлен в значение по умолчанию: {parsedValue}");

                    }
                    mtxtFirstRow.Text = parsedValue.ToString();

                    #endregion

                    #region mtxtPlaceCol

                    if (SelectedWorksheetInfo.PlaceCol.HasValue)
                        parsedValue = SelectedWorksheetInfo.PlaceCol.Value;
                    else
                    {
                        parsedValue = WorksheetInfo.DefaultPlaceCol;
                        messages.Add($"В листе \"{SelectedWorksheetInfo.Name}\" не удалось автоматически найти столбец Место. Его номер установлен в значение по умолчанию: {parsedValue}");
                    }
                    mtxtPlaceCol.Text = parsedValue.ToString();

                    #endregion

                    #region mtxtNameSurnameCol

                    if (SelectedWorksheetInfo.NameSurnameCol.HasValue)
                        parsedValue = SelectedWorksheetInfo.NameSurnameCol.Value;
                    else
                    {
                        parsedValue = WorksheetInfo.DefaultNameSurnameCol;
                        messages.Add($"В листе \"{SelectedWorksheetInfo.Name}\" не удалось автоматически найти столбец Фамилия Имя. Его номер установлен в значение по умолчанию: {parsedValue}");
                    }
                    mtxtNameSurnameCol.Text = parsedValue.ToString();

                    #endregion

                    #region mtxtYearOfBirthCol

                    if (SelectedWorksheetInfo.YearOfBirthCol.HasValue)
                        parsedValue = SelectedWorksheetInfo.YearOfBirthCol.Value;
                    else
                    {
                        parsedValue = WorksheetInfo.DefaultYearOfBirthCol;
                        messages.Add($"В листе \"{SelectedWorksheetInfo.Name}\" не удалось автоматически найти столбец Год рождения. Его номер установлен в значение по умолчанию: {parsedValue}");
                    }
                    mtxtYearOfBirthCol.Text = parsedValue.ToString();

                    #endregion

                    #region mtxtInitGradeCol

                    if (SelectedWorksheetInfo.InitGradeCol.HasValue)
                        parsedValue = SelectedWorksheetInfo.InitGradeCol.Value;
                    else
                    {
                        parsedValue = WorksheetInfo.DefaultInitGradeCol;
                        messages.Add($"В листе \"{SelectedWorksheetInfo.Name}\" не удалось автоматически найти столбец Разряд. Его номер установлен в значение по умолчанию: {parsedValue}");
                    }
                    mtxtInitGradeCol.Text = parsedValue.ToString();

                    #endregion

                    if (SelectedWorksheetInfo.LastFillMembersFromSheetResult.Result == enFillMembersFromSheetResult.CriticalError)
                    {
                        MessageBox.Show(this,
                            string.Format("В ходе анализа листа \"{0}\" произошла следующие ошибка:\n\r{1}",
                                            SelectedWorksheetInfo.Name, SelectedWorksheetInfo.LastFillMembersFromSheetResult.Message),
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    if (SelectedWorksheetInfo.LastFillMembersFromSheetResult.Result == enFillMembersFromSheetResult.SkipWorksheet)
                    {
                        messages.Add(string.Format("В ходе анализа листа \"{0}\" возникла проблема:\n\r{1}",
                                            SelectedWorksheetInfo.Name, SelectedWorksheetInfo.LastFillMembersFromSheetResult.Message));
                    }

                    if (messages.Count > 0)
                    {
                        MessageBox.Show(this, string.Join("\n\r", messages), "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                    ShowMembers(SelectedWorksheetInfo.Members);

                    Modified = true;
                    GradesAreActual = false;
                }
            }
        }

        private void btnAnalyseWorkbook_Click(object sender, EventArgs e)
        {
            FillWorksheetsComboBoxAsync();
        }

        private void btnGetMembers_Click(object sender, EventArgs e)
        {
            FillMembersFromSheetAsync();
        }

        #endregion

        private void btnCalcGrades_Click(object sender, EventArgs e)
        {
            if (SelectedWorksheetInfo == null)
            {
                MessageBox.Show(this, $"Не выбран лист", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string message;
            GradeCalculations.CalculateGrades(SelectedWorksheetInfo.Members, _settings, out message);

            if (!string.IsNullOrEmpty(message))
            {
                MessageBox.Show(this, $"Не удалось рассчитать разряды. Ошибка: {message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ShowMembers(SelectedWorksheetInfo.Members);
                        
            GradesAreActual = true;
            Modified = false;
        }

        #region Calculating Settings

        private void chk75Percent_CheckedChanged(object sender, EventArgs e)
        {
            _settings.UseOnly75Percent = chk75Percent.Checked;
            Modified = true;
            GradesAreActual = false;
        }
                
        private void nudMinAge_ValueChanged(object sender, EventArgs e)
        {
            _settings.MaxYear = _settings.CompetitionYear - (int)nudMinAge.Value;
            lblMaxYear.Text = $"лет (c {_settings.MaxYear} г.р.)";
            Modified = true;
            GradesAreActual = false;
        }

        private void cmbCalcingMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            _settings.CalcMethod = (enResultGradeCalcMethod)cmbCalcingMethod.SelectedIndex;
            Modified = true;
            GradesAreActual = false;
        }

        private void cmbCompetitionStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            _settings.CompetitionStatus = (enCompetitionStatus)cmbCompetitionStatus.SelectedIndex;
            Modified = true;
            GradesAreActual = false;
        }

        private void nudCompetitionYear_ValueChanged(object sender, EventArgs e)
        {
            _settings.CompetitionYear = (int)nudCompetitionYear.Value;
            Modified = true;
            GradesAreActual = false;
        }

        #endregion
                
        #region Exctracting Data From Excel Settings

        private void mtxtColRowIndex_TextChanged(object sender, EventArgs e)
        {
            Modified = true;
            GradesAreActual = false;
            MemberInfosAreActual = false;
        }

        private void mtxtFirstRow_Leave(object sender, EventArgs e)
        {
            if (SelectedWorksheetInfo != null)
            {
                SelectedWorksheetInfo.FirstRow = ParseMaskedTextBox(mtxtFirstRow,
                    SelectedWorksheetInfo.FirstRow ?? WorksheetInfo.DefaultFirstRow);
            }
        }

        private void mtxtPlaceCol_Leave(object sender, EventArgs e)
        {
            if (SelectedWorksheetInfo != null)
            {
                SelectedWorksheetInfo.PlaceCol = ParseMaskedTextBox(mtxtPlaceCol,
                    SelectedWorksheetInfo.PlaceCol ?? WorksheetInfo.DefaultPlaceCol);
            }
        }

        private void mtxtNameSurnameCol_Leave(object sender, EventArgs e)
        {
            if (SelectedWorksheetInfo != null)
            {
                SelectedWorksheetInfo.NameSurnameCol = ParseMaskedTextBox(mtxtNameSurnameCol,
                    SelectedWorksheetInfo.NameSurnameCol ?? WorksheetInfo.DefaultNameSurnameCol);
            }
        }

        private void mtxtYearOfBirthCol_Leave(object sender, EventArgs e)
        {
            if (SelectedWorksheetInfo != null)
            {
                SelectedWorksheetInfo.YearOfBirthCol = ParseMaskedTextBox(mtxtYearOfBirthCol,
                    SelectedWorksheetInfo.YearOfBirthCol ?? WorksheetInfo.DefaultYearOfBirthCol);
            }
        }

        private void mtxtInitGradeCol_Leave(object sender, EventArgs e)
        {
            if (SelectedWorksheetInfo != null)
            {
                SelectedWorksheetInfo.InitGradeCol = ParseMaskedTextBox(mtxtInitGradeCol,
                    SelectedWorksheetInfo.InitGradeCol ?? WorksheetInfo.DefaultInitGradeCol);
            }
        }

        private int ParseMaskedTextBox(MaskedTextBox mtxt, int defaultValue)
        {
            int result;
            if (int.TryParse(mtxt.Text, out result))
                return result;
            else
                return defaultValue;
        }

        private void chkChangeJuniorGradesToAdult_CheckedChanged(object sender, EventArgs e)
        {
            _settings.ChangeJuniorGradesToAdultForAdults = chkChangeJuniorGradesToAdult.Checked;
            MemberInfosAreActual = false;
            Modified = true;
            GradesAreActual = false;
        }

        #endregion

        #region Workbook Drag Drop Support

        private bool DataCanBeDropped(DragEventArgs e, out string fullFilePath)
        {
            fullFilePath = "";
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] fullFilePaths = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (fullFilePaths.Length == 1)
                {
                    fullFilePath = fullFilePaths[0];
                    var ext = Path.GetExtension(fullFilePath);
                    return ext == ".xls" || ext == ".xlsx" || ext == ".xlsm";
                }
                else
                    return false;
            }
            else
                return false;
        }

        private void txtWbkFullPath_DragOver(object sender, DragEventArgs e)
        {
            string fullFilePath;
            e.Effect = DataCanBeDropped(e, out fullFilePath) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void txtWbkFullPath_DragDrop(object sender, DragEventArgs e)
        {
            string fullFilePath;
            if (DataCanBeDropped(e, out fullFilePath))
            {
                txtWbkFullPath.Text = fullFilePath;
                FillWorksheetsComboBoxAsync();
            }
        }

        #endregion

        private void mmiAbout_Click(object sender, EventArgs e)
        {
            AboutBox wnd = new AboutBox()
            {
                Owner = this
            };

            wnd.ShowDialog();
        }
    }
}
