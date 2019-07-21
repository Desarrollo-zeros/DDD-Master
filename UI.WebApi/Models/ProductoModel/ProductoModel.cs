using Domain.Entities.Producto;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UI.WebApi.Models.FacturaModel;

namespace UI.WebApi.Models.ProductoModel
{
    public class ProductoModel : Model<Producto>
    {
        public Producto Producto { set; get; }
        
        [JsonIgnore]
        private static ProductoModel productoModel;
        public static ProductoModel Instance
        {
            get
            {
                if (productoModel == null)
                {
                    productoModel = new ProductoModel();
                }
                return productoModel;
            }
        }


   

        

        public IEnumerable<Producto> ProductosTop(int top)
        {
            return (
                   from p in Instance.GetAll().ToList()
                   join c in CompraClienteModel.Instance.GetAll().ToList()
                   on p.Id equals c.Producto_Id
                   group p.Id by new
                   {
                       p.Id,
                       p.Nombre,
                       p.Descripción,
                       p.Imagen,
                       p.PrecioCompra,
                       p.PrecioVenta,
                       p.CantidadProducto,
                       p.Categoria_Id,
                       p.FechaCreacion,
                       p.CantidadComprada
                   } into pc
                   orderby pc.Count() descending
                   select new Producto
                   {
                       Id = pc.Key.Id,
                       Nombre = pc.Key.Nombre,
                       Descripción = pc.Key.Descripción,
                       Imagen = pc.Key.Imagen,
                       PrecioCompra = pc.Key.PrecioCompra,
                       PrecioVenta = pc.Key.PrecioVenta,
                       CantidadProducto = pc.Key.CantidadProducto,
                       Categoria_Id = pc.Key.Categoria_Id,
                       FechaCreacion = pc.Key.FechaCreacion,
                       CantidadComprada = pc.Count()
                   }
               ).ToList().Take(top);
        }
    }

}