using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Husb.Common;
using System.Data.Entity;

namespace Husb.Data
{
    public class ServiceBase<T> : IRepository<T> where T : class,  IEntity //class 
    {
        private readonly IRepository<T> repository;

        protected ServiceBase(IRepository<T> repository)
        {
            if (repository == null) throw new ArgumentNullException("repository may not be null");
            this.repository = repository;
        }


        public void Create(T entity)
        {
            repository.Create(entity);
        }

        public void Update(T entity)
        {
            repository.Update(entity);
        }

        public void Delete(T entity)
        {
            repository.Delete(entity);
        }

        public void ExecuteSql(string sql)
        {
            repository.ExecuteSql(sql);
        }

        public IQueryable<T> Table
        {
            get { return repository.Table; }
        }

        public IQueryable<T> Query
        {
            get { return repository.Query; }
        }

        public ICollection<T> GetAll()
        {
            return repository.Table.ToList();
        }
        //public IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        //{
            
        //}


        public T GetById(object id)
        {
            //return repository.Table.Where( t => t.ID == id);
            //throw new NotImplementedException();

            //repository.Table.SingleOrDefault(e => e.ID == id);

            //var query = from item in repository.Table
            //            where item.ID.ToString() == id.ToString() // .Equals( id)
            //            select item;
            //return query.FirstOrDefault();
            
            //return query.FirstOrDefault();
            return (T)repository.GetById(id);
        }


        public IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return repository.Table.Where(where);

           
        }


        public DbContext Context
        {
            get { return repository.Context; }
        }
    }
}
