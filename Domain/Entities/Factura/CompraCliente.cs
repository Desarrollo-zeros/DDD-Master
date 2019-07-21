using Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Domain.Entities.Factura
{
    [Table("Compra_Cliente")]
    public class CompraCliente : Entity<int>
    {
        public CompraCliente(int producto_Id, int compra_Id, int cantidad, Enum.EstadoClienteArticulo estadoProductoCliente)
        {
            Producto_Id = producto_Id;
            Compra_Id = compra_Id;
            Cantidad = cantidad;
            EstadoClienteArticulo = estadoProductoCliente;
        }

        public CompraCliente() { }

        public int Compra_Id { set; get; }
        [ForeignKey("Compra_Id")] public Compra Compra { set; get; }
      
        public int Producto_Id { set; get; }
        [ForeignKey("Producto_Id")] public Producto.Producto Producto { set; get; }
        public int Cantidad { set; get; }

        public EstadoClienteArticulo EstadoClienteArticulo { set; get; }

    }
}
