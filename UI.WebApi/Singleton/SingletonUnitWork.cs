using Domain;
using Domain.Abstracts;
using Infraestructure.Data.Base;
using System;

namespace UI.WebApi.Singleton
{
    public static class SingletonUnitWork<T> where T : BaseEntity
    {
        private static readonly Lazy<IUnitOfWork> unitOfWork =
            new Lazy<IUnitOfWork>(() => new UnitOfWork(SingletonDBContext<T>.Instance_db));
        public static IUnitOfWork UnitOfWork => unitOfWork.Value;
    }

}