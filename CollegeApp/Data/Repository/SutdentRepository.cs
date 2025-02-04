
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
			var existingStudent  = _dbContext.Students.FirstOrDefault(x => x.Id == student.Id);
			if (existingStudent == null)
			{
				throw new ArgumentNullException($"This Item With This Id: {student.Id} not found");
			}
				_dbContext.Students.Remove(existingStudent);
				_dbContext.SaveChanges();
				return true;
			
		}

		List<Student> IStudentRepository.GetAll()
		{
			return  _dbContext.Students.ToList();
		}

		Student IStudentRepository.GetById(int id)
		{
			var student = _dbContext.Students.FirstOrDefault(s => s.Id == id);
			return student;
		}

		Student IStudentRepository.GetByName(string name)
		{
			return _dbContext.Students.FirstOrDefault(s => s.StudentName ==  name);
		}

		int IStudentRepository.Update(Student student)
		{
			var exisistingStudent = _dbContext.Students.FirstOrDefault(i => i.Id == student.Id);
			if(exisistingStudent == null)
			{
				throw new ArgumentNullException($"New Student found id: {student.Id}");
			}
				exisistingStudent.StudentName = student.StudentName;
				exisistingStudent.Email = student.Email;
				exisistingStudent.Address =  student.Address;
				exisistingStudent.DOB = student.DOB;
				_dbContext.SaveChanges();
		
			return student.Id;	
		}
	}
}
