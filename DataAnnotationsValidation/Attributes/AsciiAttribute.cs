using System.ComponentModel.DataAnnotations;

namespace DataAnnotationsValidation.Attributes
{
	public class AsciiAttribute : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			if (value == null)
			{
				return true;
			}
			var encodedOutput = System.Text.Encoding.ASCII.GetString(System.Text.Encoding.ASCII.GetBytes(value.ToString()));
			return encodedOutput == value.ToString();
		}
	}
}
