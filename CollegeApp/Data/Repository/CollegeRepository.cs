
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CollegeApp.Data.Repository
{
	public class CollegeRepository<T> : ICollegeRepository<T> where T : class
	{
		private readonly CollegeDBContext _dbContext;
		private DbSet<T> _dbSet;

		public CollegeRepository(CollegeDBContext dbContext)
		{
			_dbContext = dbContext;
			_dbSet = _dbContext.Set<T>();
		}
		T ICollegeRepository<T>.Create(T dbRecord)
		{
			_dbSet.Add(dbRecord);
			_dbContext.SaveChanges();
			return dbRecord;
		}

		bool  ICollegeRepository<T>.Delete(T dbRecord)
		{
			_dbSet.Remove(dbRecord);
			_dbContext.SaveChanges();
			return true;
		}

		List<T> ICollegeRepository<T>.GetAll()
		{
			return  _dbSet.ToList();

		}

		T ICollegeRepository<T>.Get(Expression<Func<T , bool>>filter, bool useNoTracking)
		{
			if (useNoTracking)

				return _dbSet.AsNoTracking().FirstOrDefault(filter);

			else
				return _dbSet.FirstOrDefault(filter);
		}

	

		T  ICollegeRepository<T>.Update(T dbRecord)
		{

			_dbSet.Update(dbRecord);
			_dbContext.SaveChanges();

			return dbRecord;
		}
	}
}
