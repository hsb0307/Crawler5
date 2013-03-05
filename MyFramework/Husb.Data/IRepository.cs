using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using Husb.Common;
using System.Data.Entity;

namespace Husb.Data
{

    public interface IRepository<T> where T : class, IEntity  //where T : class IEntity
    {
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> Table { get; }

        T GetById(object id);
        ICollection<T> GetAll();
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);

        void ExecuteSql(string sql);
        
        IQueryable<T> Query { get; }

        DbContext Context { get; }
    }
}
