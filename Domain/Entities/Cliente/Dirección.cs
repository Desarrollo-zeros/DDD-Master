using Domain.Entities.Cliente;
using Domain.Entities.Localizacíon;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Cliente
{
    public class Dirección : Entity<int>
    {
   
        public Dirección(string barrio, string direccion, string codigoPostal, int municipio_Id, int cliente_Id)
        {
            Barrio = barrio;
            Direccion = direccion;
            CodigoPostal = codigoPostal;
            Municipio_Id = municipio_Id;
            Cliente_Id = cliente_Id;
        }

        public Dirección() { }

        public string Barrio { set; get; }
        public string Direccion { set; get; }
        public string CodigoPostal { set; get; }
        public int Municipio_Id { set; get; }
        [ForeignKey("Municipio_Id")] public Municipio Municipio { set; get; }
        
        public int Cliente_Id { set; get; }
        [ForeignKey("Cliente_Id")] public Cliente Cliente { set; get; }
    }


}
