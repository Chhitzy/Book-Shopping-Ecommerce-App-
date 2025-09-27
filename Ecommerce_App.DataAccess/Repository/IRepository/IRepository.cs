//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;
////This whole file is for normal database management without stored procedure
//namespace Ecommerce_App.DataAccess.Repository.IRepository
//{
//    public interface IRepository<T> where T :class
//    {
//        void Add(T entity);
//        void Update(T entity);
//        void Remove(T entity);
//        void Remove(int id);    
//        void RemoveRange(IEnumerable <T> entities);
//        T Get(int id);
//        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null);

//       T FirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null);//Used for normal code first database approach, but we will use stored procedure


//    }
//}
