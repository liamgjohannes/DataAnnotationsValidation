using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataAnnotationsValidation.Attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class RequiresOneOfAttribute : RequiredAttributeBase
	{
		private readonly string[] _propertyNames;

		public ReadOnlyCollection<string> PropertyNames => _propertyNames.ToList().AsReadOnly();

		//Required to enable "AllowMultiple" on attribute usage

		public RequiresOneOfAttribute(params string[] propertyNames)
		{
			_propertyNames = propertyNames;
			ErrorMessage = "At least one of the properties are required.";
		}

		protected override System.ComponentModel.DataAnnotations.ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			//Not validating object instance required
			if (value == null)
				return System.ComponentModel.DataAnnotations.ValidationResult.Success;

			var oneSpecified = false;
			foreach (var propertyName in _propertyNames)
			{
				var propertyValue = GetPropertyValue(value, propertyName);
				oneSpecified |= PropertyIsValid(propertyValue);
			}

			return oneSpecified
				? System.ComponentModel.DataAnnotations.ValidationResult.Success :
				new System.ComponentModel.DataAnnotations.ValidationResult(ErrorMessage, _propertyNames);
		}
	}
}
