﻿using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Base
{

    public interface IEntityService<T> : IService
        where T : BaseEntity
    {
        T Find(object id);
        T Create(T entity);
        bool Delete(T entity);
        IEnumerable<T> GetAll();
        bool Update(T entity);
    }
}
