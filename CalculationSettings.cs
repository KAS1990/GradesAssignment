using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GradesAssignment
{
    public enum enResultGradeCalcMethod
    {
        None = -1,
        /// <summary>
        /// Это место и выше (округление "вниз")
        /// </summary>
        Floor = 0,
        /// <summary>
        /// Это место и выше ("математическое" округление до целых)
        /// </summary>
        Round = 1,
    }

    public enum enCompetitionStatus
    {
        None = -1,
        /// <summary>
        /// На соревнованиях не могут быть выполнены МС и КМС
        /// </summary>
        BeforeMasterAndMasterCouldNotBeAssigned = 0,
        /// <summary>
        /// Первенство мира,
        /// Юношеские Олимпийские игры
        /// </summary>
        YourthWorldChampionship_YourthOlimpicGames,
        /// <summary>
        /// Всемирная универсиада
        /// </summary>
        WorldUniversiade,
        /// <summary>
        /// Первенство Европы
        /// </summary>
        YourthEuropeanChampionship,
        /// <summary>
        /// Другие международные спортивные соревнования, включенные в ЕКП
        /// </summary>
        OtherInternationalCompetitions,
        /// <summary>
        /// Первенство мира среди студентов
        /// </summary>
        StudentsWorldChampionship,
        /// <summary>
        /// Чемпионат России
        /// </summary>
        RussianChampionship,
        /// <summary>
        /// Кубок России
        /// </summary>
        RussianCup,
        /// <summary>
        /// Первенство России,
        /// Всероссийская спартакиада между субъектами Российской Федерации
        /// </summary>
        RussianYourthChampionship_AllRussianSportsDay,
        /// <summary>
        /// Другие всероссийские спортивные соревнования, включенные в ЕКП
        /// </summary>
        OtherRussianCompetitions,
        /// <summary>
        /// Чемпионат федерального округа, двух и более федеральных округов,
        /// чемпионаты г. Москвы и г. Санкт-Петербурга
        /// </summary>
        RussainRegionsChampionship_MoscowOrStPetersburgChampionship,
        /// <summary>
        /// Первенство федерального округа, двух и более федеральных округов,
        /// Спартакиада одного или двух и более федеральных округов,
        /// первенства г. Москвы и г. Санкт-Петербурга
        /// </summary>
        RussainFederalRegionsYouthChampionship_RussainFederalRegionsSportsDay_MoscowOrStPetersburgYouthChampionship,
        /// <summary>
        /// Чемпионат субъекта Российской Федерации (кроме г. Москвы и г. Санкт-Петербурга)
        /// </summary>
        RussainRegionChampionshipExceptMoscowAndStPetersburg,
    }

    public class CalculationSettings
    {
        public int MaxYear = -1;
        public enResultGradeCalcMethod CalcMethod = enResultGradeCalcMethod.None;
        public enCompetitionStatus CompetitionStatus = enCompetitionStatus.None;
        public bool ChangeJuniorGradesToAdultForAdults = true;
        public bool UseOnly75Percent = true;
        public int CompetitionYear = DateTime.Today.Year;
    }

    public class Range
    {
        public int Min { get; } = 0;
        public int Max { get; } = 0;
                
        public Range(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public bool IsInRange(int value)
        {
            return Min <= value && value <= Max;
        }
    }

    /// <summary>
    /// Одна строка в листе "МС и КМС"
    /// </summary>
    public class OneRule
    {
        public Range PlacesRange { get; } = null;

        /// <summary>
        /// Список: [разряд, сколько минимально чиловек должно быть].
        /// Правила объединяются через ИЛИ
        /// </summary>
        public List<KeyValuePair<enGrade, int>> GradesCountRequirements { get; set; } = new List<KeyValuePair<enGrade, int>>();

        public OneRule(int minPlace, int maxPlace)
        {
            PlacesRange = new Range(minPlace, maxPlace);
        }
    }

    public class BeforeMasterOrMasterAssignmentRules
    {
        public Range GroupAgesRange { get; } = null;

        public List<OneRule> Rules { get; set; } = new List<OneRule>();

        public BeforeMasterOrMasterAssignmentRules(int minGroupAge, int maxGroupAge)
        {
            GroupAgesRange = new Range(minGroupAge, maxGroupAge);
        }
    }
}
