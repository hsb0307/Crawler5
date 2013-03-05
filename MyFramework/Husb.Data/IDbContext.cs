using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Linq.Expressions;

using Husb.Common;
using System.Data.Entity.Infrastructure;

namespace Husb.Data
{
    public interface IDbContext
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : class, IEntity;//where TEntity :class IEntity<V>;
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class, IEntity;
        int SaveChanges();
    }
}
