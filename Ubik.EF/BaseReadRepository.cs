using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using Ubik.Infra.Contracts;
using Ubik.Infra.Ext;
namespace Ubik.EF
{
    public abstract class BaseReadRepository<T, TDbContext> : IReadRepository<T>
        where T : class
        where TDbContext : DbContext
    {
        public abstract TDbContext DbContext { get; }

        public virtual IQueryable<T> GetQuery()
        {
            var entityName = GetEntitySetName(typeof(T));
            return ((IObjectContextAdapter)DbContext).ObjectContext.CreateQuery<T>(entityName);
        }

        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            return DbContext.Set<T>().FirstOrDefault(predicate);
        }

        public virtual T GetOriginalEntity(Expression<Func<T, bool>> predicate)
        {
            var t = Get(predicate);

            object originalItem;
            var entitySet = this.GetEntitySetName(typeof(T));
            var key = ((IObjectContextAdapter)DbContext).ObjectContext.CreateEntityKey(entitySet, t);

            if (((IObjectContextAdapter)DbContext).ObjectContext.TryGetObjectByKey(key, out originalItem))
            {
                return (T)originalItem;
            }
            return null;
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate, Func<T, object> orderby)
        {
            return DbContext.Set<T>().Where(predicate).OrderBy(@orderby).ToList();
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate, string orderByProperty, bool desc)
        {
            return DbContext.Set<T>().Where(predicate).OrderBy(orderByProperty, desc).ToList();
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate, Func<T, object> orderby, bool desc, int pageNumber, int pageSize, out int totalRecords)
        {
            var q = DbContext.Set<T>().Where(predicate);
            totalRecords = q.Count();

            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // in case the pageNumber is greater than totalPages
            // then we should use the last page to get the data
            if (pageNumber > totalPages) { pageNumber = totalPages; }

            return !desc ?
                q.DefaultIfEmpty().OrderBy(@orderby).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList() :
                q.DefaultIfEmpty().OrderByDescending(@orderby).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate, string orderByProperty, bool desc, int pageNumber, int pageSize, out int totalRecords)
        {
            var q = DbContext.Set<T>().Where(predicate);
            totalRecords = q.Count();

            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // in case the pageNumber is greater than totalPages
            // then we should use the last page to get the data
            if (pageNumber > totalPages) { pageNumber = totalPages; }

            if (totalRecords == 0)  //a case with no data
            {
                return q.OrderBy(orderByProperty, desc);
            }

            return q.OrderBy(orderByProperty, desc).Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public virtual bool Contains(Expression<Func<T, bool>> predicate)
        {
            return GetQuery().Any(predicate);
        }

        public virtual string GetEntitySetName(Type type)
        {
            var container = ((IObjectContextAdapter)DbContext)
                .ObjectContext.MetadataWorkspace.GetEntityContainer(((IObjectContextAdapter)DbContext).ObjectContext.DefaultContainerName, DataSpace.CSpace);
            return (from meta in container.BaseEntitySets
                    where meta.ElementType.Name == type.Name
                    select meta.Name).First();
        }
    }
}