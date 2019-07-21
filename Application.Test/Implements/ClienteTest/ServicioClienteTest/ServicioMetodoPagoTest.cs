using Application.Implements.Cliente.ServicioCliente;
using Domain.Entities.Cliente;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.WebApi.Singleton;
using Application.Test.Implements.ClienteTest.ServicioUsuarioTest;

namespace Application.Test.Implements.ClienteTest.ServicioClienteTest
{
    [TestFixture]
    public class ServicioMetodoPagoTest
    {
        public ServicioMetodoPago servicioMetodoPago;

        public ServicioMetodoPagoTest()
        {
            Initialize();
        }

        [SetUp]
        public void Initialize()
        {
            servicioMetodoPago = new ServicioMetodoPago(FactoriesSingleton<ClienteMetodoDePago>.UnitOfWork, FactoriesSingleton<ClienteMetodoDePago>.GenericRepository);
        }

        [Test]
        public void CreateSuccessTest()
        {
            servicioMetodoPago.Delete(servicioMetodoPago._repository.FindBy(x=>x.CreditCard.CardNumber == "5106929458367416").FirstOrDefault());

            var metodoPago = new ServicioMetodoPagoRequest
            {
                Cliente_Id = 1,
                Saldo = 10000,
                Activo = true,
                Cliente = new ServicioClienteTest().ServicioCliente._repository.FindBy().LastOrDefault(),
                CreditCard = new Domain.ValueObjects.CreditCard(Domain.Enum.CreditCardType.Mastercard, "5493725989475483","000","carlos", new DateTime(2019,12,12))
            };

            var response = servicioMetodoPago.Create(metodoPago);
            Assert.AreEqual(response.Mensaje, "Metodo de pago ya existe");

            metodoPago.CreditCard.CardNumber = "5106929458367416";

            response = servicioMetodoPago.Create(metodoPago);
            Assert.AreEqual(response.Mensaje, "Metodo de pago creado con exito");
        }

        [Test]
        public void EditFailsYSuccessTest()
        {
            var cliente = new ServicioClienteTest().ServicioCliente._repository.FindBy(x=>x.Documento == "1063969856").FirstOrDefault();

            var metodoPago = new ServicioMetodoPagoRequest
            {
                Id = 100,
                Cliente_Id = 1,
                Saldo = 0,
                Activo = true,
                Cliente = null,
                CreditCard = new Domain.ValueObjects.CreditCard(Domain.Enum.CreditCardType.Mastercard, "5493725989475483", "000", "carlos", new DateTime(2019, 12, 12))
            };

            var response = servicioMetodoPago.Edit(metodoPago);
            Assert.AreEqual(response.Mensaje, "Metodo de pago no existe");

            metodoPago.Id = 1;

            response = servicioMetodoPago.Edit(metodoPago);
            Assert.AreEqual(response.Mensaje, "CLiente no existe");
            metodoPago.Cliente = cliente;

            response = servicioMetodoPago.Edit(metodoPago);
            Assert.AreEqual(response.Mensaje, "No tiene permiso para modificar su saldo, los datos no fueron modificados");
            metodoPago.Saldo = 10000;
          
            response = servicioMetodoPago.Edit(metodoPago);
            Assert.AreEqual(response.Mensaje, "Metodo de pago actualizado");
        }
    }
}
