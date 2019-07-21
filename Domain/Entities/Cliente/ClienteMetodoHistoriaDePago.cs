using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Cliente
{
    [Table("Cliente_Metodo_Historia_DePago")]
    public class ClienteMetodoHistoriaDePago : Entity<int>
    {
        public ClienteMetodoHistoriaDePago(int cliente_Id, int clienteMetodoDePago_Id, double saldoAnterio, double saldoNuevo, string iP, DateTime fechaMovimiento)
        {
            Cliente_Id = cliente_Id;
            ClienteMetodoDePago_Id = clienteMetodoDePago_Id;
            SaldoAnterio = saldoAnterio;
            SaldoNuevo = saldoNuevo;
            IP = iP;
            FechaMovimiento = fechaMovimiento;
        }

        public int Cliente_Id { set; get; }
        [ForeignKey("Cliente_Id")] public Cliente Cliente { set; get; }
        public int ClienteMetodoDePago_Id { set; get; }
        [ForeignKey("ClienteMetodoDePago_Id")]public ClienteMetodoDePago ClienteMetodoDePago { set; get; }
        public double SaldoAnterio { set; get; }
        public double SaldoNuevo { set; get; }
        public string IP { set; get; }
        public DateTime FechaMovimiento { set; get; }
    }

}
