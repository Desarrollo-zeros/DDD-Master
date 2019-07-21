using Domain;
using Domain.Abstracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Base
{

    public abstract class EntityService<T> : IEntityService<T> where T : BaseEntity
    {
        readonly IUnitOfWork _unitOfWork;
        [JsonIgnore]
        public readonly IGenericRepository<T> _repository;

        protected EntityService(IUnitOfWork unitOfWork, IGenericRepository<T> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public virtual T Find(object id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            return _repository.Find(id);
        }

        

        public virtual T Create(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            var t = _repository.Add(entity);
            if(_unitOfWork.Commit() == 1)
            {
                return t;
            }
            return null;
        }

        public virtual bool Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _repository.Edit(entity);
            if (_unitOfWork.Commit() == 1)
            {
                return true;
            }
            return false;
        }

        public virtual bool Delete(int id)
        {
            if (id == 0) throw new ArgumentNullException("no id");
            _repository.Delete(Find(id));
            if (_unitOfWork.Commit() == 1)
            {
                return true;
            }
            return false;
        }

        public virtual bool Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _repository.Delete(entity);
            if (_unitOfWork.Commit() == 1)
            {
                return true;
            }
            return false;
        }


        public virtual IEnumerable<T> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
