using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Producto
{
    public class Categoria : Entity<int>
    {
        public Categoria(string nombre, string descripción, DateTime fechaCreacion)
        {
            Nombre = nombre;
            Descripción = descripción;
            FechaCreacion = fechaCreacion;
        }

        public Categoria() { }

        [Column("Categoria")]
        public string Nombre { set; get; }
        public string Descripción { set; get; }
        public DateTime FechaCreacion { set; get; }

        public virtual IEnumerable<Producto> Productos { set; get; }
    }
}
