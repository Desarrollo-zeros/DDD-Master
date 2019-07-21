using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.ValueObjects
{
    public class Nombre
    {
        public Nombre()
        {
        }

        public Nombre(string primerNombre, string segundoNombre, string primerApellido, string segundoApellido)
        {
            PrimerNombre = primerNombre.ToLower();
            SegundoNombre = segundoNombre.ToLower();
            PrimerApellido = primerApellido.ToLower();
            SegundoApellido = segundoApellido.ToLower();
        }


        [Column("PrimerNombre")] public string PrimerNombre { set; get; }
        [Column("SegundoNombre")] public string SegundoNombre { set; get; }
        [Column("PrimerApellido")] public string PrimerApellido { set; get; }
        [Column("SegundoApellido")] public string SegundoApellido { set; get; }


        private string ConstructorNombre()
        {
            return this.PrimerNombre + " " + this.SegundoNombre + " " + this.PrimerApellido + " " + this.SegundoApellido;
        }
    }
}
