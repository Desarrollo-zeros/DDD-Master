using Domain.Entities.Producto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    class Cotización
    {
        public virtual List<Producto> Productos { set; get; }
        public virtual List<Descuento> Descuentos { set; get; }

        public double Total { set; get; }

        public Cotización()
        {

        }

        public Cotización(List<Producto> Productos)
        {
            this.Productos = Productos;
        }

        public Cotización(List<Descuento> Descuentos)
        {
            this.Descuentos = Descuentos;
        }

        public Cotización(List<Producto> Productos, List<Descuento> Descuentos)
        {
            this.Productos = Productos;
            this.Descuentos = Descuentos;
        }

        public void TotalProducto()
        {
            foreach (var producto in Productos)
            {
                this.Total += producto.PrecioCompra;
            }
        } 

    }
}
