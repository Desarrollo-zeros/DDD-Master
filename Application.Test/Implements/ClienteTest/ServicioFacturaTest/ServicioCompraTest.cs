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
    public class ServicioCompraTest
    {
        public ServicioCompra servicioCompra;
        public ServicioCompraTest()
        {
            Initialize();
        }

        [SetUp]
        public void Initialize()
        {
            servicioCompra = new ServicioCompra(FactoriesSingleton<Compra>.UnitOfWork, FactoriesSingleton<Compra>.GenericRepository);
        }

        [Test]
        public void CreateSuccessTest()
        {

            var compra = new ServicioCompraRequest
            {
                Cliente_Id = 1,
                FechaCompra = DateTime.Now
            };

            var response = servicioCompra.Create(null);
            Assert.AreEqual(response.Mensaje,"Compra no debe estar vacia");

            response = servicioCompra.Create(compra);
            Assert.AreEqual(response.Mensaje, "Compra Creada con exito");
        }

    }
}
