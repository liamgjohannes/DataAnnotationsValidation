using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DataAnnotationsValidation.Attributes;
using DataAnnotations = System.ComponentModel.DataAnnotations;

namespace DataAnnotationsValidation
{
	public interface IValidator
	{
		IEnumerable<ValidationResult> Validate(object toValidate);
	}
	/// <summary>
	/// Implementation of DataAnnotations validation for the IValidator interface.
	/// </summary>
	public class DataAnnotationValidator : IValidator
	{
		private readonly IMapper _mapper;
		private readonly Dictionary<Type, object> _services;

		public DataAnnotationValidator(IMapper mapper)
		{
			_mapper = mapper;
		}

		public DataAnnotationValidator(IMapper mapper, IEnumerable<Tuple<Type, object>> services) 
			: this(mapper)
		{
			_services = services.ToDictionary(t => t.Item1, t => t.Item2);
		}

		public virtual IEnumerable<ValidationResult> Validate(object toValidate)
		{
			//main object cannot be null
			if (toValidate == null)
			{
				var validationResult = new ValidationResult("Cannot perform validations on a null.");
				return new List<ValidationResult> { validationResult };
			}

			var resultList = ValidateRecursively(toValidate);

			return resultList;
		}

		/// <summary>
		/// Validates nested objects. In order for an object to be validated it must be marked with the [MustValidate] attribute.
		/// </summary>
		/// <param name="toValidate"></param>
		/// <param name="validationPath"></param>
		/// <returns></returns>
		public virtual IEnumerable<ValidationResult> ValidateRecursively(object toValidate, params string[] validationPath)
		{
			var resultList = new List<ValidationResult>();

			//if object is null, do not validate
			if (toValidate == null)
			{
				return resultList;
			}

			var mainContext = new DataAnnotations.ValidationContext(toValidate);
			mainContext.InitializeServiceProvider(t => _services[t]);
			var mainResults = new List<DataAnnotations.ValidationResult>();
			DataAnnotations.Validator.TryValidateObject(toValidate, mainContext, mainResults, true);

			resultList.AddRange(mainResults.Select(vr =>
			{
				var result = _mapper.Map<ValidationResult>(vr);
				result.MemberNames = result.MemberNames.Select(name => validationPath.Concat(new [] { name }).Aggregate((s1, s2) => s1 + " -> " + s2));
				return result;
			}));
			
			var objectType = toValidate.GetType();
			var mustBeValidatedPropertyInfos = objectType.GetProperties().Where(
																				prop => Attribute.IsDefined(prop, typeof(MustValidateAttribute)) ||
																						Attribute.IsDefined(prop, typeof(MustValidateIfAttribute)));

			foreach (var mustBeValidatedProperty in mustBeValidatedPropertyInfos)
			{
				var childObject = mustBeValidatedProperty.GetValue(toValidate, null);
				if (childObject == null)
					continue;

				if (Attribute.IsDefined(mustBeValidatedProperty, typeof(MustValidateIfAttribute)))
				{
					//check if should validate.
					var customAttribute = Attribute.GetCustomAttribute(mustBeValidatedProperty, typeof(MustValidateIfAttribute)) as MustValidateIfAttribute;
					if(customAttribute == null)
						continue;

					var mustValidate = customAttribute.MustValidate(toValidate);
					if (!mustValidate) 
						continue;
				}

				var childResults = ValidateRecursively(childObject, validationPath.Concat(new [] { mustBeValidatedProperty.Name }).ToArray());
				resultList.AddRange(childResults.Select(_mapper.Map<ValidationResult>));
			}
			
			return resultList;
		}
	}
}