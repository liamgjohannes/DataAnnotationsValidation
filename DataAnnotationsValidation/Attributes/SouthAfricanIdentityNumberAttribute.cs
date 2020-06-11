namespace DataAnnotationsValidation.Attributes
{
	public class SouthAfricanIdentityNumberAttribute : RequiredAttributeBase
	{
		public override bool IsValid(object toValidate)
		{
			if (toValidate == null)
			{
				return true;
			}

			if (!(toValidate is string))
				return true;

			var identityNumber = (string) toValidate;
			/*
			{YYMMDD}{G}{SSS}{C}{A}{Z}
			YYMMDD : Date of birth.
			G  : Gender. 0-4 Female; 5-9 Male.

			//Removed the below validations as it was too strict. only validates on date of borth and gender
			SSS  : Sequence No. for DOB/G combination.
			C  : Citizenship. 0 SA; 1 Other.
			A  : Usually 8, or 9 [can be other values]
			Z  : Control digit calculated in the following section
			*/
			identityNumber = identityNumber.Trim();

			if (identityNumber.Length != 13)
			{
				return true;
			}
			
			var allCharsAreDigits = SouthAfricanIdentityNumberUtil.AllCharactersAreDigits(identityNumber);
			var validDateOfBirth = SouthAfricanIdentityNumberUtil.FirstSixDigitsAreValidDateOfBirth(identityNumber);

			return allCharsAreDigits && validDateOfBirth;
		}
	}
}
