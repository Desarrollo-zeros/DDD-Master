using Domain;
using Domain.Abstracts;
using Infraestructure.Data.Repositories;
using System;

namespace UI.WebApi.Singleton
{
    public static class SingletonRepository<T> where T : BaseEntity
    {
        private static readonly Lazy<IGenericRepository<T>> repository =
            new Lazy<IGenericRepository<T>>(() => new Repository<T>(SingletonDBContext<T>.Instance_db));
        public static IGenericRepository<T> GenericRepository => repository.Value;
    }

}