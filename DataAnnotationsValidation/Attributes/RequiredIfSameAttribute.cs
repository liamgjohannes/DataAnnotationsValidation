using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataAnnotationsValidation.Attributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public class RequiredIfSameAttribute : RequiredAttributeBase
	{
	    private readonly string[] _ifProperties;
	    public ReadOnlyCollection<string> IfProperties => _ifProperties.ToList().AsReadOnly();

		public RequiredIfSameAttribute(params string [] ifProperties)
		{
		    _ifProperties = ifProperties;
		    ErrorMessage = $"Field is required if properties {ifProperties.Aggregate((a, b) => a + "," + b)} are not alike.";
        }
        
	    protected override System.ComponentModel.DataAnnotations.ValidationResult IsValid(object value, ValidationContext validationContext)
	    {
	        var values = _ifProperties.Select(x => GetPropertyValue(validationContext.ObjectInstance, x)).ToList();

	        var allSame = values.Distinct().Count() == 1;

            //all the fields don't have the same value, so not required.  Return success.
            if(!allSame)
                return System.ComponentModel.DataAnnotations.ValidationResult.Success;

	        var isValid = PropertyIsValid(value);

            return isValid 
                    ? System.ComponentModel.DataAnnotations.ValidationResult.Success
                    : new System.ComponentModel.DataAnnotations.ValidationResult(ErrorMessage, new []{validationContext.MemberName});
	    }
	}
}
