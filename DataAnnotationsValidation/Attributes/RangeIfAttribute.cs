using System;
using System.ComponentModel.DataAnnotations;

namespace DataAnnotationsValidation.Attributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public class RangeIfAttribute : RequiredAttributeBase
	{
	    private readonly Type _type;
	    private readonly object _minimum;
	    private readonly object _maximum;
	    private readonly bool _allowZero;
	    private readonly object _ifValue;

        public string IfProperty { get; }

        public object IfPropertyNullValue { get; set; }
		public object NullValue { get; set; }
        
        public RangeIfAttribute(string ifProperty, object ifValue, int minimum, int maximum, bool allowZero = false)
			: this(ifProperty, minimum, maximum, allowZero)
		{
			_ifValue = ifValue;
		}

		public RangeIfAttribute(string ifProperty, object ifValue, double minimum, double maximum, bool allowZero = false)
			: this(ifProperty, minimum, maximum, allowZero)
		{
			_ifValue = ifValue;
		}
		public RangeIfAttribute(string ifProperty, object ifValue, Type type, string minimum, string maximum, bool allowZero = false)
			: this(ifProperty, type, minimum, maximum, allowZero)
		{
			_ifValue = ifValue;
		}

	    public RangeIfAttribute(string ifProperty, int minimum, int maximum, bool allowZero = false)
	    {
	        _minimum = minimum;
	        _maximum = maximum;
	        _allowZero = allowZero;
	        _type = typeof(int);
	        IfProperty = ifProperty;
	    }

	    public RangeIfAttribute(string ifProperty, double minimum, double maximum, bool allowZero = false)
	    {
	        _minimum = minimum;
	        _maximum = maximum;
	        _allowZero = allowZero;
	        _type = typeof(double);
	        IfProperty = ifProperty;
	    }

	    public RangeIfAttribute(string ifProperty, Type type, string minimum, string maximum, bool allowZero = false)
	    {
	        _minimum = minimum;
	        _maximum = maximum;
	        _allowZero = allowZero;
	        _type = type;
	        IfProperty = ifProperty;
	    }
        

        protected override System.ComponentModel.DataAnnotations.ValidationResult IsValid(object value, ValidationContext validationContext)
		{
		    //if other value not set, or is set but not equal to the _ifValue then don't validate, ie. always return success.
            var otherValue = GetPropertyValue(validationContext.ObjectInstance, IfProperty);
			var otherValueAsStringIsValid = PropertyAsStringIsValid(otherValue);
			if ((otherValue == null || otherValue.Equals(IfPropertyNullValue)) 
				|| !otherValueAsStringIsValid 
				|| (_ifValue != null && !_ifValue.Equals(otherValue)))
				return System.ComponentModel.DataAnnotations.ValidationResult.Success;

			var dependentOnValueIsCorrectAndValueIsSet = (_ifValue != null && _ifValue.Equals(otherValue) && PropertyIsValid(value, NullValue));
			var valueIsSet = (_ifValue == null && PropertyIsValid(value, NullValue));

            //if other value is set,  and equal to the _ifValue (if specified), then we need to validate.  Else return success (don't do validation).
			if(!dependentOnValueIsCorrectAndValueIsSet && !valueIsSet)
				return System.ComponentModel.DataAnnotations.ValidationResult.Success;

            //if zero's allowed,  check if zero and return success if so.
            if(_allowZero && IsZero(value))
                return System.ComponentModel.DataAnnotations.ValidationResult.Success;

            //create the underlying RangeAttribute instance and validate against the passed in params.
            var attr = new RangeAttribute(_type, _minimum.ToString(), _maximum.ToString());
		    return attr.GetValidationResult(value, validationContext);
		}

	    private bool IsZero(object value)
	    {
	        switch (value)
			{
				case int i when i == 0:
				case double d when Math.Abs(d) == 0:
					return true;
			}

			if(double.TryParse(value?.ToString(), out double result))
                return Math.Abs(result) == 0;

	        return false;
	    }
	}
}
