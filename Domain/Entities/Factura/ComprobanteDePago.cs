using Domain.Enum;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Factura
{
    [Table("Comprobante_De_Pago")]
    public class ComprobanteDePago : Entity<int>
    {
        public ComprobanteDePago(EstadoDePago estadoDePago, double total, double subTotal, MedioPago medioPago, double monto, DateTime fechaDePago, double totalDescuentoAplicados, int compra_id)
        {
            EstadoDePago = estadoDePago;
            Total = total;
            SubTotal = subTotal;
            MedioPago = medioPago;
            FechaDePago = fechaDePago;
            Monto = monto;
            TotalDescuentoAplicados = totalDescuentoAplicados;
            Compra_Id = compra_id;
        }

        public ComprobanteDePago() { }


        public int Compra_Id { set; get; }

        [ForeignKey("Compra_Id")]
        public Compra Compra { set; get; }

        [Column("Estado_de_pago")]  public EstadoDePago EstadoDePago { set; get; }
        public double Total { set; get; }
        public double SubTotal { set; get; }

        [Column("Medio_de_pago")] public MedioPago MedioPago { set; get; }

        [Column("Monto_Pagado")] public double Monto { get; set; }

        public DateTime FechaDePago { set; get; }


        [Column("Total_Descuento_Aplicado")] public double TotalDescuentoAplicados { set; get; }

    }
}
