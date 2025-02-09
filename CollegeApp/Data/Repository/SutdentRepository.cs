
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data.Repository
{
	public class SutdentRepository : CollegeRepository<Student>,IStudentRepository
	{
		private readonly CollegeDBContext _dbContext;

        public SutdentRepository(CollegeDBContext dbContext):  base(dbContext) 
		{
            _dbContext = dbContext;
        }

		public List<Student> GetStudentByFeeStatus(int feeStatus)
		{
			return null;
		}

	}
}
