using System;
using System.ComponentModel.DataAnnotations;

namespace DataAnnotationsValidation.Attributes
{
	public class EarlierThanAttribute : ValidationAttribute
	{
		private readonly DateTimeOffset _maxDate;

		public EarlierThanType EarlierThanType { get; }
		public TimeSpan Offset { get; }

		public EarlierThanAttribute()
		{
			_maxDate = DateTimeOffset.Now;
			EarlierThanType = EarlierThanType.Now;
			Offset = TimeSpan.Zero;
		}

		public EarlierThanAttribute(string timeOffset)
		{
			var initSuccess = TimeSpan.TryParse(timeOffset, out var offset);

			if (!initSuccess)
				throw new ArgumentException("The specified time offset cannot be parsed. Please specify a valid time offset string for the attribute.");
			_maxDate = DateTimeOffset.Now.Add(offset);
			Offset = offset;
		}

		public EarlierThanAttribute(bool useMidnight)
		{
			if (useMidnight)
			{
				_maxDate = new DateTimeOffset(DateTime.Today);
				EarlierThanType = EarlierThanType.Today;
			}
			else
			{
				_maxDate = DateTimeOffset.Now;
				EarlierThanType = EarlierThanType.Now;
			}
			Offset = TimeSpan.Zero;
		}

		public override bool IsValid(object value)
		{
			//Not a required validation
			if (value == null)
				return true;

			//Not a type validation
			if (!(value is DateTimeOffset) && !(value is DateTime))
				return true;

			var dateTime = (value is DateTime)
							? new DateTimeOffset((DateTime)value)
							: (DateTimeOffset)value;

			return dateTime < _maxDate;
		}
	}

	public enum EarlierThanType
	{
		Today,
		BeforeNow,
		Now
	}
}
