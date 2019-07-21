using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Domain.Enum;
using Domain.Entities.Cliente;

namespace Domain.Entities.Factura
{
    [Table("Compra_Envio")]
    public class CompraEnvio : Entity<int>
    {
        public CompraEnvio(int compra_Id, int dirección_Id, DateTime fechaEnvio, DateTime fechaLLegada, EstadoDeEnvio estadoDeEnvio)
        {
            Compra_Id = compra_Id;
            Dirección_Id = dirección_Id;
            FechaEnvio = fechaEnvio;
            FechaLLegada = fechaLLegada;
            EstadoDeEnvio = estadoDeEnvio;
            
        }

        public CompraEnvio() {
            
        }

        public int Compra_Id { set; get; }
        [ForeignKey("Compra_Id")] public Compra Compra { set; get; }

        public int Dirección_Id { set; get; }
        [ForeignKey("Dirección_Id")] public Dirección Dirección { set; get; }

        public DateTime FechaEnvio { set; get; }
        public DateTime FechaLLegada { set; get; }
        public EstadoDeEnvio EstadoDeEnvio { set; get; }

        public virtual IEnumerable<CompraEnvioProducto> CompraEnvioProductos { set; get; }
       

        public bool EnviarProducto(int producto_id)
        {
            if(EstadoDeEnvio == EstadoDeEnvio.EN_VERIFICACIÓN)
            {
                CompraEnvioProducto compraEnvioProducto = CompraEnvioProductos.ToList().Find(x => x.Producto_Id == producto_id);
                if(compraEnvioProducto == null)
                {
                    throw new Exception("No existe el producto a enviar");
                }

                if (compraEnvioProducto.EstadoDeEnvioProducto == EstadoDeEnvioProducto.ENVIADO)
                {
                    return false;
                }
                compraEnvioProducto.EstadoDeEnvioProducto = EstadoDeEnvioProducto.ENVIADO;
                return true;
            }
            return false;
        }

        public bool EnviarProducto()
        {
            if (EstadoDeEnvio == EstadoDeEnvio.EN_VERIFICACIÓN)
            {
                if(CompraEnvioProductos.Count() == 0)
                {
                    throw new Exception("no hay productos que enviar");
                }
                CompraEnvioProductos.ToList().ForEach(x =>
                {
                    x.EstadoDeEnvioProducto = EstadoDeEnvioProducto.ENVIADO;
                });
                return true;
            }
            return false;
        }

    }
}
