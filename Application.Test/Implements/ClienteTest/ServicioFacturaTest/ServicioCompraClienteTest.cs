using Application.Implements.Factura;
using Domain.Entities.Factura;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.WebApi.Singleton;

namespace Application.Test.Implements.ClienteTest.ServicioFacturaTest
{
    [TestFixture]
    class ServicioCompraClienteTest
    {
        public ServicioCompraCliente servicioCompraCliente;
        public ServicioCompraClienteTest()
        {
            Initialize();
        }

        [SetUp]
        public void Initialize()
        {
            servicioCompraCliente = new ServicioCompraCliente(FactoriesSingleton<CompraCliente>.UnitOfWork, FactoriesSingleton<CompraCliente>.GenericRepository);
        }

        [Test]
        public void CreateSuccessTest()
        {
           var compraCliente =  new ServicioCompraClienteRequest
            {
                Cantidad = 10,
                Compra_Id = 1,
                Producto_Id = 1,
                EstadoClienteArticulo = Domain.Enum.EstadoClienteArticulo.NO_PAGADO
            };

            var response = servicioCompraCliente.Create(null);
            Assert.AreEqual(response.Mensaje, "Compra Cliente No puede estar vacio");

            response = servicioCompraCliente.Create(compraCliente);
            Assert.AreEqual(response.Mensaje, "Compra Cliente creada con exito");
        }

        [Test]
        public void EditSuccessTest()
        {
            var compraCliente = new ServicioCompraClienteRequest
            {
                Id = 110,
                Cantidad = 10,
                Compra_Id = 1,
                Producto_Id = 1,
                EstadoClienteArticulo = Domain.Enum.EstadoClienteArticulo.NO_PAGADO
            };

            var response = servicioCompraCliente.Edit(null);
            Assert.AreEqual(response.Mensaje, "Compra Cliente No puede estar vacio");

            response = servicioCompraCliente.Edit(compraCliente);
            Assert.AreEqual(response.Mensaje, "Compra Cliente No existe");

            compraCliente.Id = 1;

            response = servicioCompraCliente.Edit(compraCliente);
            Assert.AreEqual(response.Mensaje, "Compra Cliente Creada con exito");
        }
    }
}
