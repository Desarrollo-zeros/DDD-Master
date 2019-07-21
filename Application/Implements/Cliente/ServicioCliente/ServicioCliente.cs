using Application.Base;
using Domain.Abstracts;
using Domain.Entities.Cliente;
using Domain.Entities.Factura;
using Domain.Factories;
using Domain.ValueObjects;
using Infraestructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implements.Cliente.ServicioCliente
{
    public class ServicioCliente  : EntityService<Domain.Entities.Cliente.Cliente>
    {
        readonly IUnitOfWork _unitOfWork;
        
        public ServicioCliente(IUnitOfWork unitOfWork, IGenericRepository<Domain.Entities.Cliente.Cliente> repository) : base(unitOfWork, repository)
        {
            _unitOfWork = unitOfWork;
            
        }

        public ServiceResponse Create(ServicioClienteRequest request)
        {
            if (_repository.FindBy(x => x.Usuario_Id == request.Usuario_Id).FirstOrDefault() != null)
            {
                return new ServiceResponse() { Mensaje = "Cliente ya existe", Status = false };
            }

            if (_repository.FindBy(x => x.Documento == request.Documento).FirstOrDefault() != null)
            {
                return new ServiceResponse() { Mensaje = "Documento ya existe", Status = false };
            }
            if (_repository.FindBy(x => x.Email == request.Email).FirstOrDefault() != null)
            {
                return new ServiceResponse() { Mensaje = "Email ya existe", Status = false };
            }

            var builderCLient = BuilderFactories.Cliente(request.Documento, request.Nombre, request.Email, request.Usuario_Id);

            var cliente = _repository.Add(builderCLient);

            if (_unitOfWork.Commit() == 1)
            {
                return new ServiceResponse() { Mensaje = "Cliente creado exito", Status = true, Id = cliente.Id };
            }
            else
            {
                return new ServiceResponse() { Mensaje = "No se pudo crear cliente", Status = false };
            }
            
        }


        public Domain.Entities.Cliente.Cliente Get(ServicioClienteRequest request)
        {
            if (request.Id != 0)
            {
                return _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();
            }
            if (request.Documento != null)
            {
                return _repository.FindBy(x => x.Documento == request.Documento).FirstOrDefault();
            }
            if (request.Email != null)
            {
                return _repository.FindBy(x => x.Email == request.Email).FirstOrDefault();
            }
            if (request.Usuario_Id != 0)
            {
                return _repository.FindBy(x => x.Usuario_Id == request.Usuario_Id).FirstOrDefault();
            }
            return null;
        }

        public ServiceResponse Edit(ServicioClienteRequest request)
        {
           
            var cliente = Get(new ServicioClienteRequest { Usuario_Id = request.Usuario_Id });
          
            if(cliente == null)
            {
                return new ServiceResponse() { Mensaje = "Cliente no existe", Status = false };
            }
            
            if(cliente.Documento != request.Documento)
            {
                if (_repository.FindBy(x => x.Documento == request.Documento && x.Usuario_Id != cliente.Usuario_Id).FirstOrDefault() != null)
                {
                    return new ServiceResponse() { Mensaje = "Documento ya existe", Status = false };
                }
                else
                {
                    cliente.Documento = request.Documento;
                }
            }

            if (cliente.Email != request.Email)
            {

                if (_repository.FindBy(x => x.Email == request.Email && x.Usuario_Id != cliente.Usuario_Id).FirstOrDefault() != null)
                {
                    return new ServiceResponse() { Mensaje = "Email ya existe", Status = false };
                }
                else
                {
                    cliente.Email = request.Email;
                }
            }

            cliente.Nombre = request.Nombre;
            _repository.Edit(cliente);

            if (_unitOfWork.Commit() == 1)
            {
                return new ServiceResponse() { Mensaje = "Cliente Modificado con exito", Status = true , Id = cliente.Id };
            }
            else
            {
                return new ServiceResponse() { Mensaje = "No se pudo Modificar cliente", Status = false };
            }
        }


    }

    public class ServicioClienteRequest : Domain.Entities.Cliente.Cliente
    {
       
    }

}
