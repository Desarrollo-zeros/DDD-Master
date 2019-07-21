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
    public class CategoriaTest
    {
        public Categoria categoria;

        [SetUp]
        public void Initialize()
        {
            categoria = BuilderFactories.Categoria("ejemplo", "ejemplo", DateTime.Now);
            categoria.Id = 1;
        }
        [Test]
        public void CategoriaAddTest()
        {
            Assert.AreNotEqual(categoria,null);
        }

    }
}
