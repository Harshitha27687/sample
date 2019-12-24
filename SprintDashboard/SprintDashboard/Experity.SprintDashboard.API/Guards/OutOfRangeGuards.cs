using System;
using System.Collections.Generic;
using System.Linq;

namespace Ardalis.GuardClauses
{
    public static class OutOfRangeGuards
    {
        public static void OutOfRange<T>(this IGuardClause guardClause, int? input, string parameterName) where T : Enum
        {
            IEnumerable<T> validValues = Enum.GetValues(typeof(T))
                                             .Cast<T>();

            OutOfRange<T>(guardClause, input, validValues, parameterName);
        }

        public static void OutOfRange<T>(this IGuardClause guardClause, int? input, IEnumerable<T> validValues, string parameterName) where T : Enum
        {
            if (input == null) return;

            if (Enum.IsDefined(typeof(T), input))
            {
                T enumToCheck = (T)(object)input.Value;
                if (validValues.Contains(enumToCheck)) return;
            }

            IEnumerable<string> validEnumsWithValue = validValues.OrderBy(x => Convert.ToInt32(x)).Select(x => $"{x} - {(Convert.ToInt32(x)).ToString()}");

            string concatenatedValidValues = string.Join(", ", validEnumsWithValue).TrimEnd(',', ' ');

            throw new ArgumentOutOfRangeException(parameterName, input, $"Valid values are {concatenatedValidValues}.");
        }

    }
}
