using System;
using System.ComponentModel.DataAnnotations;

namespace DataAnnotationsValidation.Attributes
{
	public class DateTimeOffsetAttribute : ValidationAttribute
    {
        /// <summary>
        /// Specifies if DateTimeOffset.MinValue is a valid assignment to this property.  Default value is false.
        /// </summary>
	    public bool AllowMin { get; set; } = false;

        /// <summary>
        /// Specifies if DateTimeOffset.MaxValue is a valid assignment to this property.  Default value is false.
        /// </summary>
        public bool AllowMax { get; set; } = false;

        public override bool IsValid(object value)
		{
		    DateTimeOffset compare;
		    if (value is DateTimeOffset offset)
		    {
		        compare = offset;
		    }
		    else
		    {
		        if (!(value is string))
		            return false;

		        if (!DateTimeOffset.TryParse((string) value, out compare))
		            return false;
		    }


		    if (!AllowMin && compare.Equals(DateTimeOffset.MinValue))
		        return false;

            //the string "12/31/9999 11:59:59 PM +00:00" is parsed as a DateTimeOffset with ticks 3155......000.
            //the DateTimeOffset.MaxValue, even though it parses to the same string has ticks 3155......999.  So the values won't match.
            //"zeroing" out the values to midnight on the day (".Date"), will have this comparison pass.
		    return AllowMax || !compare.Date.Equals(DateTimeOffset.MaxValue.Date);
		}
	}
}
