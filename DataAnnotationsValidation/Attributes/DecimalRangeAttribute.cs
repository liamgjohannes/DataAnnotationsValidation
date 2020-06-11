using System.ComponentModel.DataAnnotations;

namespace DataAnnotationsValidation.Attributes
{
    public class DecimalRangeAttribute: ValidationAttribute
    {
        private readonly decimal _min;
        private readonly decimal _max;
        private readonly bool _allowNull;

        public DecimalRangeAttribute(double min, double max, bool allowNull = true)
        {
            const double decimalMinAsDouble = (double) decimal.MinValue;
            const double decimalMaxAsDouble = (double) decimal.MaxValue;

            if (DoubleOutOfRangeOfDecimal(min))
                _min = decimal.MinValue;
            else
                _min = (decimal) min;

            if (DoubleOutOfRangeOfDecimal(max))
                _max = decimal.MaxValue;
            else
                _max = (decimal)max;

            _min = (decimal)min;

            _allowNull = allowNull;

            // Local Functions
            bool DoubleOutOfRangeOfDecimal(double doubleInput)
            {
                return doubleInput <= decimalMinAsDouble ||
                       doubleInput >= decimalMaxAsDouble;
            }
        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return _allowNull;

            var decimalValue = (decimal)value;

            return decimalValue >= _min &&
                   decimalValue <= _max;
        }
	}
}
