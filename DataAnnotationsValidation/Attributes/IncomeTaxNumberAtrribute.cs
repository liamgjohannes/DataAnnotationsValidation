using System;
using System.Linq;

namespace DataAnnotationsValidation.Attributes
{
	public class IncomeTaxNumberAttribute : RequiredAttributeBase
	{
		private bool _isValid;

		public override bool IsValid(object toValidate)
		{
			_isValid = true;
			if (toValidate == null)
			{
				return true;
			}

			if (!(toValidate is string))
			{
				return true;
			}

			var incomeTaxNumber = (string)toValidate;

			incomeTaxNumber = incomeTaxNumber.Trim();
			if (incomeTaxNumber.Length < 10)
			{
				incomeTaxNumber = incomeTaxNumber.PadLeft(10, '0');
			}

			if (incomeTaxNumber.Length > 10)
			{
				return true;
			}

			ValidateAllCharactersAreDigits(incomeTaxNumber);

			var taxNumberStartDigitToValidate = new[] {"0", "1", "2", "3", "9"};
			if (taxNumberStartDigitToValidate.Contains(incomeTaxNumber.Substring(0, 1)))
			{
				ValidateCheckDigit(incomeTaxNumber);
			}
			return _isValid;
		}

		private void ValidateAllCharactersAreDigits(string incomeTaxNumber)
		{
			if (_isValid && incomeTaxNumber.Any(c => !IsDigit(c)))
			{
				_isValid = false;
			}
		}

		private static bool IsDigit(char @char)
		{
			try
			{
				var value = Convert.ToByte(@char.ToString());
				return true;
			}
			catch (Exception)
			{
				//TODO: Handle exception
			}
			return false;
		}
		
		private void ValidateCheckDigit(string incomeTaxNumber)
		{
			if (_isValid && !CheckDigitIsValid(incomeTaxNumber))
			{
				_isValid = false;
			}
		}

		private static bool CheckDigitIsValid(string incomeTaxNumber)
		{
			var checkDigit = GetCheckDigit(incomeTaxNumber);
			return checkDigit == incomeTaxNumber.Substring(9, 1);
		}

		private static string GetCheckDigit(string incomeTaxNumber)
		{
			try
			{
				var total = 0;
				var counter = 0;
				for (var i = incomeTaxNumber.Length - 2; i >= 0; i--)
				{
					var digit = Convert.ToInt32(incomeTaxNumber.Substring(i, 1));
					if (counter % 2 == 0)
						digit *= 2;
					if (digit > 9)
						digit -= 9;
					total += digit;
					counter++;
				}

				return (total.ToString().Substring(total.ToString().Length - 1, 1)) == "0" ? "0" : (10 - (total%10)).ToString();
			}
			catch (Exception ex)
			{
				//TODO: Handle exception
			}
			return "_";
		}
	}
}
