using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace DataAnnotationsValidation.Attributes
{
	//Checks if a Collection has the specified minimum number of elements
	public class MinimumCollectionCountAttribute : ValidationAttribute
	{
		private readonly int _minElements;
		public MinimumCollectionCountAttribute(int minElements)
		{
			_minElements = minElements;
		}

		public override bool IsValid(object value)
		{
			if (value is ICollection collection)
			{
				return collection.Count >= _minElements;
			}

			return false;
		}
	}
}
