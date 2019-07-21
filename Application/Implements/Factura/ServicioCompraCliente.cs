using Application.Base;
using Domain.Abstracts;
using Domain.Entities.Factura;
using Domain.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implements.Factura
{
    public class ServicioCompraCliente : EntityService<CompraCliente>
    {
        readonly IUnitOfWork _unitOfWork;

        public ServicioCompraCliente(IUnitOfWork unitOfWork, IGenericRepository<CompraCliente> repository) : base(unitOfWork, repository)
        {
            _unitOfWork = unitOfWork;
        }

        public ServiceResponse Create(ServicioCompraClienteRequest request)
        {
            if(request == null)
            {
                return new ServiceResponse
                {
                    Mensaje = "Compra Cliente No puede estar vacio",
                    Status = false
                };
            }

            var CompraCliente = base.Create(BuilderFactories.CompraCliente(request.Producto_Id, request.Compra_Id, request.Cantidad,request.EstadoClienteArticulo));

            if(CompraCliente == null)
            {
                return new ServiceResponse
                {
                    Mensaje = "Compra Cliente No Pudo Crearse",
                    Status = false
                };
            }

            return new ServiceResponse
            {
                Id = CompraCliente.Id,
                Mensaje = "Compra Cliente creada con exito",
                Status = true
            };
        }


        public ServiceResponse Edit(ServicioCompraClienteRequest request)
        {
            if (request == null)
            {
                return new ServiceResponse
                {
                    Mensaje = "Compra Cliente No puede estar vacio",
                    Status = false
                };
            }

            var compraCliente = Find(request.Id);
            if(compraCliente == null)
            {
                return new ServiceResponse
                {
                    Mensaje = "Compra Cliente No existe",
                    Status = false
                };
            }

            compraCliente.Compra_Id = request.Compra_Id;
            compraCliente.Cantidad = request.Cantidad;
            compraCliente.EstadoClienteArticulo = request.EstadoClienteArticulo;
            compraCliente.Producto_Id = request.Producto_Id;

            if (!base.Update(compraCliente))
            {
                return new ServiceResponse
                {
                    Mensaje = "Compra Cliente No pudo Editarse",
                    Status = false
                };
            }
            return new ServiceResponse
            {
                Id = compraCliente.Id,
                Mensaje = "Compra Cliente Creada con exito",
                Status = false
            };
        }
    }

    public class ServicioCompraClienteRequest: CompraCliente
    {

    }



}
