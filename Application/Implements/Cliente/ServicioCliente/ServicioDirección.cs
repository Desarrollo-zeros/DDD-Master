using Application.Base;
using Domain.Abstracts;
using Domain.Entities.Cliente;
using Domain.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implements.Cliente.ServicioCliente
{
    public class ServicioDirección : EntityService<Dirección>
    {
        readonly IUnitOfWork _unitOfWork;


        public ServicioDirección(IUnitOfWork unitOfWork, IGenericRepository<Dirección> repository) : base(unitOfWork, repository)
        {
            _unitOfWork = unitOfWork;
           
        }

        public ServiceResponse Create(ServicioDireccíonRequest request)
        {
            var dirección = _repository.FindBy(x => x.Cliente_Id == request.Cliente_Id && x.Barrio == request.Barrio  && x.Direccion == request.Direccion ).FirstOrDefault();
            if (dirección != null)
            {
                return new ServiceResponse() { Mensaje = "Dirección ya registrada", Status = false };
            }

            if (request.Direcciónes != null)
            {
                dirección = _repository.AddRange(request.Direcciónes.ToList()).FirstOrDefault();
            }

            if (request.Municipio_Id != 0)
            {
                dirección = _repository.Add(BuilderFactories.Dirección(request.Barrio, request.Direccion, request.CodigoPostal, request.Municipio_Id, request.Cliente_Id));
            }

            if (dirección == null)
            {
                return new ServiceResponse() { Mensaje = "Usuario fue Creado con exito, (Error al registrar Dirección)", Status = false };
            }

            if (_unitOfWork.Commit() == 1)
            {
                return new ServiceResponse() { Mensaje = "Usuario fue Creado con exito, (Dirección(es) creada(s) con exito)", Status = true };
            }
            else
            {
                return new ServiceResponse() { Mensaje = "Usuario fue Creado con exito, (No se pudo registrar las/la Dirección(es)", Status = false };
            }
        }

        public ServiceResponse Edit(ServicioDireccíonRequest request)
        {
            Dirección dirección = null;
            if (request.Id != 0)
            {
                dirección = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();
                dirección.Barrio = request.Barrio;
                dirección.CodigoPostal = request.CodigoPostal;
                dirección.Direccion = request.Direccion;
                dirección.Municipio_Id = request.Municipio_Id;
                _repository.Edit(dirección);
            }


            if (dirección == null)
            {
                return new ServiceResponse() { Mensaje = "Usuario fue Creado con exito, (Error al Modificar Dirección)", Status = false };
            }

            if (_unitOfWork.Commit() == 1)
            {
                return new ServiceResponse() { Mensaje = "Usuario fue Creado con exito, (Dirección(s) Modificado(s) con exito)", Status = true };
            }
            else
            {
                return new ServiceResponse() { Mensaje = "Usuario fue Creado con exito, (No se pudo Modificar los/el Dirección(es)", Status = false };
            }

        }

        public IEnumerable<Dirección> Get(ServicioDireccíonRequest request)
        {
            if (request.Id != 0 && request.Barrio != null && request.CodigoPostal != null && request.Direccion != null && request.Cliente_Id != 0)
            {
                return _repository.FindBy(x =>
                    x.Id == request.Id &&
                    x.Barrio == request.Barrio && 
                    x.CodigoPostal == request.CodigoPostal &&
                    x.Direccion == request.Direccion &&
                    x.Cliente_Id == request.Cliente_Id
                    ).ToList();
            }
            else
            {
                if (request.Id != 0)
                {
                    return _repository.FindBy(x => x.Id == request.Id).ToList();
                }

                else if (request.Barrio != null && request.Cliente_Id != 0)
                {
                    return _repository.FindBy(x => x.Barrio == request.Barrio && request.Cliente_Id == x.Cliente_Id).ToList();
                }
                else if (request.CodigoPostal != null && request.Cliente_Id != 0)
                {
                    return _repository.FindBy(x => x.CodigoPostal == request.CodigoPostal && x.Cliente_Id == request.Cliente_Id).ToList();
                }
                else if (request.Direccion != null && request.Cliente_Id != 0)
                {
                    return _repository.FindBy(x => x.Direccion == request.Direccion && x.Cliente_Id == request.Cliente_Id).ToList();
                }

                else if (request.Cliente_Id != 0)
                {
                    return _repository.FindBy(x => x.Cliente_Id == request.Cliente_Id).ToList();
                }
                else
                {
                    return null;
                }
            }
        }
    }




    public class ServicioDireccíonRequest : Dirección
    {
        public IEnumerable<Dirección> Direcciónes { set; get; }
    }

}
