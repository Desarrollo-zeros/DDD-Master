using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infraestructure.Data;
using System.Data.Entity;
using Domain.Entities;

namespace Infraestructure.Test
{
    public class DBContextTest : DBContext
    {
        public DBContextTest() : base()
        {
        }

        protected DBContextTest(DbConnection connection) : base(connection)
        {
            Log = Console.WriteLine;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Suppress code first model migration check          
            Database.SetInitializer<DBContextTest>(new AlwaysCreateInitializer());


            base.OnModelCreating(modelBuilder);
        }

        public void Seed(DBContext context)
        {
            context.SaveChanges();
        }

    }

    public class CreateInitializer : CreateDatabaseIfNotExists<DBContextTest>
    {
        protected override void Seed(DBContextTest context)
        {
            context.Seed(context);
            base.Seed(context);
        }
    }


    public class DropCreateIfChangeInitializer : DropCreateDatabaseIfModelChanges<DBContextTest>
    {
        protected override void Seed(DBContextTest context)
        {
            context.Seed(context);
            base.Seed(context);
        }
    }

    public class AlwaysCreateInitializer : DropCreateDatabaseAlways<DBContextTest>
    {
        protected override void Seed(DBContextTest context)
        {
            context.Seed(context);
            base.Seed(context);
        }
    }

}
