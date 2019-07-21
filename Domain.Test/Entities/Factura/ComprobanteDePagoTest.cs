using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Factura;
using Domain.Test.Entities.Producto;
using NUnit.Framework;

namespace Domain.Test.Entities.Factura
{
    [TestFixture]
    public class ComprobanteDePagoTest
    {
        public ComprobanteDePago comprobanteDePago;
        public ComprobanteDePagoTest()
        {
            Initialize();
        }

        [SetUp]
        public void Initialize()
        {
            comprobanteDePago = new ComprobanteDePago(
                Enum.EstadoDePago.EN_VERIFICACION,
                10000,
                12000,
                Enum.MedioPago.EFECTIVO,
                15000,
                DateTime.Now,
                600,
                1);
        }

        [Test]
        public void ComprobanteDePagoAddTest()
        {
            Assert.AreNotEqual(comprobanteDePago, null);
        }

    }
}
