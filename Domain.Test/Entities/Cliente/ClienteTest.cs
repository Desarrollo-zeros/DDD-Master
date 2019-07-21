using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Cliente;
using NUnit.Framework;
using Domain.Factories;
using Domain.ValueObjects;
using Domain.Entities.Localizacíon;
using Domain.Entities.Factura;
using Domain.Entities.Producto;
using Domain.Enum;

namespace Domain.Test.Entities.Cliente
{
    [TestFixture]
    public class ClienteTest
    {
        public Domain.Entities.Cliente.Cliente cliente;

        public ClienteTest()
        {
            Initialize();
        }

        [SetUp]
        public void Initialize()
        {

            var nombre = new Nombre("carlos", "andres", "castilla", "garcia");

            //inicializo cliente
            cliente = BuilderFactories.Cliente("1063969856", nombre, "carloscastilla31@gmail.com", 1);
            cliente.Id = 1;
            //inicializo usuario
            cliente.Usuario = BuilderFactories.Usuario("zeros", "toor", true, Enum.Rol.ADMINISTRADOR);
            cliente.Usuario.Id = 1;

            cliente.Telefónos = new List<Telefóno>
            {
                BuilderFactories.Telefóno("3043541475", Enum.TipoTelefono.CELULAR, 1)
            };
            cliente.Telefónos.FirstOrDefault().Id = 1;

            cliente.Direcciónes = new List<Dirección>
            {
                BuilderFactories.Dirección("El recreo","cr 22 # 16-76","000",1,1)
            };

            cliente.Direcciónes.FirstOrDefault().Id = 1;

            cliente.Direcciónes.FirstOrDefault().Municipio = new Municipio("Bosconia", 1)
            {
                Departamento = new Departamento("Cesar", 1),
            };

            cliente.Direcciónes.FirstOrDefault().Municipio.Departamento.País = new País("Colombia", Enum.Continente.AMÉRICA_SUR);

            cliente.ClienteMetodoDePagos = new List<ClienteMetodoDePago>
            {
                BuilderFactories.ClienteMetodoDePago(1, true, 10000, Enum.CreditCardType.Visa, "5269736513905509", "000", "carlos ", new DateTime(2019, 10, 20))

            };
            cliente.ClienteMetodoDePagos.FirstOrDefault().Id = 1;

            cliente.ClienteMetodoDePagos.FirstOrDefault().Cliente = null;

        }


        [Test]
        public void GetUsuarioSuccessTest()
        {
            var usuario = cliente.Usuario.Id == cliente.Usuario_Id ? cliente : null;

            Assert.AreEqual(usuario, cliente);
        }

        [Test]
        public void GetUsuarioFailsTest()
        {
            var usuario = cliente.Usuario.Id == 2 ? cliente : null;
            Assert.AreEqual(usuario, null);
        }


        [Test]
        public void ValidarCreditCardNumberTestFails()
        {
            var ex = Assert.Throws<Exception>(() => new CreditCard(CreditCardType.Mastercard, "123456789123456", "000", "carlos", new DateTime(2019, 07, 29)));
            Assert.That(ex.Message, Is.EqualTo("Numero Tarjeta invalido"));
        }


     
        [Test]
        public void AuemtarSaldoTestSuccess()
        {
            Assert.AreEqual(cliente.ClienteMetodoDePagos.FirstOrDefault().AumentarSaldo(1000), true);
            Assert.AreEqual(cliente.ClienteMetodoDePagos.FirstOrDefault().Saldo, 11000);
        }

        [Test]
        public void AuemtarSaldoTestFails()
        {
            Assert.AreEqual(cliente.ClienteMetodoDePagos.FirstOrDefault().AumentarSaldo(0), false);
            Assert.AreEqual(cliente.ClienteMetodoDePagos.FirstOrDefault().Saldo, 10000);
        }

        [Test]
        public void DescontarSaldoTestFails()
        {
            Assert.AreEqual(cliente.ClienteMetodoDePagos.FirstOrDefault().DescontarSaldo(0), false);
            Assert.AreEqual(cliente.ClienteMetodoDePagos.FirstOrDefault().Saldo, 10000);
        }

        [Test]
        public void DescontarSaldoTestSuccess()
        {
            Assert.AreEqual(cliente.ClienteMetodoDePagos.FirstOrDefault().DescontarSaldo(9000), true);
            Assert.AreEqual(cliente.ClienteMetodoDePagos.FirstOrDefault().Saldo, 1000);
        }


    }

    

}
