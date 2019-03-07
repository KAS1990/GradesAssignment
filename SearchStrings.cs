using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GradesAssignment
{
    public static class SearchStrings
    {
        public const string PLACE_COL_NAME = "место";

        public const string SURNAME = "фамилия";
        public const string NAME = "имя";
        public const string SURNAME_NAME_SHORT = "фи";
        public const string SURNAME_NAME_SHORT_1 = "фио";

        public const string YEAR_OF_BIRTH = "г.р";
        public const string YEAR_OF_BIRTH_1 = "г. р";
        public const string YEAR_OF_BIRTH_2 = "гр";

        public const string GRADE_COL_NAME = "разряд";
        public const string GRADE_SHORT = "разр";

        public const string FIRST_PLACE = "I";
        public const string SECOND_PLACE = "II";
        public const string THIRD_PLACE = "III";
        public const string BEYOND_QUALIF = "В/К";

        public static enSearchResult Check(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return enSearchResult.NotFound;

            string source = value.Trim().ToLower();

            if (source == PLACE_COL_NAME)
            {
                return enSearchResult.Place;
            }

            if ((source.Contains(SURNAME) && source.Contains(NAME))
                || source == SURNAME_NAME_SHORT || source == SURNAME_NAME_SHORT_1)
            {
                return enSearchResult.SurnameAndName;
            }

            if (source.StartsWith(YEAR_OF_BIRTH) || source.StartsWith(YEAR_OF_BIRTH_1) || source.StartsWith(YEAR_OF_BIRTH_2))
            {
                return enSearchResult.YearOfBirth;
            }

            if (source == GRADE_COL_NAME || source.StartsWith(GRADE_SHORT))
            {
                return enSearchResult.Grade;
            }

            return enSearchResult.NotFound;
        }

        public static int? ParsePlace(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            string source = value.Trim().ToUpper();

            switch (source)
            {
                case FIRST_PLACE:
                    return 1;

                case SECOND_PLACE:
                    return 2;

                case THIRD_PLACE:
                    return 3;

                case BEYOND_QUALIF:
                    return -1;

                default:
                    {
                        int result;
                        if (int.TryParse(source, out result))
                            return result;
                        break;
                    }
            }

            return null;
        }
    }


    public enum enSearchResult
    {
        NotFound,
        Place,
        SurnameAndName,
        YearOfBirth,
        Grade
    }
}
