using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Producto;
using Domain.Factories;
using NUnit.Framework;

namespace Domain.Test.Entities.Producto
{

    [TestFixture]
    class DescuentoTest
    {
        public Descuento descuento;

        public DescuentoTest()
        {
            Initialize();
        }

        [SetUp]
        public void Initialize()
        {
            descuento =  BuilderFactories.Descuento(Enum.TipoDescuento.FIJO, true, new DateTime(2019, 7, 20, 1, 1, 1), new DateTime(2019, 7, 21, 20, 1, 1), 0.5);
        }
        [Test]
        public void DescuentoAddTest()
        {
            Assert.AreNotEqual(descuento, null);
        }
        [Test]
        public void DescuentoFailsExceptionFechaTest()
        {
            var exc = Assert.Throws<Exception>(() => BuilderFactories.Descuento(Enum.TipoDescuento.FIJO, true, DateTime.Now, DateTime.Now, 0.5));
            Assert.That(exc.Message, Is.EqualTo("Solo se puede crear descuento para Fines de semanas"));
        }

    }
}
