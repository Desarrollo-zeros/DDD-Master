using Domain.Entities.Factura;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Cliente
{
    public class Cliente : Entity<int>
    {
        public Cliente(string Documento, Nombre Nombre, string Email, int Usuario_Id)
        {
            this.Documento = Documento;
            this.Nombre = Nombre;
            this.Email = Email.ToLower();
            this.Usuario_Id = Usuario_Id;
        }
        public Cliente() {

        }
        public string Documento { set; get; }
        public Nombre Nombre { set; get; }
        public string Email { set; get; }

        public int Usuario_Id { set; get; }
        [ForeignKey("Usuario_Id")] public Usuario Usuario { set; get; }

        public virtual IEnumerable<Telefóno> Telefónos { set; get; }

        public virtual IEnumerable<Dirección> Direcciónes { set; get; }

        public virtual IEnumerable<ClienteMetodoDePago> ClienteMetodoDePagos { set; get; }
        public virtual IEnumerable<CompraCliente> CompraClientes { set; get; }

     

    }
}
