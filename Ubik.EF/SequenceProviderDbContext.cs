using System;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ubik.EF.Contracts;

namespace Ubik.EF
{
    public abstract class SequenceProviderDbContext : DbContext, ISequenceProvider
    {
        protected SequenceProviderDbContext()
            : base("cmsconnectionstring")
        {
        }

        protected SequenceProviderDbContext(string connString)
            : base(connString)
        {
        }

        //TODO: http://www.proficiencyconsulting.com/ShowArticle.aspx?ID=23
        public void Next(DbEntityEntry entry)
        {
            var seqName = entry.Entity.GetType().Name;
            var sqlText = string.Format("SELECT NEXT VALUE FOR {0};", seqName);
            var id = default(int);
            try
            {
                id = Database.SqlQuery<int>(sqlText).First();
            }
            catch (Exception ex)
            {
                if (ex.Source != ".Net SqlClient Data Provider" /* TODO: && ex.*/) throw;
                var sqlToCreate = string.Format("CREATE SEQUENCE {0} AS INTEGER MINVALUE 1 NO CYCLE; ", seqName);
                id = Database.SqlQuery<int>(sqlToCreate + sqlText).First();

            }
            var objectContext = ((IObjectContextAdapter)this).ObjectContext;
            var wKey =
                objectContext.MetadataWorkspace.GetEntityContainer(objectContext.DefaultContainerName, DataSpace.CSpace)
                    .BaseEntitySets.First(meta => meta.ElementType.Name == seqName)
                    .ElementType.KeyMembers.Select(k => k.Name).FirstOrDefault();
            entry.Property(wKey).CurrentValue = id;
        }

        public override int SaveChanges()
        {
            NextIds();
            return base.SaveChanges();
        }

        protected virtual void NextIds()
        {
            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added))
            {
                var entity = entry.Entity as ISequenceBase;
                if (entity != null)
                {
                    Next(entry);
                }
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            NextIds();
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}