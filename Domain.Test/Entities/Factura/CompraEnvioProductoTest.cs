using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Producto;
using Domain.Factories;
using Domain.Entities.Factura;
using Domain.Test.Entities.Producto;

namespace Domain.Test.Entities.Factura
{
    [TestFixture]
    public class CompraEnvioProductoTest
    {
        public CompraEnvioProducto compraEnvioProducto;

        public CompraEnvioProductoTest()
        {
            Initialize();
        }

        [SetUp]
        public void Initialize()
        {
            compraEnvioProducto = new CompraEnvioProducto(
                    1,
                    1, 
                    1,
                    DateTime.Now,
                    Enum.EstadoDeEnvioProducto.NO_ENVIADO
                );
        }


        [Test]
        public void CompraEnvioProductoAddTest()
        {
            Assert.AreNotEqual(compraEnvioProducto, null);
        }

    }
}
