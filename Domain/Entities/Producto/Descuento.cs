using Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Producto
{
    public class Descuento : Entity<int>
    {
        public Descuento(TipoDescuento tipoDescuento, bool acomulable, DateTime fechaYHoraInicio, DateTime fechaYHoraTerminación, double descuento)
        {
            if (!DescuentoEsAplicable(fechaYHoraInicio, fechaYHoraTerminación))
            {
                throw new Exception("Solo se puede crear descuento para Fines de semanas");
            }
            this.TipoDescuento = tipoDescuento;
            Acomulable = acomulable;
            FechaYHoraInicio = fechaYHoraInicio;
            FechaYHoraTerminación = fechaYHoraTerminación;
            Descu = descuento;
        }
        public Descuento() { }

        public TipoDescuento TipoDescuento { set; get; }

        public bool Acomulable { set; get; }
        
        public DateTime FechaYHoraInicio { set; get; }
        public DateTime FechaYHoraTerminación { set; get; }

        [Column("Descuento")]
        public double Descu { set; get; }
        public virtual IEnumerable<ProductoDescuento> ProductoDescuentos { set; get; }

        private bool DiasFinDeSemanas(DateTime fecha)
        {
            return (fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday);
        }


        //los descuento solo seran aplicados los fines de semanas (sabado,domingo)
        public bool DescuentoEsAplicable(DateTime fechaYHoraInicio, DateTime fechaYHoraTerminación)
        {
            bool r = false;
            if(DiasFinDeSemanas(fechaYHoraInicio) && DiasFinDeSemanas(fechaYHoraTerminación))
            {
                if (fechaYHoraInicio.Year == fechaYHoraTerminación.Year && fechaYHoraInicio.Month == fechaYHoraTerminación.Month)
                {
                    if ((fechaYHoraInicio.Day + 1) == fechaYHoraTerminación.Day)
                    {
                        if (fechaYHoraInicio.Hour <=23 && fechaYHoraTerminación.Hour <=23)
                        {
                            r = true;
                        }
                    }
                }
            }
            return r;
        }




    }
}
