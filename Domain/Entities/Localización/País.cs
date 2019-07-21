using System;
using System.Collections.Generic;
using Domain.Enum;

namespace Domain.Entities.Localizacíon
{
    public class País : Entity<int>
    {

        public País()
        {

        }
        public País(string nombre, Continente continente)
        {
            Nombre = nombre;
            Continente = continente;
        }

        public País(string nombre, Continente continente, IEnumerable<Departamento> departamentos)
        {
            Nombre = nombre;
            Continente = continente;
            Departamentos = departamentos;
        }

        public string Nombre { set; get; }
        public Continente Continente { set; get; }
        public virtual IEnumerable<Departamento> Departamentos { set; get; }
    }


}
