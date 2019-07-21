using Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Factura
{
    [Table("Compra_Envio_Producto")]
    public class CompraEnvioProducto : Entity<int>
    {
        public CompraEnvioProducto(int compraEnvio_Id, int producto_Id, int compra_Id, DateTime fecha, EstadoDeEnvioProducto estadoDeEnvioProducto)
        {
            CompraEnvio_Id = compraEnvio_Id;
            Producto_Id = producto_Id;
            Compra_Id = compra_Id;
            Fecha = fecha;
            EstadoDeEnvioProducto = estadoDeEnvioProducto;
        }

        public CompraEnvioProducto() { }

        [Column("Compra_Envio_Id")]
        public int CompraEnvio_Id { set; get; }
        [ForeignKey("CompraEnvio_Id")] public CompraEnvio CompraEnvio { set; get; }

        public int Producto_Id { set; get; }
        [ForeignKey("Producto_Id")] public Producto.Producto Producto { set; get; }

        public int Compra_Id { set; get; }
        [ForeignKey("Compra_Id")] public Compra Compra { set; get; }


        public DateTime Fecha { set; get; }

        public EstadoDeEnvioProducto EstadoDeEnvioProducto { set; get; }

    }
}
