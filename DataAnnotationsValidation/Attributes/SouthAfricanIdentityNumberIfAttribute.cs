using System;
using System.ComponentModel.DataAnnotations;

namespace DataAnnotationsValidation.Attributes
{
	[AttributeUsage(AttributeTargets.Property)]
	public class SouthAfricanIdentityNumberIfAttribute : SouthAfricanIdentityNumberAttribute
	{
		private readonly string _ifProperty;
		private readonly object _ifValue;
		
		public SouthAfricanIdentityNumberIfAttribute(string ifProperty)
		{
			_ifProperty = ifProperty;
		}

		public SouthAfricanIdentityNumberIfAttribute(string ifProperty, object ifValue)
			: this(ifProperty)
		{
			_ifValue = ifValue;
		}

		protected override System.ComponentModel.DataAnnotations.ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var otherValue = GetPropertyValue(validationContext.ObjectInstance, _ifProperty);
			var otherValueAsStringIsValid = PropertyAsStringIsValid(otherValue);
			if ((otherValue == null
				|| !otherValueAsStringIsValid 
				|| (_ifValue != null && !_ifValue.Equals(otherValue))))
				return System.ComponentModel.DataAnnotations.ValidationResult.Success;

			var dependentOnValueIsCorrectAndValueIsSetAndValid = (_ifValue != null && _ifValue.Equals(otherValue) && IsValid(value));

			return (dependentOnValueIsCorrectAndValueIsSetAndValid)
				? System.ComponentModel.DataAnnotations.ValidationResult.Success
				: new System.ComponentModel.DataAnnotations.ValidationResult(ErrorMessage,
					new[] { validationContext.MemberName, _ifProperty });
		}
	}
}
