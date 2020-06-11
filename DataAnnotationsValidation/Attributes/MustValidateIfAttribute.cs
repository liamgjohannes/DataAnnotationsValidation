using System;

namespace DataAnnotationsValidation.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MustValidateIfAttribute : Attribute
    {
        private readonly string _propertyToCheck;

        public MustValidateIfAttribute(string propertyToCheck)
        {
            _propertyToCheck = propertyToCheck;
        }

        public bool MustValidate(object objectToValidate)
        {
            if (string.IsNullOrEmpty(_propertyToCheck))
                return false;

            if (objectToValidate == null)
                return false;

            var type = objectToValidate.GetType();
            var propertyInfo = type.GetProperty(_propertyToCheck);
            if (propertyInfo == null)
                return false;

            var value = propertyInfo.GetValue(objectToValidate);
            if (value == null)
                return false;

            var valueType = value.GetType();
            if (valueType == typeof(bool))
            {
                return (bool)value;
            }

            return valueType != typeof(bool?) || (bool?)value != false;
        }
    }
}
