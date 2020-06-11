using System.ComponentModel.DataAnnotations;

namespace DataAnnotationsValidation.Attributes
{
	public abstract class RequiredAttributeBase : ValidationAttribute
	{
		private readonly object _typeId = new object();

		public override object TypeId => _typeId;

		public override bool RequiresValidationContext => true;
		public bool AllowEmptyStrings { get; set; }

		protected static object GetPropertyValue(object value, string propertyName)
		{
			var propertyInfo = value.GetType().GetProperty(propertyName);
			return propertyInfo == null 
				? null 
				: propertyInfo.GetValue(value);
		}

		protected bool PropertyIsValid(object value, object nullValue = null)
		{
			return value != null && !value.Equals(nullValue) && PropertyAsStringIsValid(value);
		}

		protected bool PropertyAsStringIsValid(object value)
		{
			if (value is string str && !AllowEmptyStrings)
				return str.Trim() != "";
			return true;
		}
	}
}