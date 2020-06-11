using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace DataAnnotationsValidation
{
	[DataContract(Namespace = "http://www.10X.co.za/Services/2015/12")]
	public class ValidationResult
	{
		public ValidationResult()
		{
			
		}
		public ValidationResult(string errorMessage, IEnumerable<string> memberNames = null)
		{
			MemberNames = memberNames;
			ErrorMessage = errorMessage;
		}

		public override string ToString()
		{
			return $"ErrorMessage: {ErrorMessage}, MemberNames: {(MemberNames == null ? "(null)" : "[" + MemberNames.Aggregate((s1, s2) => s1 + ", " + s2) + "]")}";
		}
		
		[DataMember(IsRequired = false)]
		public string ErrorMessage { get; private set; }
		[DataMember(IsRequired = false)]
		public IEnumerable<string> MemberNames { get;  set; }
	}
}