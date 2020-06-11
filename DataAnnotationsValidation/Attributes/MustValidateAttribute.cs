using System;

namespace DataAnnotationsValidation.Attributes
{
    /// <summary>
    /// The default behavior of the validator is to validate only the properties of built-in type. 
    /// When this attribute is specified on a property, it indicates to the DataAnnotationValidator that the property itself also needs to be run through the validator.
    /// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class MustValidateAttribute : Attribute
	{
	}
}
