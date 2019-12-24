using System;

namespace Ardalis.GuardClauses
{
    public static class NumericExtensions
    {
        public static void LessThan(this IGuardClause guardClause, int lowBound, int? input, string parameterName)
        {
            if (input == null) return;

            if (input < lowBound)
            {
                throw new ArgumentException($"Valid values for {parameterName} are greater than {lowBound}.", parameterName);
            }
        }
    }
}
