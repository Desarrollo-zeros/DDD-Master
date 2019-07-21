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
    public class ServicioCompra : Base.EntityService<Compra>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public ServicioCompra(IUnitOfWork unitOfWork, IGenericRepository<Compra> repository) : base(unitOfWork, repository)
        {
            _unitOfWork = unitOfWork;
        }

        public ServiceResponse Create(ServicioCompraRequest request)
        {
            if(request == null)
            {
                return new ServiceResponse
                {
                    Mensaje = "Compra no debe estar vacia",
                    Status = false
                };
            }

            var compra = base.Create(BuilderFactories.Compra(request.Cliente_Id, request.FechaCompra));
            if (compra == null)
            {
                return new ServiceResponse
                {
                    Mensaje = "Compra no pudo crearse",
                    Status = false
                };
            }

            return new ServiceResponse
            {
                Id = compra.Id,
                Mensaje = "Compra Creada con exito",
                Status = true
            };

        }


        
    }

    public class ServicioCompraRequest : Compra
    {

    }

}
