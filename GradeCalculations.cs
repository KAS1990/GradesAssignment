using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GradesAssignment
{
    public enum enGrade
    {
        None = -1,
        /// <summary> б/р </summary>
        WithoutGrade = 1,

        /// <summary> 3 ю </summary>
        Young3 = 2,

        /// <summary> 2 ю </summary>
        Young2 = 3,

        /// <summary> 1 ю </summary>
        Young1 = 4,

        /// <summary> 3 </summary>
        Adult3 = 5,

        /// <summary> 2 </summary>
        Adult2 = 6,

        /// <summary> 1 </summary>
        Adult1 = 7,

        /// <summary> КМС </summary>
        BeforeMaster = 8,

        /// <summary> МС </summary>
        Master = 9,

        /// <summary> МСМК </summary>
        InternationalMaster = 10,
    }

    public static class GradeCalculations
    {
        #region Коэффициенты для присвоения массовых разрядов

        private readonly static double[][] GRADES_CALCULATING_COEFFS = new double[][]
        {
            // 1 разряд
            new double[] { 1.0, 0.8, 0.4, 0.2 },
            // 2 разряд
            new double[] { 0.2, 0.4, 0.2 },
            // 3 разряд
            new double[] { 0.2, 0.4, 0.3 },

            // 1 ю разряд
            new double[] { 0.2, 0.4, 0.2 },
            // 2 ю разряд
            new double[] { 0.2, 0.4, 0.2 },
            // 3 ю разряд
            new double[] { 0.2, 0.4, 0.3 },
        };

        private readonly static double[][] GRADES_CALCULATING_COEFFS_BEFORE_2018 = new double[][]
        {
            // 1 разряд
            new double[] { 0.8, 0.8, 0.6, 0.2 },
            // 2 разряд
            new double[] { 0.4, 0.4, 0.2 },
            // 3 разряд
            new double[] { 0.4, 0.4, 0.2 },

            // 1 ю разряд
            new double[] { 0.4, 0.4, 0.2 },
            // 2 ю разряд
            new double[] { 0.4, 0.4, 0.2 },
            // 3 ю разряд
            new double[] { 0.4, 0.4, 0.2 },
        };

        #endregion

        private readonly static Dictionary<enGrade, int> MIN_GRADE_ASSIGNMENT_AGES = new Dictionary<enGrade, int>()
        {
            { enGrade.Young3, 10 },
            { enGrade.Young2, 10 },
            { enGrade.Young1, 10 },
            { enGrade.Adult3, 10 },
            { enGrade.Adult2, 12 },
            { enGrade.Adult1, 12 },
            { enGrade.BeforeMaster, 14 },
            { enGrade.Master, 15 },
        };

        #region Правила присвоения КМС

        private readonly static Dictionary<enCompetitionStatus, List<BeforeMasterOrMasterAssignmentRules>> BEFORE_MASTER_ASSIGNMENT_RULES = new Dictionary<enCompetitionStatus, List<BeforeMasterOrMasterAssignmentRules>>()
        {
            #region YourthWorldChampionship_YourthOlimpicGames
            {
                enCompetitionStatus.YourthWorldChampionship_YourthOlimpicGames,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Юноши, девушки (14-15 лет)
                    new BeforeMasterOrMasterAssignmentRules(14, 15)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 6)
                        }
                    },
                }
            },
            #endregion

            #region WorldUniversiade
            {
                enCompetitionStatus.WorldUniversiade,
                new List<BeforeMasterOrMasterAssignmentRules>()
            },
            #endregion

            #region YourthEuropeanChampionship
            {
                enCompetitionStatus.YourthEuropeanChampionship,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Юноши, девушки (14-15 лет)
                    new BeforeMasterOrMasterAssignmentRules(14, 15)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 5)
                        }
                    },
                }
            },
            #endregion

            #region OtherInternationalCompetitions
            {
                enCompetitionStatus.OtherInternationalCompetitions,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Юниоры, юниорки (18-19 лет)
                    new BeforeMasterOrMasterAssignmentRules(18, 19)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(4, 8)
                        }
                    },
                    // Юноши, девушки (16-17 лет)
                    new BeforeMasterOrMasterAssignmentRules(16, 17)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(2, 6)
                        }
                    },
                    // Юноши, девушки (14-15 лет)
                    new BeforeMasterOrMasterAssignmentRules(14, 15)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 4)
                        }
                    },
                }
            },
            #endregion

            #region StudentsWorldChampionship
            {
                enCompetitionStatus.StudentsWorldChampionship,
                new List<BeforeMasterOrMasterAssignmentRules>()
            },
            #endregion

            #region RussianChampionship
            {
                enCompetitionStatus.RussianChampionship,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Мужчины, женщины
                    new BeforeMasterOrMasterAssignmentRules(0, 99)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(9, 20)
                        }
                    },
                }
            },
            #endregion

            #region RussianCup
            {
                enCompetitionStatus.RussianCup,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Мужчины, женщины
                    new BeforeMasterOrMasterAssignmentRules(0, 99)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(5, 12)
                        }
                    },
                }
            },
            #endregion

            #region RussianYourthChampionship_AllRussianSportsDay
            {
                enCompetitionStatus.RussianYourthChampionship_AllRussianSportsDay,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Юниоры, юниорки (18-19 лет)
                    new BeforeMasterOrMasterAssignmentRules(18, 19)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(3, 8)
                        }
                    },
                    // Юноши, девушки (16-17 лет)
                    new BeforeMasterOrMasterAssignmentRules(16, 17)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 5)
                        }
                    },
                    // Юноши, девушки (14-15 лет)
                    new BeforeMasterOrMasterAssignmentRules(14, 15)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 2)
                        }
                    },
                }
            },
            #endregion

            #region OtherRussianCompetitions
            {
                enCompetitionStatus.OtherRussianCompetitions,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Мужчины, женщины
                    new BeforeMasterOrMasterAssignmentRules(20, 99)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(4, 10)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 20)
                                }
                            },
                            new OneRule(2, 6)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 15)
                                }
                            },
                            new OneRule(1, 4)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 10),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 20)
                                }
                            },
                            new OneRule(1, 2)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 5),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 10)
                                }
                            },
                        }
                    },
                    // Юниоры, юниорки (18-19 лет)
                    new BeforeMasterOrMasterAssignmentRules(18, 19)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(2, 6)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 10)
                                }
                            },
                            new OneRule(1, 4)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 8),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 20)
                                }
                            },
                            new OneRule(1, 2)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 5),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 10)
                                }
                            },
                        }
                    },
                    // Юноши, девушки (16-17 лет)
                    new BeforeMasterOrMasterAssignmentRules(16, 17)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 2)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 5),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 10)
                                }
                            },
                            new OneRule(1, 1)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 8)
                                }
                            },
                        }
                    },
                    // Юноши, девушки (14-15 лет)
                    new BeforeMasterOrMasterAssignmentRules(14, 15)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 1)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 10)
                                }
                            },
                        }
                    },
                }
            },
            #endregion

            #region RussainRegionsChampionship_MoscowOrStPetersburgChampionship
            {
                enCompetitionStatus.RussainRegionsChampionship_MoscowOrStPetersburgChampionship,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Мужчины, женщины
                    new BeforeMasterOrMasterAssignmentRules(0, 99)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 6)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 15),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 30)
                                }
                            },
                            new OneRule(1, 4)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 10),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 20)
                                }
                            },
                            new OneRule(1, 3)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 8),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 15)
                                }
                            },
                            new OneRule(1, 2)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 5),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 15)
                                }
                            },
                            new OneRule(1, 1)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 10)
                                }
                            },
                        }
                    },
                }
            },
            #endregion

            #region RussainFederalRegionsYouthChampionship_RussainFederalRegionsSportsDay_MoscowOrStPetersburgYouthChampionship
            {
                enCompetitionStatus.RussainFederalRegionsYouthChampionship_RussainFederalRegionsSportsDay_MoscowOrStPetersburgYouthChampionship,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Юниоры, юниорки (18-19 лет)
                    new BeforeMasterOrMasterAssignmentRules(18, 19)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 4)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 15),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 30)
                                }
                            },
                            new OneRule(1, 2)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 10),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 20)
                                }
                            },
                            new OneRule(1, 1)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 5),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 10)
                                }
                            },
                        }
                    },
                    // Юноши, девушки (16-17 лет)
                    new BeforeMasterOrMasterAssignmentRules(16, 17)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 4)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 15),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 30)
                                }
                            },
                            new OneRule(1, 2)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 10),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 20)
                                }
                            },
                            new OneRule(1, 1)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 5),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 10)
                                }
                            },
                        }
                    },
                    // Юноши, девушки (14-15 лет)
                    new BeforeMasterOrMasterAssignmentRules(14, 15)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 1)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 5),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 10)
                                }
                            },
                        }
                    },
                }
            },
            #endregion

            #region RussainRegionChampionshipExceptMoscowAndStPetersburg
            {
                enCompetitionStatus.RussainRegionChampionshipExceptMoscowAndStPetersburg,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Мужчины, женщины
                    new BeforeMasterOrMasterAssignmentRules(0, 99)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 2)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 10),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 20)
                                }
                            },
                            new OneRule(1, 1)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 5),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 10)
                                }
                            },
                        }
                    },
                }
            }
            #endregion
        };

        private readonly static Dictionary<enCompetitionStatus, List<BeforeMasterOrMasterAssignmentRules>> BEFORE_MASTER_ASSIGNMENT_RULES_BEFORE_2018 = new Dictionary<enCompetitionStatus, List<BeforeMasterOrMasterAssignmentRules>>()
        {
            #region YourthWorldChampionship_YourthOlimpicGames
            {
                enCompetitionStatus.YourthWorldChampionship_YourthOlimpicGames,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Юноши, девушки (14-15 лет)
                    new BeforeMasterOrMasterAssignmentRules(14, 15)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 6)
                        }
                    },
                }
            },
            #endregion

            #region WorldUniversiade
            {
                enCompetitionStatus.WorldUniversiade,
                new List<BeforeMasterOrMasterAssignmentRules>()
            },
            #endregion

            #region YourthEuropeanChampionship
            {
                enCompetitionStatus.YourthEuropeanChampionship,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Юноши, девушки (14-15 лет)
                    new BeforeMasterOrMasterAssignmentRules(14, 15)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 5)
                        }
                    },
                }
            },
            #endregion

            #region OtherInternationalCompetitions
            {
                enCompetitionStatus.OtherInternationalCompetitions,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Юниоры, юниорки (18-19 лет)
                    new BeforeMasterOrMasterAssignmentRules(18, 19)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(4, 8)
                        }
                    },
                    // Юноши, девушки (16-17 лет)
                    new BeforeMasterOrMasterAssignmentRules(16, 17)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(2, 6)
                        }
                    },
                    // Юноши, девушки (14-15 лет)
                    new BeforeMasterOrMasterAssignmentRules(14, 15)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 4)
                        }
                    },
                }
            },
            #endregion

            #region StudentsWorldChampionship
            {
                enCompetitionStatus.StudentsWorldChampionship,
                new List<BeforeMasterOrMasterAssignmentRules>()
            },
            #endregion

            #region RussianChampionship
            {
                enCompetitionStatus.RussianChampionship,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Мужчины, женщины
                    new BeforeMasterOrMasterAssignmentRules(0, 99)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(9, 20)
                        }
                    },
                }
            },
            #endregion

            #region RussianCup
            {
                enCompetitionStatus.RussianCup,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Мужчины, женщины
                    new BeforeMasterOrMasterAssignmentRules(0, 99)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(5, 12)
                        }
                    },
                }
            },
            #endregion

            #region RussianYourthChampionship_AllRussianSportsDay
            {
                enCompetitionStatus.RussianYourthChampionship_AllRussianSportsDay,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Юниоры, юниорки (18-19 лет)
                    new BeforeMasterOrMasterAssignmentRules(18, 19)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(3, 8)
                        }
                    },
                    // Юноши, девушки (16-17 лет)
                    new BeforeMasterOrMasterAssignmentRules(16, 17)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 5)
                        }
                    },
                    // Юноши, девушки (14-15 лет)
                    new BeforeMasterOrMasterAssignmentRules(14, 15)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 2)
                        }
                    },
                }
            },
            #endregion

            #region OtherRussianCompetitions
            {
                enCompetitionStatus.OtherRussianCompetitions,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Мужчины, женщины
                    new BeforeMasterOrMasterAssignmentRules(20, 99)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(4, 10)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 20)
                                }
                            },
                            new OneRule(2, 6)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 15)
                                }
                            },
                            new OneRule(1, 4)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 10),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 20)
                                }
                            },
                            new OneRule(1, 2)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 5),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 10)
                                }
                            },
                        }
                    },
                    // Юниоры, юниорки (18-19 лет)
                    new BeforeMasterOrMasterAssignmentRules(18, 19)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(2, 6)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 10)
                                }
                            },
                            new OneRule(1, 4)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 8),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 20)
                                }
                            },
                            new OneRule(1, 2)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 5),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 10)
                                }
                            },
                        }
                    },
                    // Юноши, девушки (16-17 лет)
                    new BeforeMasterOrMasterAssignmentRules(16, 17)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 2)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 5),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 10)
                                }
                            },
                            new OneRule(1, 1)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 8)
                                }
                            },
                        }
                    },
                    // Юноши, девушки (14-15 лет)
                    new BeforeMasterOrMasterAssignmentRules(14, 15)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 1)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 10)
                                }
                            },
                        }
                    },
                }
            },
            #endregion

            #region RussainRegionsChampionship_MoscowOrStPetersburgChampionship
            {
                enCompetitionStatus.RussainRegionsChampionship_MoscowOrStPetersburgChampionship,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Мужчины, женщины
                    new BeforeMasterOrMasterAssignmentRules(0, 99)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 6)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 15),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 30)
                                }
                            },
                            new OneRule(1, 4)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 10),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 20)
                                }
                            },
                            new OneRule(1, 3)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 8),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 15)
                                }
                            },
                            new OneRule(1, 2)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 5),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 15)
                                }
                            },
                            new OneRule(1, 1)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 10)
                                }
                            },
                        }
                    },
                }
            },
            #endregion

            #region RussainFederalRegionsYouthChampionship_RussainFederalRegionsSportsDay_MoscowOrStPetersburgYouthChampionship
            {
                enCompetitionStatus.RussainFederalRegionsYouthChampionship_RussainFederalRegionsSportsDay_MoscowOrStPetersburgYouthChampionship,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Юниоры, юниорки (18-19 лет)
                    new BeforeMasterOrMasterAssignmentRules(18, 19)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 4)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 15),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 30)
                                }
                            },
                            new OneRule(1, 2)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 10),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 20)
                                }
                            },
                            new OneRule(1, 1)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 5),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 10)
                                }
                            },
                        }
                    },
                    // Юноши, девушки (16-17 лет)
                    new BeforeMasterOrMasterAssignmentRules(16, 17)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 4)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 15),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 30)
                                }
                            },
                            new OneRule(1, 2)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 10),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 20)
                                }
                            },
                            new OneRule(1, 1)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 5),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 10)
                                }
                            },
                        }
                    },
                    // Юноши, девушки (14-15 лет)
                    new BeforeMasterOrMasterAssignmentRules(14, 15)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 1)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 5),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 10)
                                }
                            },
                        }
                    },
                }
            },
            #endregion

            #region RussainRegionChampionshipExceptMoscowAndStPetersburg
            {
                enCompetitionStatus.RussainRegionChampionshipExceptMoscowAndStPetersburg,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Мужчины, женщины
                    new BeforeMasterOrMasterAssignmentRules(0, 99)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 2)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 10),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 20)
                                }
                            },
                            new OneRule(1, 1)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.BeforeMaster, 5),
                                    new KeyValuePair<enGrade, int>(enGrade.Adult1, 10)
                                }
                            },
                        }
                    },
                }
            }
            #endregion
        };

        #endregion

        #region Правила присвоения МС

        private readonly static Dictionary<enCompetitionStatus, List<BeforeMasterOrMasterAssignmentRules>> MASTER_ASSIGNMENT_RULES = new Dictionary<enCompetitionStatus, List<BeforeMasterOrMasterAssignmentRules>>()
        {
            #region YourthWorldChampionship_YourthOlimpicGames
            {
                enCompetitionStatus.YourthWorldChampionship_YourthOlimpicGames,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Юниоры, юниорки (18-19 лет)
                    new BeforeMasterOrMasterAssignmentRules(18, 19)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 5)
                        }
                    },
                    // Юноши, девушки (16-17 лет)
                    new BeforeMasterOrMasterAssignmentRules(16, 17)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 3)
                        }
                    },
                }
            },
            #endregion

            #region WorldUniversiade
            {
                enCompetitionStatus.WorldUniversiade,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Юниоры, юниорки (17-25 лет)
                    new BeforeMasterOrMasterAssignmentRules(17, 25)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 5)
                        }
                    },
                }
            },
            #endregion

            #region YourthEuropeanChampionship
            {
                enCompetitionStatus.YourthEuropeanChampionship,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Юниоры, юниорки (18-19 лет)
                    new BeforeMasterOrMasterAssignmentRules(18, 19)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 4)
                        }
                    },
                    // Юноши, девушки (16-17 лет)
                    new BeforeMasterOrMasterAssignmentRules(16, 17)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 2)
                        }
                    },
                }
            },
            #endregion

            #region OtherInternationalCompetitions
            {
                enCompetitionStatus.OtherInternationalCompetitions,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Юниоры, юниорки (18-19 лет)
                    new BeforeMasterOrMasterAssignmentRules(18, 19)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 3)
                        }
                    },
                    // Юноши, девушки (16-17 лет)
                    new BeforeMasterOrMasterAssignmentRules(16, 17)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 1)
                        }
                    },
                }
            },
            #endregion

            #region StudentsWorldChampionship
            {
                enCompetitionStatus.StudentsWorldChampionship,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Юниоры, юниорки (17-25 лет)
                    new BeforeMasterOrMasterAssignmentRules(17, 25)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 3)
                        }
                    },
                }
            },
            #endregion

            #region RussianChampionship
            {
                enCompetitionStatus.RussianChampionship,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Мужчины, женщины
                    new BeforeMasterOrMasterAssignmentRules(0, 99)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 8)
                        }
                    },
                }
            },
            #endregion

            #region RussianCup
            {
                enCompetitionStatus.RussianCup,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Мужчины, женщины
                    new BeforeMasterOrMasterAssignmentRules(0, 99)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 4)
                        }
                    },
                }
            },
            #endregion

            #region RussianYourthChampionship_AllRussianSportsDay
            {
                enCompetitionStatus.RussianYourthChampionship_AllRussianSportsDay,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Юниоры, юниорки (18-19 лет)
                    new BeforeMasterOrMasterAssignmentRules(18, 19)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 2)
                        }
                    },
                }
            },
            #endregion

            #region OtherRussianCompetitions
            {
                enCompetitionStatus.OtherRussianCompetitions,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Мужчины, женщины
                    new BeforeMasterOrMasterAssignmentRules(20, 99)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 3)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.Master, 5)
                                }
                            },
                            new OneRule(1, 1)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.Master, 3)
                                }
                            },
                        }
                    },
                    // Юниоры, юниорки (18-19 лет)
                    new BeforeMasterOrMasterAssignmentRules(18, 19)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 1)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.Master, 3)
                                }
                            },
                        }
                    },
                }
            },
            #endregion

            #region RussainRegionsChampionship_MoscowOrStPetersburgChampionship
            {
                enCompetitionStatus.RussainRegionsChampionship_MoscowOrStPetersburgChampionship,
                new List<BeforeMasterOrMasterAssignmentRules>()
            },
            #endregion

            #region RussainFederalRegionsYouthChampionship_RussainFederalRegionsSportsDay_MoscowOrStPetersburgYouthChampionship
            {
                enCompetitionStatus.RussainFederalRegionsYouthChampionship_RussainFederalRegionsSportsDay_MoscowOrStPetersburgYouthChampionship,
                new List<BeforeMasterOrMasterAssignmentRules>()
            },
            #endregion

            #region RussainRegionChampionshipExceptMoscowAndStPetersburg
            {
                enCompetitionStatus.RussainRegionChampionshipExceptMoscowAndStPetersburg,
                new List<BeforeMasterOrMasterAssignmentRules>()
            }
            #endregion
        };

        private readonly static Dictionary<enCompetitionStatus, List<BeforeMasterOrMasterAssignmentRules>> MASTER_ASSIGNMENT_RULES_BEFORE_2018 = new Dictionary<enCompetitionStatus, List<BeforeMasterOrMasterAssignmentRules>>()
        {
            #region YourthWorldChampionship_YourthOlimpicGames
            {
                enCompetitionStatus.YourthWorldChampionship_YourthOlimpicGames,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Юниоры, юниорки (18-19 лет)
                    new BeforeMasterOrMasterAssignmentRules(18, 19)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 5)
                        }
                    },
                    // Юноши, девушки (16-17 лет)
                    new BeforeMasterOrMasterAssignmentRules(16, 17)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 3)
                        }
                    },
                }
            },
            #endregion

            #region WorldUniversiade
            {
                enCompetitionStatus.WorldUniversiade,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Юниоры, юниорки (17-25 лет)
                    new BeforeMasterOrMasterAssignmentRules(17, 25)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 5)
                        }
                    },
                }
            },
            #endregion

            #region YourthEuropeanChampionship
            {
                enCompetitionStatus.YourthEuropeanChampionship,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Юниоры, юниорки (18-19 лет)
                    new BeforeMasterOrMasterAssignmentRules(18, 19)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 4)
                        }
                    },
                    // Юноши, девушки (16-17 лет)
                    new BeforeMasterOrMasterAssignmentRules(16, 17)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 2)
                        }
                    },
                }
            },
            #endregion

            #region OtherInternationalCompetitions
            {
                enCompetitionStatus.OtherInternationalCompetitions,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Юниоры, юниорки (18-19 лет)
                    new BeforeMasterOrMasterAssignmentRules(18, 19)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 3)
                        }
                    },
                    // Юноши, девушки (16-17 лет)
                    new BeforeMasterOrMasterAssignmentRules(16, 17)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 1)
                        }
                    },
                }
            },
            #endregion

            #region StudentsWorldChampionship
            {
                enCompetitionStatus.StudentsWorldChampionship,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Юниоры, юниорки (17-25 лет)
                    new BeforeMasterOrMasterAssignmentRules(17, 25)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 3)
                        }
                    },
                }
            },
            #endregion

            #region RussianChampionship
            {
                enCompetitionStatus.RussianChampionship,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Мужчины, женщины
                    new BeforeMasterOrMasterAssignmentRules(0, 99)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 8)
                        }
                    },
                }
            },
            #endregion

            #region RussianCup
            {
                enCompetitionStatus.RussianCup,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Мужчины, женщины
                    new BeforeMasterOrMasterAssignmentRules(0, 99)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 4)
                        }
                    },
                }
            },
            #endregion

            #region RussianYourthChampionship_AllRussianSportsDay
            {
                enCompetitionStatus.RussianYourthChampionship_AllRussianSportsDay,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Юниоры, юниорки (18-19 лет)
                    new BeforeMasterOrMasterAssignmentRules(18, 19)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 2)
                        }
                    },
                }
            },
            #endregion

            #region OtherRussianCompetitions
            {
                enCompetitionStatus.OtherRussianCompetitions,
                new List<BeforeMasterOrMasterAssignmentRules>()
                {
                    // Мужчины, женщины
                    new BeforeMasterOrMasterAssignmentRules(20, 99)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 3)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.Master, 5)
                                }
                            },
                            new OneRule(1, 1)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.Master, 3)
                                }
                            },
                        }
                    },
                    // Юниоры, юниорки (18-19 лет)
                    new BeforeMasterOrMasterAssignmentRules(18, 19)
                    {
                        Rules = new List<OneRule>()
                        {
                            new OneRule(1, 1)
                            {
                                GradesCountRequirements = new List<KeyValuePair<enGrade, int>>()
                                {
                                    new KeyValuePair<enGrade, int>(enGrade.Master, 3)
                                }
                            },
                        }
                    },
                }
            },
            #endregion

            #region RussainRegionsChampionship_MoscowOrStPetersburgChampionship
            {
                enCompetitionStatus.RussainRegionsChampionship_MoscowOrStPetersburgChampionship,
                new List<BeforeMasterOrMasterAssignmentRules>()
            },
            #endregion

            #region RussainFederalRegionsYouthChampionship_RussainFederalRegionsSportsDay_MoscowOrStPetersburgYouthChampionship
            {
                enCompetitionStatus.RussainFederalRegionsYouthChampionship_RussainFederalRegionsSportsDay_MoscowOrStPetersburgYouthChampionship,
                new List<BeforeMasterOrMasterAssignmentRules>()
            },
            #endregion

            #region RussainRegionChampionshipExceptMoscowAndStPetersburg
            {
                enCompetitionStatus.RussainRegionChampionshipExceptMoscowAndStPetersburg,
                new List<BeforeMasterOrMasterAssignmentRules>()
            }
            #endregion
        };
        #endregion

        private class GradeStat
        {
            public enGrade? Grade = null;
            public int MembersWithGrade = 0;
        }

        public static string ToGradeString(this enGrade grade)
        {
            switch (grade)
            {
                case enGrade.WithoutGrade:
                    return "б/р";

                case enGrade.Young3:
                    return "3 ю";

                case enGrade.Young2:
                    return "2 ю";

                case enGrade.Young1:
                    return "1 ю";

                case enGrade.Adult3:
                    return "3";

                case enGrade.Adult2:
                    return "2";

                case enGrade.Adult1:
                    return "1";

                case enGrade.BeforeMaster:
                    return "КМС";

                case enGrade.Master:
                    return "МС";

                case enGrade.InternationalMaster:
                    return "МСМК";

                default:
                    return "";
            }
        }
        
        public static enGrade ParseGrade(this string grade)
        {
            enGrade result = enGrade.None;
            string source = grade.ToLower().Trim();

            // Исправляем ошибки в названии разрядов
            if (source.Contains('б') && source.Contains('р'))
            {
                result = enGrade.WithoutGrade;
            }
            else if (source.Contains('3'))
            {
                if (source.Contains('ю'))
                {
                    result = enGrade.Young3;
                }
                else
                {
                    result = enGrade.Adult3;
                }
            }
            else if (source.Contains('2'))
            {
                if (source.Contains('ю'))
                {
                    result = enGrade.Young2;
                }
                else
                {
                    result = enGrade.Adult2;
                }
            }
            else if (source.Contains('1'))
            {
                if (source.Contains('ю'))
                {
                    result = enGrade.Young1;
                }
                else
                {
                    result = enGrade.Adult1;
                }
            }
            else if (source.Contains("мк"))
            {
                result = enGrade.InternationalMaster;
            }
            else if (source.Contains("к"))
            {
                result = enGrade.BeforeMaster;
            }
            else if (source.Contains("м"))
            {
                result = enGrade.Master;
            }
            
            return result;
        }

        public static string ToAssignmentStatusString(this enAssignmentStatus status)
        {
            switch (status)
            {
                case enAssignmentStatus.Improve:
                    return "выполнил";

                case enAssignmentStatus.Сonfirm:
                    return "подтвердил";

                case enAssignmentStatus.WillBeNotAssigned:
                    return "не будет присвоен";

                default:
                    return "";
            }
        }

        private static int CalcMinPlaceForNewGrade(enResultGradeCalcMethod ResultGradeCalcMethod, double raw)
        {
            switch (ResultGradeCalcMethod)
            {
                case enResultGradeCalcMethod.Floor:
                    return (int)Math.Floor(raw);

                case enResultGradeCalcMethod.Round:
                    return (int)Math.Round(raw, 0);
            }

            return 0;
        }

        private static List<KeyValuePair<enGrade, int>> CalculateMassGradesInternal(Dictionary<enGrade?, int> gradesStat,
            CalculationSettings settings,
            double[][] coeffs)
        {
            List<KeyValuePair<enGrade, int>> minPlaceForNewGrade = new List<KeyValuePair<enGrade, int>>();

            double placeRaw = 0;

            // 1 разряд
            placeRaw =  coeffs[0][0] * (gradesStat[enGrade.Master] + gradesStat[enGrade.InternationalMaster]) +
                        coeffs[0][1] * gradesStat[enGrade.BeforeMaster] +
                        coeffs[0][2] * gradesStat[enGrade.Adult1] +
                        coeffs[0][3] * gradesStat[enGrade.Adult2];
            minPlaceForNewGrade.Add(new KeyValuePair<enGrade, int>(enGrade.Adult1, CalcMinPlaceForNewGrade(settings.CalcMethod, placeRaw)));

            // 2 разряд
            placeRaw += coeffs[1][0] * gradesStat[enGrade.Adult1] +
                        coeffs[1][1] * gradesStat[enGrade.Adult2] +
                        coeffs[1][2] * gradesStat[enGrade.Adult3];
            minPlaceForNewGrade.Add(new KeyValuePair<enGrade, int>(enGrade.Adult2, CalcMinPlaceForNewGrade(settings.CalcMethod, placeRaw)));

            // 3 разряд
            placeRaw += coeffs[2][0] * gradesStat[enGrade.Adult2] +
                        coeffs[2][1] * gradesStat[enGrade.Adult3] +
                        coeffs[2][2] * gradesStat[enGrade.Young1];
            minPlaceForNewGrade.Add(new KeyValuePair<enGrade, int>(enGrade.Adult3, CalcMinPlaceForNewGrade(settings.CalcMethod, placeRaw)));

            // 1 ю разряд
            placeRaw += coeffs[3][0] * gradesStat[enGrade.Adult3] +
                        coeffs[3][1] * gradesStat[enGrade.Young1] +
                        coeffs[3][2] * gradesStat[enGrade.Young2];
            minPlaceForNewGrade.Add(new KeyValuePair<enGrade, int>(enGrade.Young1, CalcMinPlaceForNewGrade(settings.CalcMethod, placeRaw)));

            // 2 ю разряд
            placeRaw += coeffs[4][0] * gradesStat[enGrade.Young1] +
                        coeffs[4][1] * gradesStat[enGrade.Young2] +
                        coeffs[4][2] * gradesStat[enGrade.Young3];
            minPlaceForNewGrade.Add(new KeyValuePair<enGrade, int>(enGrade.Young2, CalcMinPlaceForNewGrade(settings.CalcMethod, placeRaw)));

            // 3 ю разряд
            placeRaw += coeffs[5][0] * gradesStat[enGrade.Young2] +
                        coeffs[5][1] * gradesStat[enGrade.Young3] +
                        coeffs[5][2] * gradesStat[enGrade.WithoutGrade];
            minPlaceForNewGrade.Add(new KeyValuePair<enGrade, int>(enGrade.Young3, CalcMinPlaceForNewGrade(settings.CalcMethod, placeRaw)));

            return minPlaceForNewGrade;
        }

        /// <summary>
        /// Присвоение МС и КМС
        /// </summary>
        /// <param name="orderedMembers"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        private static void BeforeMasterAndMasterAssignment(IEnumerable<MemberInfo> orderedMembers,
            List<BeforeMasterOrMasterAssignmentRules> rules, bool isMaster, bool isBefore2018)
        {
            foreach (var member in orderedMembers)
            {
                bool gradeShouldBeAssigned =
                    rules.Any(rule =>
                        rule.GroupAgesRange.IsInRange(member.Age.Value)
                        && rule.Rules.Exists(oneRule =>
                            oneRule.PlacesRange.IsInRange(member.Place.Value)
                            && (oneRule.GradesCountRequirements.Count == 0
                                || oneRule.GradesCountRequirements.Any(requirement =>
                                    orderedMembers.Count(arg => arg.InitGrade >= requirement.Key) >= requirement.Value))));

                if (gradeShouldBeAssigned)
                {
                    if (isBefore2018)
                        member.ResultGradeBefore2018 = isMaster ? enGrade.Master : enGrade.BeforeMaster;
                    else
                    {
                        member.ResultGrade = isMaster ? enGrade.Master : enGrade.BeforeMaster;
                        member.RefreshAssignmentStatus(MIN_GRADE_ASSIGNMENT_AGES);
                    }
                }
            }
        }

        /// <summary>
        /// Присвоение МС и КМС
        /// </summary>
        /// <param name="members"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        private static void BeforeMasterAndMasterAssignment(IEnumerable<MemberInfo> orderedMembers, CalculationSettings settings)
        {
            if (settings.CompetitionStatus == enCompetitionStatus.None
                || settings.CompetitionStatus == enCompetitionStatus.BeforeMasterAndMasterCouldNotBeAssigned)
            {
                return;
            }

            BeforeMasterAndMasterAssignment(orderedMembers, BEFORE_MASTER_ASSIGNMENT_RULES[settings.CompetitionStatus], false, false);
            BeforeMasterAndMasterAssignment(orderedMembers, BEFORE_MASTER_ASSIGNMENT_RULES_BEFORE_2018[settings.CompetitionStatus], false, true);
            BeforeMasterAndMasterAssignment(orderedMembers, MASTER_ASSIGNMENT_RULES[settings.CompetitionStatus], true, false);
            BeforeMasterAndMasterAssignment(orderedMembers, MASTER_ASSIGNMENT_RULES_BEFORE_2018[settings.CompetitionStatus], true, true);
        }

        public static bool CalculateGrades(List<MemberInfo> members, CalculationSettings settings, out string message)
        {
            message = "";
                        
            IEnumerable<MemberInfo> membersForGradesCalc = members
                .Where(arg => arg.Place.HasValue && arg.InitGrade.HasValue && arg.InitGrade != enGrade.None)
                .OrderBy(arg => arg.Place.Value);

            if (membersForGradesCalc.Count() != members.Count)
            {   // Разряды и места есть не у всех участников
                message = "Не удалось присвоить разряды, т.к. места и начальные разряды заданы не для всех участников";
                return false;
            }

            membersForGradesCalc = members.Where(arg => arg.YearOfBirth.HasValue && arg.YearOfBirth <= settings.MaxYear);

            var membersFilteredByYearOfBirth = membersForGradesCalc;

            members.ForEach(arg =>
            {
                if (settings.ChangeJuniorGradesToAdultForAdults && arg.IsAdult.Value && arg.InitGrade < enGrade.Young1)
                {
                    arg.InitGrade = enGrade.Young1;
                }
                arg.ResultGrade = arg.ResultGradeBefore2018 = null;
                arg.UsedInCalculating = false;
                arg.AssignmentStatus = enAssignmentStatus.None;
            });

            if (settings.UseOnly75Percent)
            {   // Учитываем только 75% участников
                int membersForCalcing = (int)(Math.Floor(membersForGradesCalc.Count() * 0.75));
                List<MemberInfo> lst = membersForGradesCalc.ToList();
                if (membersForCalcing < lst.Count && membersForCalcing > 0 && lst[membersForCalcing].Place == lst[membersForCalcing - 1].Place)
                {   // Паровоз на границе 75% => включаем его в расчёт
                    int curPlace = lst[membersForCalcing - 1].Place.Value;
                    while (membersForCalcing < lst.Count && lst[membersForCalcing].Place.Value == curPlace)
                    {
                        membersForCalcing++;
                    }
                }
                membersForGradesCalc = membersForGradesCalc.Take(membersForCalcing);
            }

            foreach (var member in membersForGradesCalc)
                member.UsedInCalculating = true;

            Dictionary<enGrade?, int> gradesStat = (from member in membersForGradesCalc
                                                    group member by member.InitGrade into MembersGrades
                                                    select new GradeStat
                                                    {
                                                        Grade = MembersGrades.Key,
                                                        MembersWithGrade = MembersGrades.Count(arg => arg.InitGrade == MembersGrades.Key)
                                                    }).ToDictionary(key => key.Grade, item => item.MembersWithGrade);


            int tmp;
            for (enGrade grade = enGrade.WithoutGrade; grade <= enGrade.InternationalMaster; grade++)
            {
                if (!gradesStat.TryGetValue(grade, out tmp))
                    gradesStat[grade] = 0;
            }

            // Определяем места
            List<KeyValuePair<enGrade, int>> minPlaceForNewGrade = CalculateMassGradesInternal(gradesStat, settings, GRADES_CALCULATING_COEFFS);
            List<KeyValuePair<enGrade, int>> minPlaceForNewGradeBefore2018 = CalculateMassGradesInternal(gradesStat, settings, GRADES_CALCULATING_COEFFS_BEFORE_2018);

            // Присваиваем массовые разряды
            int placeInYear = 0;
            int prevPlace = 0;
            int trainLength = 1;
            foreach (var member in membersForGradesCalc)
            {
                if (member.Place.Value != prevPlace)
                {
                    placeInYear += trainLength;
                    prevPlace = member.Place.Value;
                    trainLength = 1;
                }
                else
                    trainLength++;

                for (int i = 0; i < minPlaceForNewGrade.Count; i++)
                {
                    if (placeInYear <= minPlaceForNewGrade[i].Value)
                    {
                        member.ResultGrade = minPlaceForNewGrade[i].Key;
                        member.RefreshAssignmentStatus(MIN_GRADE_ASSIGNMENT_AGES);
                        break;
                    }
                }

                for (int i = 0; i < minPlaceForNewGradeBefore2018.Count; i++)
                {
                    if (placeInYear <= minPlaceForNewGradeBefore2018[i].Value)
                    {
                        member.ResultGradeBefore2018 = minPlaceForNewGradeBefore2018[i].Key;
                        break;
                    }
                }
            }

            // Присваиваем МС и КМС. При этом учитываем всех участников соревнований, старше минимального возраста
            BeforeMasterAndMasterAssignment(membersFilteredByYearOfBirth, settings);

            return true;
        }
    }
}
