using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


namespace Infraestructure.Data.Base
{
    public interface IDbContext
    {
        DbSet<T> Set<T>() where T : class;
        Action<string> Log { get; set; }
        DbEntityEntry Entry(object entity);
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        void SetModified(object entity);
        int SaveChanges();
    }

    [DbConfigurationType(typeof(MySql.Data.EntityFramework.MySqlEFConfiguration))]
    public class DbContextBase : DbContext, IDbContext
    {
        public DbContextBase(string nameOrConnectionString)
            : base(nameOrConnectionString){}
        public DbContextBase(DbConnection connection): base(connection, true)
        { Configuration.LazyLoadingEnabled = false; }

        public Action<string> Log
        {
            get
            {
                return this.Database.Log;
            }
            set
            {
                this.Database.Log = value;
            }
        }

        public void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }
    }
}
