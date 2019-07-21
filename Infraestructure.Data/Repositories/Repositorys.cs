
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Abstracts;
using Domain.Entities;
using Domain.Entities.Cliente;
using Domain.Entities.Localizacíon;
using Infraestructure.Data.Base;


namespace Infraestructure.Data.Repositories
{
    public class Repository<T> : GenericRepository<T>  where T : BaseEntity
    {
        private readonly IDbContext Context;
        public Repository(IDbContext context) : base(context) { Context = context; }

        public T Add(T t, bool saveChange)
        {
            Add(t);
            if (saveChange) { Context.SaveChanges(); }
            return t;
        }

        public T Edit(T t, bool saveChange)
        {
            Edit(t);
            if (saveChange) { Context.SaveChanges(); }
            return t;
        }

        public T Delete(T t, bool saveChange)
        {
            Delete(t);
            if (saveChange) { Context.SaveChanges(); }
            return t;
        }       
    }
}

