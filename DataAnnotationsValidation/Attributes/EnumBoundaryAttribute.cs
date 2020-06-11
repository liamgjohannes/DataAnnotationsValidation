using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataAnnotationsValidation.Attributes
{
    /// <summary>
    /// Is used too ensure values passed to the property are valid values for the enum defined on that property
    /// </summary>
    public class EnumBoundaryAttribute : ValidationAttribute
    {
        /// <summary>
        /// Specifies if null values are allowed to be sent to the property and have it still be valid.  Default value is false.
        /// </summary>
        public bool AllowNulls { get; set; } = false;
        
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return AllowNulls;
            }

            var enumType = value.GetType();
            if (!enumType.IsEnum)
                return false;

            var values = Enum.GetValues(enumType).Cast<object>();
            return values.Contains(value);
        }
    }
}
