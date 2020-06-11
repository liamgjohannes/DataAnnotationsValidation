using System;
using System.ComponentModel.DataAnnotations;

namespace DataAnnotationsValidation.Attributes
{
	public class DateAttribute : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			return !(value is string) || DateTime.TryParse((string) value, out _);
		}
	}
}
