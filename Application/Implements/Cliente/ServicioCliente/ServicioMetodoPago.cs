using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Base;
using Domain.Abstracts;
using Domain.Entities.Cliente;
using Domain.Factories;

namespace Application.Implements.Cliente.ServicioCliente
{
    public class ServicioMetodoPago : EntityService<ClienteMetodoDePago>
    {
        readonly IUnitOfWork _unitOfWork;
        
        public ServicioMetodoPago(IUnitOfWork unitOfWork, IGenericRepository<ClienteMetodoDePago> repository) : base(unitOfWork, repository)
        {
            _unitOfWork = unitOfWork;
           
        }

        public ServiceResponse Create(ServicioMetodoPagoRequest request)
        {
            var metodoPago = Get(request);

            if(metodoPago != null)
            {
                return new ServiceResponse
                {
                    Mensaje = "Metodo de pago ya existe",
                    Status = false
                };
            }
            var buildMetodoPago = BuilderFactories.ClienteMetodoDePago(request.Cliente_Id, request.Activo, request.Saldo, request.CreditCard.Type, request.CreditCard.CardNumber, request.CreditCard.SecurityNumber, request.CreditCard.OwnerName, request.CreditCard.ExpirationDate);
            metodoPago = _repository.Add(buildMetodoPago);

            if (_unitOfWork.Commit() == 1)
            {
                return new ServiceResponse
                {
                    Id = metodoPago.Id,
                    Mensaje = "Metodo de pago creado con exito",
                    Status = true
                };
            }
            else
            {
                return new ServiceResponse
                {
                    Mensaje = "Metodo no pudo crearse",
                    Status = false
                };
            }

        }

        public ServiceResponse Edit(ServicioMetodoPagoRequest request)
        {
            var metodoPago = Get(new ServicioMetodoPagoRequest { Id = request.Id, Cliente_Id = request.Cliente_Id  });
         
            if (metodoPago == null)
            {
                return new ServiceResponse
                {
                    Mensaje = "Metodo de pago no existe",
                    Status = false
                };
            }

            metodoPago.Cliente = request.Cliente;

            if (metodoPago.Cliente == null)
            {
                return new ServiceResponse
                {
                    Mensaje = "CLiente no existe",
                    Status = false
                };
            }

            if (metodoPago.Saldo > request.Saldo)
            {

                if (!metodoPago.DescontarSaldo(request.Saldo))
                {
                    return new ServiceResponse
                    {
                        Mensaje = "No tiene permiso para modificar su saldo, los datos no fueron modificados",
                        Status = false
                    };
                }
            }

            if (metodoPago.Saldo < request.Saldo)
            {
                if (!metodoPago.AumentarSaldo(request.Saldo))
                {
                    return new ServiceResponse
                    {
                        Mensaje = "No tiene permiso para modificar su saldo, los datos no fueron modificados",
                        Status = false
                    };
                }
            }

            metodoPago.Activo = request.Activo;

            if (metodoPago.CreditCard.CardNumber != request.CreditCard.CardNumber)
            {
                var m = Get(new ServicioMetodoPagoRequest { CreditCard = request.CreditCard });
                if (m != null)
                {
                    return new ServiceResponse
                    {
                        Mensaje = "Numero de tarjeta ya esta en uso",
                        Status = false
                    };
                }
                else
                {
                    metodoPago.CreditCard = request.CreditCard;
                }
            }

            metodoPago.Cliente = null;

            _repository.Edit(metodoPago);
            if (_unitOfWork.Commit() == 1)
            {
                return new ServiceResponse
                {
                    Mensaje = "Metodo de pago actualizado",
                    Status = true
                };
            }
            else
            {
                return new ServiceResponse
                {
                    Mensaje = "No se puedo modificar el metodo de pago",
                    Status = false
                };
            }
        }


        public ClienteMetodoDePago Get(ServicioMetodoPagoRequest request)
        {
            if (request.Id > 0 && request.Cliente_Id == 0)
            {
                return _repository.FindBy(x => x.Id == request.Id).FirstOrDefault();
            }
            if (request.Id > 0 && request.Cliente_Id != 0)
            {
                return _repository.FindBy(x => x.Id == request.Id && request.Cliente_Id == x.Cliente_Id).FirstOrDefault();
            }
            if (null != request.CreditCard.CardNumber)
            {
                return _repository.FindBy(x => x.CreditCard.CardNumber == request.CreditCard.CardNumber).FirstOrDefault();
            }
            return null;
        }


        public IEnumerable<ClienteMetodoDePago> GetAll(ServicioMetodoPagoRequest request)
        {
            var meotodo = _repository.FindBy(x => x.Cliente_Id == request.Cliente_Id);
            return meotodo;
        }
    }

    public class ServicioMetodoPagoRequest : ClienteMetodoDePago
    {

    }
}
