using System;
using System.ComponentModel.DataAnnotations;

namespace DataAnnotationsValidation.Attributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public class RequiredIfAttribute : RequiredAttributeBase
	{
		private readonly string _ifProperty;
		private readonly object _ifValue;

		public string IfProperty => _ifProperty;

		public object IfPropertyNullValue { get; set; }
		public object NullValue { get; set; }

		public RequiredIfAttribute(string ifProperty)
		{
			_ifProperty = ifProperty;
		}

		public RequiredIfAttribute(string ifProperty, object ifValue)
			: this(ifProperty)
		{
			_ifValue = ifValue;
		}

		protected override System.ComponentModel.DataAnnotations.ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var otherValue = GetPropertyValue(validationContext.ObjectInstance, _ifProperty);
			var otherValueAsStringIsValid = PropertyAsStringIsValid(otherValue);
			if ((otherValue == null || otherValue.Equals(IfPropertyNullValue)) 
				|| !otherValueAsStringIsValid 
				|| (_ifValue != null && !_ifValue.Equals(otherValue)))
				return System.ComponentModel.DataAnnotations.ValidationResult.Success;

			var dependentOnValueIsCorrectAndValueIsSet = (_ifValue != null && _ifValue.Equals(otherValue) && PropertyIsValid(value, NullValue));
			var valueIsSet = (_ifValue == null && PropertyIsValid(value, NullValue));

			return dependentOnValueIsCorrectAndValueIsSet || valueIsSet
				? System.ComponentModel.DataAnnotations.ValidationResult.Success
				: new System.ComponentModel.DataAnnotations.ValidationResult(ErrorMessage,
					new[] { validationContext.MemberName, _ifProperty });
		}
	}
}
