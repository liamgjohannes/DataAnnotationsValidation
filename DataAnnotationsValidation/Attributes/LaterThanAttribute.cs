using System;
using System.ComponentModel.DataAnnotations;

namespace DataAnnotationsValidation.Attributes
{
	public class LaterThanAttribute : ValidationAttribute
	{

		public LaterThanType LaterThanType { get; }
		public TimeSpan Offset { get; }

		private readonly DateTimeOffset _minDate;

		public LaterThanAttribute()
		{
			_minDate = DateTimeOffset.Now;
			LaterThanType = LaterThanType.Now;
			Offset = TimeSpan.Zero;
		}

		public LaterThanAttribute(string timeOffset)
		{
			var initSuccess = TimeSpan.TryParse(timeOffset, out var offset);
				
			if(!initSuccess)
				throw new ArgumentException("The specified time offset cannot be parsed. Please specify a valid time offset string for the attribute.");
			_minDate = DateTimeOffset.Now.Add(offset);
			LaterThanType = LaterThanType.FromNow;
			Offset = offset;
		}

		public LaterThanAttribute(bool useMidnight)
		{
			if (useMidnight)
			{
				_minDate = new DateTimeOffset(DateTime.Today);
				LaterThanType = LaterThanType.Today;
			}
			else
			{
				_minDate = DateTimeOffset.Now;
				LaterThanType = LaterThanType.Now;
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

			var dateTime = (value is DateTime time)
				? new DateTimeOffset(time)
				: (DateTimeOffset) value;

			return dateTime > _minDate;
		}
	}

	public enum LaterThanType
	{
		Today,
		FromNow,
		Now
	}

}