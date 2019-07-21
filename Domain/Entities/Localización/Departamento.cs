using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Localizacíon
{
    public class Departamento : Entity<int>
    {
        public Departamento(string nombre, int país_Id, País país, IEnumerable<Municipio> municipios)
        {
            Nombre = nombre;
            País_Id = país_Id;
            País = país;
            Municipios = municipios;
        }

        public Departamento(string nombre, int país_Id)
        {
            Nombre = nombre;
            País_Id = país_Id;
        }

        public Departamento()
        {

        }

        public string Nombre { set; get; }
        public int País_Id { set; get; }
        [ForeignKey("País_Id")] public País País { set; get; }

        public virtual IEnumerable<Municipio> Municipios { set; get; }

       
    }


}
