using Application.Base;
using Domain.Abstracts;
using Domain.Entities.Cliente;
using Domain.Enum;
using Domain.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implements.Cliente.ServicioCliente
{
    public class ServicioTelefóno : EntityService<Telefóno>
    {
        readonly IUnitOfWork _unitOfWork;
       
        public ServicioTelefóno(IUnitOfWork unitOfWork, IGenericRepository<Telefóno> repository) :base(unitOfWork, repository)
        {
            _unitOfWork = unitOfWork;

        }

        public ServiceResponse Create(ServicioTelefónoRequest request)
        {
            var  telefóno =_repository.FindBy(x => x.Cliente_Id == request.Cliente_Id && x.Número == request.Número).FirstOrDefault();
            if(telefóno != null) {
                return new ServiceResponse() { Mensaje = "Telefono ya registrado", Status = false };
            }
            
            if(request.Número != null)
            {
                telefóno = _repository.Add(BuilderFactories.Telefóno(request.Número, request.TipoTelefono, request.Cliente_Id));
            }

            if(telefóno == null)
            {
                return new ServiceResponse() { Mensaje = "Usuario fue Creado con exito, (Error al registrar telefono)", Status = false };
            }
            
            if(_unitOfWork.Commit() == 1)
            {
                return new ServiceResponse() { Mensaje = "Usuario fue Creado con exito, (Telefono(s) creado(s) con exito)", Status = true};
            }
            else
            {
                return new ServiceResponse() { Mensaje = "Usuario fue Creado con exito, (No se pudo registrar los/el telefono)", Status = false };
            }
        }


        public ServiceResponse Edit(ServicioTelefónoRequest request)
        {
            Telefóno telefóno = null;

            if (request.Id != 0)
            {
                telefóno = _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();
                telefóno.Número = request.Número;
                telefóno.TipoTelefono = request.TipoTelefono;
                _repository.Edit(telefóno); 
            }
           
            if (telefóno == null)
            {
                return new ServiceResponse() { Mensaje = "Usuario fue Creado con exito, (Error al Modificar telefono)", Status = false };
            }

            if (_unitOfWork.Commit() == 1)
            {
                return new ServiceResponse() { Mensaje = "Usuario fue Creado con exito, (Telefono(s) Modificado(s) con exito)", Status = true };
            }
            else
            {
                return new ServiceResponse() { Mensaje = "Usuario fue Creado con exito, (No se pudo Modificar los/el telefono)", Status = false };
            }
        }

        public int GetId(ServicioTelefónoRequest request)
        {
            return _repository.FindBy(x => x.Id == request.Id).FirstOrDefault().Id;
        }

        public IEnumerable<Telefóno> Get(ServicioTelefónoRequest request)
        {
            if (request.Id != 0 && request.Número != null && request.TipoTelefono != TipoTelefono.DESCONOCIDO &&  request.Cliente_Id != 0)
            {
                return _repository.FindBy(x =>
                    x.Id == request.Id &&
                    x.TipoTelefono == request.TipoTelefono &&
                    x.Número == request.Número &&
                    x.Cliente_Id == request.Cliente_Id
                    ).ToList();
            }
            else
            {
                if (request.Id != 0)
                {
                    return _repository.FindBy(x => x.Id == request.Id).ToList();
                }
                else if (request.Número != null && request.Cliente_Id != 0)
                {
                    return _repository.FindBy(x => x.Número == request.Número && x.Cliente_Id == request.Cliente_Id).ToList();
                }
                else if (request.TipoTelefono != TipoTelefono.DESCONOCIDO && 0 != request.Cliente_Id)
                {
                    return _repository.FindBy(x => x.TipoTelefono == request.TipoTelefono && x.Cliente_Id == request.Cliente_Id).ToList();
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

    public class ServicioTelefónoRequest : Telefóno
    {
        public IEnumerable<Telefóno> Telefónos { set; get; }

    }

}
