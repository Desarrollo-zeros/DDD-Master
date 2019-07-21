using Domain.Enum;
using Domain.ValueObjects;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Cliente
{
    [Table("Cliente_Metodo_De_Pago")]
    public class ClienteMetodoDePago : Entity<int>
    {
        public ClienteMetodoDePago(int cliente_Id, bool activo, CreditCard creditCard, double saldo)
        {
            Cliente_Id = cliente_Id;
            CreditCard = creditCard;
            Saldo = saldo;
            Activo = activo;
        }

        public ClienteMetodoDePago(int cliente_Id, double saldo, bool activo)
        {
            Cliente_Id = cliente_Id;
            Saldo = saldo;
            Activo = activo;
        }

        public ClienteMetodoDePago() { }

        public int Cliente_Id { set; get; }
        [ForeignKey("Cliente_Id")] public Cliente Cliente { set; get; }
        public CreditCard CreditCard { set; get; }
        public double Saldo { set; get; }
        public bool Activo { set; get; }

        private bool TieneSaldo(double saldo)
        {
            return this.Saldo > 0;
        }

        private bool SePuedeDescontar(double saldo)
        {
            return (this.Saldo - saldo) > 0;
        }


        public bool DescontarSaldo(double saldo)
        {
            if (TieneSaldo(saldo) && SePuedeDescontar(saldo) && Activo && saldo > 0)
            {
                Saldo -= saldo;
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public bool AumentarSaldo(double saldo)
        {
            if (saldo > 0 && Activo == true)
            {
                Saldo += saldo;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
