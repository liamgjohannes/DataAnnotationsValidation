using System;
using System.ComponentModel.DataAnnotations;

namespace DataAnnotationsValidation.Attributes
{       
    public class GuidAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value == null || Guid.TryParse(value as string, out _);
        }

    }
}