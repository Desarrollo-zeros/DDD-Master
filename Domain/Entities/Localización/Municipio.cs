using Domain.Entities.Cliente;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Localizacíon
{
    public class Municipio : Entity<int>
    {
     

        public Municipio(string nombre, int departamento_Id)
        {
            Nombre = nombre;
            Departamento_Id = departamento_Id;
          
        }

        public Municipio() { }

        public string Nombre { set; get; }
        public int Departamento_Id { set; get; }
        [ForeignKey("Departamento_Id")] public Departamento Departamento { set; get; }

        public virtual IEnumerable<Dirección> Direcciónes { set; get; }
    }


}
