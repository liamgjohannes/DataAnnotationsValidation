using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace DataAnnotationsValidation.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SumMustEqualAttribute : ValidationAttribute
    {
        private readonly string[] _propertyNames;
        private readonly double _expectedSum;
        
        public SumMustEqualAttribute(double expectedSum, params string[] propertyNames)
        {
            _expectedSum = expectedSum;
            _propertyNames = propertyNames;
        }
        
        // Given a list of classes move down through _properyNames until the last property specified
        // sum of last property values must equal _expectedSum
        public override bool IsValid(object listToValidate)
        {
            var list = listToValidate as IEnumerable;

            if (list == null)
                return true; // this is not the Required attribute

            double total = 0;
            foreach (var item in list)
            {
                var classToCheck = item;
                for (var i = 0; i < _propertyNames.Length; i ++)
                {
                    var propertyName = _propertyNames[i];
                    
                    classToCheck = classToCheck?.GetType().GetProperty(propertyName)?.GetValue(classToCheck, null);
                    
                    if (i != _propertyNames.Length-1) 
                        continue;
                    
                    // last property name
                    var rawValue = classToCheck?.ToString();    
                        
                    if (!double.TryParse(rawValue, out var value))
                        continue;
                        
                    total += value;
                }
            }

            return total.Equals(_expectedSum);
        }
    }
}