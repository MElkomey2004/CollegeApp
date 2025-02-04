using CollegeApp.Validator;
using System.ComponentModel.DataAnnotations;

namespace CollegeApp.Models
{
	public class StudentDTO 
	{

		public int Id { get; set; }

		[Required]
		public string StudentName { get; set; }
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		public string Address { get; set; }

		//[DataCheck]
		//public DateTime AdmissionDate { get; set; }

		public DateTime DOB { get; set; }


	}
}