//using Ecommerce_App.DataAccess.Repository.IRepository;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;
////This file is for normal database management and is not necessary if you are building stored procedures for the whole project
//namespace Ecommerce_App.DataAccess.Repository
//{
//    public class Repository<T> : IRepository<T> where T : class
//    {
//        public void Add(T entity)
//        {
//            throw new NotImplementedException();
//        }

//        public T FirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null)
//        {
//            throw new NotImplementedException();
//        }

//        public T Get(int id)
//        {
//            throw new NotImplementedException();
//        }

//        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
//        {
//            throw new NotImplementedException();
//        }

//        public void Remove(T entity)
//        {
//            throw new NotImplementedException();
//        }

//        public void Remove(int id)
//        {
//            throw new NotImplementedException();
//        }

//        public void RemoveRange(IEnumerable<T> entities)
//        {
//            throw new NotImplementedException();
//        }

//        public void Update(T entity)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
