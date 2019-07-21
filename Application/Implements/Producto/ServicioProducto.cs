using Application.Base;
using Domain.Abstracts;
using Domain.Entities.Factura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implements.Producto
{
    public class ServicioProducto : EntityService<Domain.Entities.Producto.Producto>
    {
        public ServicioProducto(IUnitOfWork unitOfWork, IGenericRepository<Domain.Entities.Producto.Producto> repository) : base(unitOfWork, repository)
        {
        }

        public bool GetExiste(ServicioProductoRequest request)
        {
            if (request.Id < 1 || request.Nombre == "") return false;

            if (_repository.FindBy(x => x.Nombre == request.Nombre && x.Id != request.Id).FirstOrDefault() != null) return true;

            return false;

        }

        public IEnumerable<Domain.Entities.Producto.Producto> GetByCategory(int id)
        {
            return _repository.FindBy(x => x.Categoria_Id == id).ToList();
        }

    }


    public class ServicioProductoRequest : Domain.Entities.Producto.Producto
    {

    }
}
