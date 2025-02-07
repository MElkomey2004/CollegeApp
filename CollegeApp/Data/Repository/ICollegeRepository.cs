using Microsoft.Identity.Client;
using System.Linq.Expressions;

namespace CollegeApp.Data.Repository
{
	public interface ICollegeRepository<T>
	{
		public List<T> GetAll();
		public T GetById(Expression<Func<T, bool>> filter, bool useNoTracking = false);
		public T GetByName(Expression<Func<T, bool>> filter);
		public T Create(T dbRecord);
		public T Update(T dbRecord);
		public bool Delete(T dbRecord);

	}
}
