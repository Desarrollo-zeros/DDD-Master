using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Producto;
using Domain.Factories;
using Domain.Entities.Factura;

namespace Domain.Test.Entities.Factura
{
    [TestFixture]
    class CompraEnvioTest
    {
        public CompraEnvio compraEnvio;

        public CompraEnvioTest()
        {
            Initialize();
        }

        [SetUp]
        public void Initialize()
        {
            compraEnvio = new CompraEnvio( 1, 1, DateTime.Now, DateTime.Now, Enum.EstadoDeEnvio.EN_VERIFICACIÓN)
            {
                Id = 1
            };
        }

        [Test]
        public void CompraEnvioAddTest()
        {
            Assert.AreNotEqual(compraEnvio, null);
        }
    }
}
