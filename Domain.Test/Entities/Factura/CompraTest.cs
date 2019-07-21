using Domain.Entities.Factura;
using Domain.Entities.Producto;
using Domain.Enum;
using Domain.Factories;
using Domain.Test.Entities.Cliente;
using Domain.Test.Entities.Producto;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Test.Entities.Factura
{
    [TestFixture]
    class CompraTest
    {
        public Compra compra;

        public CompraTest()
        {
            Initialize();
        }

        [SetUp]
        public void Initialize()
        {
            compra = BuilderFactories.Compra(1,DateTime.Now);
            compra.Id = 1;
            compra.Cliente = new ClienteTest().cliente;

            compra.CompraClientes = new List<CompraCliente>(){
                new CompraClienteTest().compraCliente
            };
            compra.CompraClientes.FirstOrDefault().Producto = new ProductoTest().producto;

            compra.CompraClientes.FirstOrDefault().Producto.ProductoDescuentos = new List<ProductoDescuento>()
            {
                new ProductoDescuentoTest().productoDescuento
            };
            compra.CompraClientes.FirstOrDefault().Producto.ProductoDescuentos.FirstOrDefault().Descuento = new DescuentoTest().descuento;
            compra.ComprobanteDePagos = new List<ComprobanteDePago>()
            {
                BuilderFactories.ComprobanteDePago(EstadoDePago.EN_ESPERA, compra.ObtenerTotal(), compra.ObtenerSubTotal(),MedioPago.EFECTIVO,0,DateTime.Now,compra.ObtenerDescuento(), 1)
            };

            compra.CompraEnvios = new List<CompraEnvio>()
            {
                new CompraEnvioTest().compraEnvio

            };
            compra.CompraEnvios.FirstOrDefault().Id = 1;
            compra.CompraEnvios.FirstOrDefault().Compra = compra;

            compra.CompraEnvios.FirstOrDefault().CompraEnvioProductos = new List<CompraEnvioProducto>
            {
                new CompraEnvioProducto(1, 1, 1, DateTime.Now, Enum.EstadoDeEnvioProducto.NO_ENVIADO)
            };
            compra.CompraEnvios.FirstOrDefault().EstadoDeEnvio = Enum.EstadoDeEnvio.EN_VERIFICACIÓN;
        }

        [Test]
        public void CompraAddTest()
        {
            Assert.AreNotEqual(compra, null);
        }

        [Test]
        public void ComprarYDescontarSaldoSucessTest()
        {
            Console.WriteLine(compra.Cliente.ClienteMetodoDePagos.FirstOrDefault().Saldo);
            Assert.AreEqual(compra.DescontarTotalProductoEnSaldo(100), true);
            Assert.AreEqual(compra.Cliente.ClienteMetodoDePagos.First().Saldo, 9900);
        }

        //saldo a descontar no puede ser mayor o igual al saldo de la cuenta
        [Test]
        public void ComprarYDescontarSaldoFailTest1()
        {
            Assert.AreEqual(compra.DescontarTotalProductoEnSaldo(0), false);
            Assert.AreEqual(compra.Cliente.ClienteMetodoDePagos.First().Saldo, 10000);
        }

        [Test]
        public void ComprarYDescontarSaldoFailTest2()
        {
            Assert.AreEqual(compra.DescontarTotalProductoEnSaldo(10000), false);
            Assert.AreEqual(compra.Cliente.ClienteMetodoDePagos.First().Saldo, 10000);
        }

        [Test]
        public void ComprarYDescontarSaldoFailTest3()
        {
            Assert.AreEqual(compra.DescontarTotalProductoEnSaldo(20000), false);
            Assert.AreEqual(compra.Cliente.ClienteMetodoDePagos.First().Saldo, 10000);
        }

        [Test]
        public void ComprarCambioEstadoDePagoSuccessTest()
        {
            double x = compra.ObtenerDescuentoPorProductoCompra(1, 5);
            Assert.AreEqual(3000, (int)x);
            Assert.AreEqual(compra.DescontarTotalProductoEnSaldo(7000 - x), true);
            Assert.AreEqual(compra.Cliente.ClienteMetodoDePagos.First().Saldo, 6000);
            Assert.AreEqual(compra.ComprobanteDePagos.FirstOrDefault().EstadoDePago, Enum.EstadoDePago.PAGADO);
        }

        [Test]
        public void ComprarCambioEstadoDePagoFailTest()
        {
            compra.Cliente_Id = 1;
            double x = compra.ObtenerDescuentoPorProductoCompra(1, 5);
            Assert.AreEqual(3000, (int)x);
            Assert.AreEqual(compra.DescontarTotalProductoEnSaldo(13000 - x), false);
            Assert.AreEqual(compra.Cliente.ClienteMetodoDePagos.First().Saldo, 10000);
            Assert.AreEqual(compra.ComprobanteDePagos.FirstOrDefault().EstadoDePago, Enum.EstadoDePago.EN_ESPERA);
        }

        [Test]
        public void ObtenerDescuentoCompraSuccessTest()
        {
            compra.Cliente_Id = 1;
            double x = compra.ObtenerDescuentoPorProductoCompra(1, 5);
            Assert.AreEqual((int)x, 3000);
        }

        [Test]
        public void ObtenerDescuentoCompraExcepcionesTest()
        {
            compra.CompraClientes.FirstOrDefault().Producto = null;
            var ex1 = Assert.Throws<Exception>(() => compra.ObtenerDescuentoPorProductoCompra(1, 10));
            Assert.That(ex1.Message, Is.EqualTo("Producto esta vacio"));

            compra.CompraClientes = null;

            var ex2 = Assert.Throws<Exception>(() => compra.ObtenerDescuentoPorProductoCompra(1, 10));
            Assert.That(ex2.Message, Is.EqualTo("Compra cliente esta vacio"));
        }

        [Test]
        public void ComprarArticulosSuccessTest()
        {
            Assert.AreEqual(compra.CompletarCompras(), true);
            Assert.AreEqual(compra.CompraClientes.FirstOrDefault().EstadoClienteArticulo, Enum.EstadoClienteArticulo.PAGADO);
            Assert.AreEqual(compra.Cliente.ClienteMetodoDePagos.FirstOrDefault().Saldo, 4000);
        }

        [Test]
        public void RealizarEnvioSuccessProducto()
        {
            compra.ComprobanteDePagos.FirstOrDefault().EstadoDePago = Enum.EstadoDePago.PAGADO;
            Assert.AreEqual(compra.EnviarCompra(1), true);
            
        }

        [Test]
        public void RealizarEnvioFailProducto()
        {
            compra.ComprobanteDePagos.FirstOrDefault().EstadoDePago = Enum.EstadoDePago.EN_VERIFICACION;
            Assert.That(Assert.Throws<Exception>(() => compra.EnviarCompra(1)).Message, Is.EqualTo("No existe Un Estado De pago"));
            compra.ComprobanteDePagos.FirstOrDefault().EstadoDePago = Enum.EstadoDePago.PAGADO;
            Assert.That(Assert.Throws<Exception>(() => compra.EnviarCompra(2)).Message, Is.EqualTo("No existe el producto a enviar"));
        }

    }
}
