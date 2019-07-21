using Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Producto
{

    [Table("Producto_Descuento")]
    public class ProductoDescuento : Entity<int>
    {

        public ProductoDescuento()
        {

        }
        public ProductoDescuento(int producto_Id, int descuento_Id, EstadoDescuento estadoDescuento)
        {
            Producto_Id = producto_Id;
            Descuento_Id = descuento_Id;
            EstadoDescuento = estadoDescuento;
        }

        public int Producto_Id { set; get; }
        [ForeignKey("Producto_Id")] public Producto Producto { set; get; }
        public int Descuento_Id { set; get; }
        [ForeignKey("Descuento_Id")] public Descuento Descuento { set; get; }
        public EstadoDescuento EstadoDescuento { set; get; }

      
     
    }
}
