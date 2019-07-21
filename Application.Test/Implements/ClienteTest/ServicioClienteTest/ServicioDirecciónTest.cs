using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities.Cliente;
using NUnit.Framework;
using UI.WebApi.Singleton;
using Application.Implements.Cliente.ServicioCliente;
using Application.Base;

namespace Application.Test.Implements.ClienteTest.ServicioClienteTest
{
    [TestFixture]
    public class ServicioDirecciónTest 
    {
        public ServicioDirección servicioDirección;
        int cliente_id = 0, direccion_id = 0;
        public ServicioDirecciónTest()
        {
            Initialize();
        }

        [SetUp]
        public void Initialize()
        {
            servicioDirección = new ServicioDirección(FactoriesSingleton<Dirección>.UnitOfWork, FactoriesSingleton<Dirección>.GenericRepository);
            cliente_id = new ServicioClienteTest().ServicioCliente._repository.FindBy(x => x.Documento == "1063969857").FirstOrDefault().Id;
           
        }

        [Test]
        public void CreateFailsYSuccessTest()
        {

            var direccion = new ServicioDireccíonRequest
            {
                Barrio = "EL recreo",
                Cliente_Id = 1,
                Municipio_Id = 1,
                Direccion = "cr 22 #16-76",
                CodigoPostal = "000",
            };
            
            var response = servicioDirección.Create(direccion);
            Assert.AreEqual(response.Mensaje, "Usuario fue Creado con exito, (Dirección(es) creada(s) con exito)");
        }


        [Test]
        public void EditFailsYSuccessTest()
        {
            var response = servicioDirección.Edit(new ServicioDireccíonRequest
            {   Id = servicioDirección._repository.FindBy().LastOrDefault().Id,
                Barrio = "EL recreo",
                Cliente_Id = cliente_id,
                Municipio_Id = 1,
                Direccion = "cr 22 #16-76"+ direccion_id,
                CodigoPostal = "000",
            });
           
            Assert.AreEqual(response.Status, true);
        }
    }
}
