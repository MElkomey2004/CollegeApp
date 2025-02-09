namespace CollegeApp.Data.Repository
{
	public interface IStudentRepository : ICollegeRepository<Student>
	{
		List<Student> GetStudentByFeeStatus(int feeStatus);	

	}
}
