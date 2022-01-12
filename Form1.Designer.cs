namespace GradesAssignment
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.txtWbkFullPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.chk75Percent = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblMaxYear = new System.Windows.Forms.Label();
            this.cmbCalcingMethod = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCalcGrades = new System.Windows.Forms.Button();
            this.drgdResults = new System.Windows.Forms.DataGridView();
            this.colPlace = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSurnameName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colYearOfBirth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGrade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResultGrade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrior2018 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblRecalcGrades = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.mtxtFirstRow = new System.Windows.Forms.MaskedTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.grbColRowIndexes = new System.Windows.Forms.GroupBox();
            this.mtxtInitGradeCol = new System.Windows.Forms.MaskedTextBox();
            this.mtxtYearOfBirthCol = new System.Windows.Forms.MaskedTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.mtxtNameSurnameCol = new System.Windows.Forms.MaskedTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.mtxtPlaceCol = new System.Windows.Forms.MaskedTextBox();
            this.btnAnalyseWorkbook = new System.Windows.Forms.Button();
            this.btnGetMembers = new System.Windows.Forms.Button();
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.mmiAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.chkChangeJuniorGradesToAdult = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbCompetitionStatus = new System.Windows.Forms.ComboBox();
            this.tbctrlWorksheets = new System.Windows.Forms.TabControl();
            this.tpg1 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.nudCompetitionYear = new System.Windows.Forms.NumericUpDown();
            this.nudMinAge = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.drgdResults)).BeginInit();
            this.grbColRowIndexes.SuspendLayout();
            this.menuMain.SuspendLayout();
            this.tbctrlWorksheets.SuspendLayout();
            this.tpg1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCompetitionYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinAge)).BeginInit();
            this.SuspendLayout();
            // 
            // txtWbkFullPath
            // 
            this.txtWbkFullPath.AllowDrop = true;
            this.txtWbkFullPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWbkFullPath.Location = new System.Drawing.Point(169, 33);
            this.txtWbkFullPath.Name = "txtWbkFullPath";
            this.txtWbkFullPath.ReadOnly = true;
            this.txtWbkFullPath.Size = new System.Drawing.Size(604, 20);
            this.txtWbkFullPath.TabIndex = 0;
            this.txtWbkFullPath.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtWbkFullPath_DragDrop);
            this.txtWbkFullPath.DragOver += new System.Windows.Forms.DragEventHandler(this.txtWbkFullPath_DragOver);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Путь к книге c протоколами";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(779, 33);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(38, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // chk75Percent
            // 
            this.chk75Percent.AutoSize = true;
            this.chk75Percent.Enabled = false;
            this.chk75Percent.Location = new System.Drawing.Point(15, 64);
            this.chk75Percent.Name = "chk75Percent";
            this.chk75Percent.Size = new System.Drawing.Size(202, 17);
            this.chk75Percent.TabIndex = 4;
            this.chk75Percent.Text = "Учитывать только 75% участников";
            this.chk75Percent.UseVisualStyleBackColor = true;
            this.chk75Percent.Visible = false;
            this.chk75Percent.CheckedChanged += new System.EventHandler(this.chk75Percent_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(472, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Разряды присваиваются с";
            // 
            // lblMaxYear
            // 
            this.lblMaxYear.AutoSize = true;
            this.lblMaxYear.Location = new System.Drawing.Point(668, 69);
            this.lblMaxYear.Name = "lblMaxYear";
            this.lblMaxYear.Size = new System.Drawing.Size(0, 13);
            this.lblMaxYear.TabIndex = 8;
            // 
            // cmbCalcingMethod
            // 
            this.cmbCalcingMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCalcingMethod.Enabled = false;
            this.cmbCalcingMethod.FormattingEnabled = true;
            this.cmbCalcingMethod.Items.AddRange(new object[] {
            "Это место и выше (округление \"вниз\") - как по разрядным требованиям до 2021",
            "Это место и выше (\"математическое\" округление) - как по разрядным требованиям с 2" +
                "022"});
            this.cmbCalcingMethod.Location = new System.Drawing.Point(170, 88);
            this.cmbCalcingMethod.Name = "cmbCalcingMethod";
            this.cmbCalcingMethod.Size = new System.Drawing.Size(490, 21);
            this.cmbCalcingMethod.TabIndex = 11;
            this.cmbCalcingMethod.SelectedIndexChanged += new System.EventHandler(this.cmbCalcingMethod_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(152, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Метод для вычисления мест";
            // 
            // btnCalcGrades
            // 
            this.btnCalcGrades.Enabled = false;
            this.btnCalcGrades.Location = new System.Drawing.Point(257, 277);
            this.btnCalcGrades.Name = "btnCalcGrades";
            this.btnCalcGrades.Size = new System.Drawing.Size(133, 23);
            this.btnCalcGrades.TabIndex = 12;
            this.btnCalcGrades.Text = "Посчитать разряды";
            this.btnCalcGrades.UseVisualStyleBackColor = true;
            this.btnCalcGrades.Click += new System.EventHandler(this.btnCalcGrades_Click);
            // 
            // drgdResults
            // 
            this.drgdResults.AllowUserToAddRows = false;
            this.drgdResults.AllowUserToDeleteRows = false;
            this.drgdResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.drgdResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.drgdResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPlace,
            this.colSurnameName,
            this.colYearOfBirth,
            this.colGrade,
            this.colResultGrade,
            this.colPrior2018,
            this.colStatus});
            this.drgdResults.Enabled = false;
            this.drgdResults.Location = new System.Drawing.Point(3, 3);
            this.drgdResults.Name = "drgdResults";
            this.drgdResults.ReadOnly = true;
            this.drgdResults.Size = new System.Drawing.Size(873, 303);
            this.drgdResults.TabIndex = 13;
            // 
            // colPlace
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colPlace.DefaultCellStyle = dataGridViewCellStyle1;
            this.colPlace.Frozen = true;
            this.colPlace.HeaderText = "Место";
            this.colPlace.Name = "colPlace";
            this.colPlace.ReadOnly = true;
            this.colPlace.Width = 50;
            // 
            // colSurnameName
            // 
            this.colSurnameName.HeaderText = "Фамилия, Имя";
            this.colSurnameName.Name = "colSurnameName";
            this.colSurnameName.ReadOnly = true;
            this.colSurnameName.Width = 200;
            // 
            // colYearOfBirth
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colYearOfBirth.DefaultCellStyle = dataGridViewCellStyle2;
            this.colYearOfBirth.HeaderText = "Г.р.";
            this.colYearOfBirth.Name = "colYearOfBirth";
            this.colYearOfBirth.ReadOnly = true;
            this.colYearOfBirth.Width = 50;
            // 
            // colGrade
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colGrade.DefaultCellStyle = dataGridViewCellStyle3;
            this.colGrade.HeaderText = "Разряд";
            this.colGrade.Name = "colGrade";
            this.colGrade.ReadOnly = true;
            this.colGrade.Width = 50;
            // 
            // colResultGrade
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colResultGrade.DefaultCellStyle = dataGridViewCellStyle4;
            this.colResultGrade.HeaderText = "Выполненный разряд";
            this.colResultGrade.Name = "colResultGrade";
            this.colResultGrade.ReadOnly = true;
            // 
            // colPrior2018
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colPrior2018.DefaultCellStyle = dataGridViewCellStyle5;
            this.colPrior2018.HeaderText = "До 2018";
            this.colPrior2018.Name = "colPrior2018";
            this.colPrior2018.ReadOnly = true;
            // 
            // colStatus
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colStatus.DefaultCellStyle = dataGridViewCellStyle6;
            this.colStatus.HeaderText = "Статус";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            // 
            // lblRecalcGrades
            // 
            this.lblRecalcGrades.AutoSize = true;
            this.lblRecalcGrades.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblRecalcGrades.ForeColor = System.Drawing.Color.Maroon;
            this.lblRecalcGrades.Location = new System.Drawing.Point(396, 282);
            this.lblRecalcGrades.Name = "lblRecalcGrades";
            this.lblRecalcGrades.Size = new System.Drawing.Size(157, 13);
            this.lblRecalcGrades.TabIndex = 14;
            this.lblRecalcGrades.Text = "Пересчитайте разряды!!!";
            this.lblRecalcGrades.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(118, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Номер первой строки";
            // 
            // mtxtFirstRow
            // 
            this.mtxtFirstRow.Location = new System.Drawing.Point(175, 15);
            this.mtxtFirstRow.Mask = "##";
            this.mtxtFirstRow.Name = "mtxtFirstRow";
            this.mtxtFirstRow.Size = new System.Drawing.Size(34, 20);
            this.mtxtFirstRow.TabIndex = 16;
            this.mtxtFirstRow.Text = "8";
            this.mtxtFirstRow.TextChanged += new System.EventHandler(this.mtxtColRowIndex_TextChanged);
            this.mtxtFirstRow.Leave += new System.EventHandler(this.mtxtFirstRow_Leave);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Номер столбца Место";
            // 
            // grbColRowIndexes
            // 
            this.grbColRowIndexes.Controls.Add(this.mtxtInitGradeCol);
            this.grbColRowIndexes.Controls.Add(this.mtxtYearOfBirthCol);
            this.grbColRowIndexes.Controls.Add(this.label9);
            this.grbColRowIndexes.Controls.Add(this.label8);
            this.grbColRowIndexes.Controls.Add(this.mtxtNameSurnameCol);
            this.grbColRowIndexes.Controls.Add(this.label7);
            this.grbColRowIndexes.Controls.Add(this.mtxtPlaceCol);
            this.grbColRowIndexes.Controls.Add(this.label5);
            this.grbColRowIndexes.Controls.Add(this.label6);
            this.grbColRowIndexes.Controls.Add(this.mtxtFirstRow);
            this.grbColRowIndexes.Location = new System.Drawing.Point(12, 175);
            this.grbColRowIndexes.Name = "grbColRowIndexes";
            this.grbColRowIndexes.Size = new System.Drawing.Size(677, 96);
            this.grbColRowIndexes.TabIndex = 18;
            this.grbColRowIndexes.TabStop = false;
            this.grbColRowIndexes.Text = "Номера строк и столбцов";
            // 
            // mtxtInitGradeCol
            // 
            this.mtxtInitGradeCol.Location = new System.Drawing.Point(400, 66);
            this.mtxtInitGradeCol.Mask = "##";
            this.mtxtInitGradeCol.Name = "mtxtInitGradeCol";
            this.mtxtInitGradeCol.Size = new System.Drawing.Size(34, 20);
            this.mtxtInitGradeCol.TabIndex = 24;
            this.mtxtInitGradeCol.Text = "5";
            this.mtxtInitGradeCol.TextChanged += new System.EventHandler(this.mtxtColRowIndex_TextChanged);
            this.mtxtInitGradeCol.Leave += new System.EventHandler(this.mtxtInitGradeCol_Leave);
            // 
            // mtxtYearOfBirthCol
            // 
            this.mtxtYearOfBirthCol.Location = new System.Drawing.Point(400, 40);
            this.mtxtYearOfBirthCol.Mask = "##";
            this.mtxtYearOfBirthCol.Name = "mtxtYearOfBirthCol";
            this.mtxtYearOfBirthCol.Size = new System.Drawing.Size(34, 20);
            this.mtxtYearOfBirthCol.TabIndex = 23;
            this.mtxtYearOfBirthCol.Text = "4";
            this.mtxtYearOfBirthCol.TextChanged += new System.EventHandler(this.mtxtColRowIndex_TextChanged);
            this.mtxtYearOfBirthCol.Leave += new System.EventHandler(this.mtxtYearOfBirthCol_Leave);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(232, 70);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(125, 13);
            this.label9.TabIndex = 22;
            this.label9.Text = "Номер столбца Разряд";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(232, 43);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(159, 13);
            this.label8.TabIndex = 21;
            this.label8.Text = "Номер столбца Год рождения";
            // 
            // mtxtNameSurnameCol
            // 
            this.mtxtNameSurnameCol.Location = new System.Drawing.Point(175, 67);
            this.mtxtNameSurnameCol.Mask = "##";
            this.mtxtNameSurnameCol.Name = "mtxtNameSurnameCol";
            this.mtxtNameSurnameCol.Size = new System.Drawing.Size(34, 20);
            this.mtxtNameSurnameCol.TabIndex = 20;
            this.mtxtNameSurnameCol.Text = "2";
            this.mtxtNameSurnameCol.TextChanged += new System.EventHandler(this.mtxtColRowIndex_TextChanged);
            this.mtxtNameSurnameCol.Leave += new System.EventHandler(this.mtxtNameSurnameCol_Leave);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 70);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(162, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Номер столбца Фамилия Имя";
            // 
            // mtxtPlaceCol
            // 
            this.mtxtPlaceCol.Location = new System.Drawing.Point(175, 40);
            this.mtxtPlaceCol.Mask = "##";
            this.mtxtPlaceCol.Name = "mtxtPlaceCol";
            this.mtxtPlaceCol.Size = new System.Drawing.Size(34, 20);
            this.mtxtPlaceCol.TabIndex = 18;
            this.mtxtPlaceCol.Text = "1";
            this.mtxtPlaceCol.TextChanged += new System.EventHandler(this.mtxtColRowIndex_TextChanged);
            this.mtxtPlaceCol.Leave += new System.EventHandler(this.mtxtPlaceCol_Leave);
            // 
            // btnAnalyseWorkbook
            // 
            this.btnAnalyseWorkbook.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAnalyseWorkbook.Location = new System.Drawing.Point(823, 31);
            this.btnAnalyseWorkbook.Name = "btnAnalyseWorkbook";
            this.btnAnalyseWorkbook.Size = new System.Drawing.Size(75, 48);
            this.btnAnalyseWorkbook.TabIndex = 19;
            this.btnAnalyseWorkbook.Text = "Анализ протокола";
            this.btnAnalyseWorkbook.UseVisualStyleBackColor = true;
            this.btnAnalyseWorkbook.Click += new System.EventHandler(this.btnAnalyseWorkbook_Click);
            // 
            // btnGetMembers
            // 
            this.btnGetMembers.Location = new System.Drawing.Point(12, 277);
            this.btnGetMembers.Name = "btnGetMembers";
            this.btnGetMembers.Size = new System.Drawing.Size(206, 23);
            this.btnGetMembers.TabIndex = 20;
            this.btnGetMembers.Text = "Получить сведения об участниках";
            this.btnGetMembers.UseVisualStyleBackColor = true;
            this.btnGetMembers.Click += new System.EventHandler(this.btnGetMembers_Click);
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmiAbout});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(910, 24);
            this.menuMain.TabIndex = 22;
            this.menuMain.Text = "menuStrip1";
            // 
            // mmiAbout
            // 
            this.mmiAbout.Name = "mmiAbout";
            this.mmiAbout.Size = new System.Drawing.Size(94, 20);
            this.mmiAbout.Text = "О программе";
            this.mmiAbout.Click += new System.EventHandler(this.mmiAbout_Click);
            // 
            // chkChangeJuniorGradesToAdult
            // 
            this.chkChangeJuniorGradesToAdult.AutoSize = true;
            this.chkChangeJuniorGradesToAdult.Checked = true;
            this.chkChangeJuniorGradesToAdult.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkChangeJuniorGradesToAdult.Location = new System.Drawing.Point(12, 152);
            this.chkChangeJuniorGradesToAdult.Name = "chkChangeJuniorGradesToAdult";
            this.chkChangeJuniorGradesToAdult.Size = new System.Drawing.Size(745, 17);
            this.chkChangeJuniorGradesToAdult.TabIndex = 23;
            this.chkChangeJuniorGradesToAdult.Text = "Автоматически записывать разряд \"1 ю\" всем спортсменам старше 17 лет, у которых н" +
    "ет разряда или которые имеют юношеские разряды";
            this.chkChangeJuniorGradesToAdult.UseVisualStyleBackColor = true;
            this.chkChangeJuniorGradesToAdult.CheckedChanged += new System.EventHandler(this.chkChangeJuniorGradesToAdult_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 118);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(116, 13);
            this.label10.TabIndex = 24;
            this.label10.Text = "Статус соревнования";
            // 
            // cmbCompetitionStatus
            // 
            this.cmbCompetitionStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCompetitionStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCompetitionStatus.Enabled = false;
            this.cmbCompetitionStatus.FormattingEnabled = true;
            this.cmbCompetitionStatus.Items.AddRange(new object[] {
            "Соревнования, на которых нельзя выполнить КМС и МС",
            "Первенство мира, Юношеские Олимпийские игры",
            "Всемирная универсиада",
            "Первенство Европы",
            "Другие международные спортивные соревнования, включенные в ЕКП",
            "Первенство мира среди студентов",
            "Чемпионат России",
            "Кубок России",
            "Первенство России, Всероссийская спартакиада между субъектами Российской Федераци" +
                "и",
            "Другие всероссийские спортивные соревнования, включенные в ЕКП",
            "Чемпионат федерального округа, двух и более федеральных округов, чемпионаты г. Мо" +
                "сквы и г. Санкт-Петербурга",
            "Первенство федерального округа, двух и более федеральных округов, Спартакиада одн" +
                "ого или двух и более федеральных округов, первенства г. Москвы и г. Санкт-Петерб" +
                "урга",
            "Чемпионат субъекта Российской Федерации (кроме г. Москвы и г. Санкт-Петербурга)"});
            this.cmbCompetitionStatus.Location = new System.Drawing.Point(170, 115);
            this.cmbCompetitionStatus.Name = "cmbCompetitionStatus";
            this.cmbCompetitionStatus.Size = new System.Drawing.Size(728, 21);
            this.cmbCompetitionStatus.TabIndex = 25;
            this.cmbCompetitionStatus.SelectedIndexChanged += new System.EventHandler(this.cmbCompetitionStatus_SelectedIndexChanged);
            // 
            // tbctrlWorksheets
            // 
            this.tbctrlWorksheets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbctrlWorksheets.Controls.Add(this.tpg1);
            this.tbctrlWorksheets.ItemSize = new System.Drawing.Size(150, 23);
            this.tbctrlWorksheets.Location = new System.Drawing.Point(12, 316);
            this.tbctrlWorksheets.Name = "tbctrlWorksheets";
            this.tbctrlWorksheets.SelectedIndex = 0;
            this.tbctrlWorksheets.ShowToolTips = true;
            this.tbctrlWorksheets.Size = new System.Drawing.Size(890, 343);
            this.tbctrlWorksheets.TabIndex = 26;
            this.tbctrlWorksheets.SelectedIndexChanged += new System.EventHandler(this.tbctrlWorksheets_SelectedIndexChanged);
            // 
            // tpg1
            // 
            this.tpg1.Controls.Add(this.drgdResults);
            this.tpg1.Location = new System.Drawing.Point(4, 27);
            this.tpg1.Name = "tpg1";
            this.tpg1.Padding = new System.Windows.Forms.Padding(3);
            this.tpg1.Size = new System.Drawing.Size(882, 312);
            this.tpg1.TabIndex = 0;
            this.tpg1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(223, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(163, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "Год проведения соревнований";
            // 
            // nudCompetitionYear
            // 
            this.nudCompetitionYear.Location = new System.Drawing.Point(388, 63);
            this.nudCompetitionYear.Maximum = new decimal(new int[] {
            2019,
            0,
            0,
            0});
            this.nudCompetitionYear.Minimum = new decimal(new int[] {
            1990,
            0,
            0,
            0});
            this.nudCompetitionYear.Name = "nudCompetitionYear";
            this.nudCompetitionYear.Size = new System.Drawing.Size(58, 20);
            this.nudCompetitionYear.TabIndex = 28;
            this.nudCompetitionYear.Value = new decimal(new int[] {
            1999,
            0,
            0,
            0});
            this.nudCompetitionYear.ValueChanged += new System.EventHandler(this.nudCompetitionYear_ValueChanged);
            // 
            // nudMinAge
            // 
            this.nudMinAge.Location = new System.Drawing.Point(621, 64);
            this.nudMinAge.Minimum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.nudMinAge.Name = "nudMinAge";
            this.nudMinAge.Size = new System.Drawing.Size(39, 20);
            this.nudMinAge.TabIndex = 29;
            this.nudMinAge.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudMinAge.ValueChanged += new System.EventHandler(this.nudMinAge_ValueChanged);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(910, 671);
            this.Controls.Add(this.nudMinAge);
            this.Controls.Add(this.nudCompetitionYear);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbctrlWorksheets);
            this.Controls.Add(this.cmbCompetitionStatus);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.chkChangeJuniorGradesToAdult);
            this.Controls.Add(this.btnGetMembers);
            this.Controls.Add(this.btnAnalyseWorkbook);
            this.Controls.Add(this.grbColRowIndexes);
            this.Controls.Add(this.lblRecalcGrades);
            this.Controls.Add(this.btnCalcGrades);
            this.Controls.Add(this.cmbCalcingMethod);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblMaxYear);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chk75Percent);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtWbkFullPath);
            this.Controls.Add(this.menuMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMain;
            this.Name = "frmMain";
            this.Text = "Выполнение разрядов с 2018 (МС - 3 ю)";
            ((System.ComponentModel.ISupportInitialize)(this.drgdResults)).EndInit();
            this.grbColRowIndexes.ResumeLayout(false);
            this.grbColRowIndexes.PerformLayout();
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.tbctrlWorksheets.ResumeLayout(false);
            this.tpg1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudCompetitionYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinAge)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtWbkFullPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.CheckBox chk75Percent;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblMaxYear;
        private System.Windows.Forms.ComboBox cmbCalcingMethod;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCalcGrades;
        private System.Windows.Forms.DataGridView drgdResults;
        private System.Windows.Forms.Label lblRecalcGrades;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.MaskedTextBox mtxtFirstRow;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox grbColRowIndexes;
        private System.Windows.Forms.MaskedTextBox mtxtInitGradeCol;
        private System.Windows.Forms.MaskedTextBox mtxtYearOfBirthCol;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.MaskedTextBox mtxtNameSurnameCol;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.MaskedTextBox mtxtPlaceCol;
        private System.Windows.Forms.Button btnAnalyseWorkbook;
        private System.Windows.Forms.Button btnGetMembers;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPlace;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSurnameName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colYearOfBirth;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGrade;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResultGrade;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrior2018;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem mmiAbout;
        private System.Windows.Forms.CheckBox chkChangeJuniorGradesToAdult;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbCompetitionStatus;
        private System.Windows.Forms.TabControl tbctrlWorksheets;
        private System.Windows.Forms.TabPage tpg1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudCompetitionYear;
        private System.Windows.Forms.NumericUpDown nudMinAge;
    }
}

