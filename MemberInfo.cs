using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GradesAssignment
{
    public class MemberInfo
    {
        public int? Place { get; set; }
        public string SurnameAndName { get; set; }

        public int? YearOfBirth { get; set; }
        public int? Age => YearOfBirth.HasValue ? frmMain.Instance.CompetitionYear - YearOfBirth : null;
        public bool? IsAdult => Age.HasValue ? (bool?)(Age > 17) : null;

        public enGrade? InitGrade { get; set; }
        public string InitGradeForShow => InitGrade?.ToGradeString();

        public enGrade? ResultGrade { get; set; }
        public string ResultGradeForShow => ResultGrade?.ToGradeString();

        public enGrade? ResultGradeBefore2018 { get; set; }
        public string ResultGradeBefore2018ForShow => ResultGradeBefore2018?.ToGradeString();

        public bool UsedInCalculating { get; set; }

        public enAssignmentStatus AssignmentStatus { get; set; } = enAssignmentStatus.None;
        public string AssignmentStatusForShow => AssignmentStatus.ToAssignmentStatusString();

        public void RefreshAssignmentStatus(Dictionary<enGrade, int> MinGradeAssignmentAges)
        {
            if (ResultGrade > InitGrade)
                AssignmentStatus = enAssignmentStatus.Improve;
            else if (ResultGrade == InitGrade)
                AssignmentStatus = enAssignmentStatus.Сonfirm;

            if (AssignmentStatus != enAssignmentStatus.None)
            {
                if ((IsAdult.Value && ResultGrade < enGrade.Adult3) || (Age < MinGradeAssignmentAges[ResultGrade.Value]))
                {
                    AssignmentStatus = enAssignmentStatus.WillBeNotAssigned;
                }
            }
        }
    }

    public enum enAssignmentStatus
    {
        None,
        /// <summary>
        /// Выполнил новый разряд
        /// </summary>
        Improve,
        /// <summary>
        /// Подтвердил разряд
        /// </summary>
        Сonfirm,
        /// <summary>
        /// Разряд не будет присвоен
        /// </summary>
        WillBeNotAssigned
    }
}
