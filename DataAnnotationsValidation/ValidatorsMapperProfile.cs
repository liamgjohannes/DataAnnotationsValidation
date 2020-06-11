using AutoMapper;
using DataAnnotations = System.ComponentModel.DataAnnotations;

namespace DataAnnotationsValidation
{
	public class ValidatorsMapperProfile : Profile
	{
		protected void Configure()
		{
			CreateMap<DataAnnotations.ValidationResult, ValidationResult>();
		}
	}
}