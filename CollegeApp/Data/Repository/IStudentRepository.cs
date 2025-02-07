namespace CollegeApp.Data.Repository
{
	public interface IStudentRepository
	{
		public List<Student> GetAll();	
		public Student GetById(int id , bool useNoTracking = false);
		public Student GetByName(string name);
		public int Create(Student student);
		public int Update(Student student);	
		public bool  Delete(Student student);
	}
}
