
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data.Repository
{
	public class SutdentRepository : IStudentRepository
	{
		private readonly CollegeDBContext _dbContext;

        public SutdentRepository(CollegeDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        int IStudentRepository.Create(Student student)
		{
			_dbContext.Students.Add(student);
			_dbContext.SaveChanges();
			return student.Id;
		}

		bool IStudentRepository.Delete(Student student)
		{
			
				_dbContext.Students.Remove(student);
				_dbContext.SaveChanges();
				return true;
			
		}

		List<Student> IStudentRepository.GetAll()
		{
			return  _dbContext.Students.ToList();
		}

		Student IStudentRepository.GetById(int id ,  bool useNoTracking = false)
		{
			if (useNoTracking)

				return _dbContext.Students.AsNoTracking().FirstOrDefault(s => s.Id == id);
			else
				return _dbContext.Students.FirstOrDefault(s => s.Id == id);
			
		}

		Student IStudentRepository.GetByName(string name)
		{
			return _dbContext.Students.FirstOrDefault(s => s.StudentName ==  name);
		}

		int IStudentRepository.Update(Student student)
		{

			_dbContext.Update(student);
				_dbContext.SaveChanges();
		
			return student.Id;	
		}
	}
}
