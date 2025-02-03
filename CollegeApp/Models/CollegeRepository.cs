using CollegeApp.Model;

namespace CollegeApp.Models
{
	public static class CollegeRepository
	{
		public static List<Student> Students { get; set; }	= new List<Student>
		{
			new Student { Id = 1, StudentName = "John Doe", Email = "elkomey2004@gmail.com", Address ="cario" },
			new Student { Id = 2, StudentName = "Jane Smith", Email = "elkomey2004@gmail.com", Address ="cario" },
		};


	}
}
