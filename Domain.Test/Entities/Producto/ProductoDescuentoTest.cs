using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Factories;
using NUnit.Framework;
using Domain.Entities.Producto;

namespace Domain.Test.Entities.Producto
{
    [TestFixture]
    class ProductoDescuentoTest
    {
        public ProductoDescuento productoDescuento;

        public ProductoDescuentoTest()
        {
            Initialize();
        }

        [SetUp]
        public void Initialize()
        {
            productoDescuento = BuilderFactories.ProductoDescuento(1,1,Enum.EstadoDescuento.ACTIVO);
            productoDescuento.Producto = new ProductoTest().producto;
        }

        [Test]
        public void ProductoDescuentoAddTest()
        {
            Assert.AreNotEqual(productoDescuento, null);
        }

    }
}
