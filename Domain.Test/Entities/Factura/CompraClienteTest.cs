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
    public class CompraClienteTest
    {
        public CompraCliente compraCliente;

        public CompraClienteTest()
        {
            Initialize();
        }

        [SetUp]
        public void Initialize()
        {
            compraCliente = BuilderFactories.CompraCliente(
                    1, 
                    1,
                    10,
                    Enum.EstadoClienteArticulo.NO_PAGADO);
            compraCliente.Id = 1;
           
        }

        [Test]
        public void CompraClienteAddTest()
        {
            Assert.AreNotEqual(compraCliente, null);
        }
    }
}
