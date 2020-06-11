using System;
using System.Linq;

namespace DataAnnotationsValidation
{
	public static class SouthAfricanIdentityNumberUtil
	{
		public static bool AllCharactersAreDigits(string identityNumber)
		{
			return identityNumber.All(IsDigit);
		}

		public static bool FirstSixDigitsAreValidDateOfBirth(string identityNumber)
		{
			return DateOfBirthIsValid(identityNumber);
		}

#region Helpers
		private static bool IsDigit(char @char)
		{
			return byte.TryParse(@char.ToString(), out _);
		}

		private static bool DateOfBirthIsValid(string identityNumber)
		{
			var year = identityNumber.Substring(0, 2);
			var month = identityNumber.Substring(2, 2);
			var day = identityNumber.Substring(4, 2);

			return TryParseAsTwentyFirstCentury(year, month, day, out _)
				   || TryParseAsTwentiethCentury(year, month, day, out _);
		}

		private static bool TryParseAsTwentyFirstCentury(string year, string month, string day, out DateTime date)
		{
			return DateTime.TryParse($"20{year}/{month}/{day}", out date) && date < DateTime.Now;
		}

		private static bool TryParseAsTwentiethCentury(string year, string month, string day, out DateTime date)
		{
			return DateTime.TryParse($"19{year}/{month}/{day}", out date);
		}

#endregion
	}
}