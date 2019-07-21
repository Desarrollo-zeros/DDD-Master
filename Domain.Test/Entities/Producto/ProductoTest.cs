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
    public class ProductoTest
    {
        public Domain.Entities.Producto.Producto producto;

        public ProductoTest()
        {
            Initialize();
        }


        [SetUp]
        public void Initialize()
        {
            producto = BuilderFactories.Producto("ejemplo", "ejemplo", "", 1000, 1200, 10,1);
            producto.Id = 1;
            producto.Categoria = new CategoriaTest().categoria;
        }

        [Test]
        public void ProductoAddTest()
        {
            Assert.AreNotEqual(producto, null);
        }
    }
}
