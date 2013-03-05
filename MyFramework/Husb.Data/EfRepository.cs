using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Husb.Common;

//using System.Data.Entity.Design.PluralizationServices;

//using System.Data.Objects;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Globalization;

namespace Husb.Data
{
    public class EfRepository<T> : IRepository<T> where T : class, IEntity //where T : BaseEntity  class
    {
        private readonly DbContext _context;
        private readonly IDbSet<T> _entities;
        //private readonly PluralizationService _pluralizer = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en"));


        public EfRepository(DbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public void Create(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                _entities.Add(entity);
                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                string msg = string.Empty;
                foreach (DbEntityValidationResult dbevr in dbEx.EntityValidationErrors)
                {
                    foreach (DbValidationError dbve in dbevr.ValidationErrors)
                    {
                        msg = msg + string.Format("Property: {0} - Error: {1}", dbve.PropertyName, dbve.ErrorMessage) + Environment.NewLine;
                    }
                }
                Exception fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public void Update(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                string msg = string.Empty;
                foreach (DbEntityValidationResult dbevr in dbEx.EntityValidationErrors)
                {
                    foreach (DbValidationError dbve in dbevr.ValidationErrors)
                    {
                        msg = msg + string.Format("Property: {0} - Error: {1}", dbve.PropertyName, dbve.ErrorMessage) + Environment.NewLine;
                    }
                }
                Exception fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public void Delete(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                _entities.Remove(entity);
                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                string msg = string.Empty;
                foreach (DbEntityValidationResult dbevr in dbEx.EntityValidationErrors)
                {
                    foreach (DbValidationError dbve in dbevr.ValidationErrors)
                    {
                        msg = msg + string.Format("Property: {0} - Error: {1}", dbve.PropertyName, dbve.ErrorMessage) + Environment.NewLine;
                    }
                }
                Exception fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public virtual  void ExecuteSql(string sql)
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<T> Table
        {
            get { return _entities; }
        }

        public virtual IQueryable<T> Query
        {
            get { return _entities; }
        }


        

        //private EntityKey GetEntityKey(object keyValue)
        //{
        //    var entitySetName = GetEntityName();
        //    var objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<T>();
        //    var keyPropertyName = objectSet.EntitySet.ElementType.KeyMembers[0].ToString();
        //    var entityKey = new EntityKey(entitySetName, new[] { new EntityKeyMember(keyPropertyName, keyValue) });
        //    return entityKey;
        //}

        //private string GetEntityName()
        //{
        //    return string.Format("{0}.{1}", ((IObjectContextAdapter)_context).ObjectContext.DefaultContainerName, _pluralizer.Pluralize(typeof(T).Name));
        //}

        public T GetById(object id)
        {
            return _entities.Find(id);

            //EntityKey key = GetEntityKey(id);

            //object originalItem;
            //if (((IObjectContextAdapter)_context).ObjectContext.TryGetObjectByKey(key, out originalItem))
            //{
            //    return (T)originalItem;
            //}
            //return default(T);
        }


        public ICollection<T> GetAll()
        {
            return _entities.ToList();
            //throw new NotImplementedException();
        }

        public IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return _entities.Where(where);
            //throw new NotImplementedException();
        }


        public DbContext Context
        {
            get { return _context; }
        }


       
    }
}
