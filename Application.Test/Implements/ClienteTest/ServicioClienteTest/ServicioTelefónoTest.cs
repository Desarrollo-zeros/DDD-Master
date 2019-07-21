using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities.Cliente;
using NUnit.Framework;
using UI.WebApi.Singleton;
using Application.Implements.Cliente.ServicioCliente;

namespace Application.Test.Implements.ClienteTest.ServicioClienteTest
{
    [TestFixture]
    public class ServicioTelefónoTest
    {
        public ServicioTelefóno servicioTelefóno;
        public int cliente_id;
        public ServicioTelefónoTest()
        {
            Initialize();
        }

        [SetUp]
        public void Initialize()
        {
            servicioTelefóno = new ServicioTelefóno(FactoriesSingleton<Telefóno>.UnitOfWork, FactoriesSingleton<Telefóno>.GenericRepository);
            cliente_id = new ServicioClienteTest().ServicioCliente._repository.FindBy(x => x.Documento == "1063969857").FirstOrDefault().Id;
            var telefono=  servicioTelefóno._repository.FindBy(x => x.Número == "3043541475" && x.Cliente_Id != 1).FirstOrDefault();

            if(telefono != null)
            {
                servicioTelefóno.Delete(telefono);
            }
        }

        [Test]
        public void CreateFailsYSuccessTest()
        {
            var telefono = new ServicioTelefónoRequest
            {
                Número = "3043541475",
                Cliente_Id = 1,
                TipoTelefono = Domain.Enum.TipoTelefono.CELULAR
            };
            var response = servicioTelefóno.Create(telefono);
            Assert.AreEqual(response.Mensaje, "Telefono ya registrado");

            telefono.Cliente_Id = cliente_id;
            Console.WriteLine(cliente_id);
            
            response = servicioTelefóno.Create(telefono);
            Assert.AreEqual(response.Mensaje, "Usuario fue Creado con exito, (Telefono(s) creado(s) con exito)");
        }

        [Test]
        public void EditFailsYSuccessTest()
        {
            var telefono = new ServicioTelefónoRequest
            {
                Id = servicioTelefóno._repository.FindBy().LastOrDefault().Id,
                Número = "3043541475",
                Cliente_Id = 1,
                TipoTelefono = Domain.Enum.TipoTelefono.CELULAR
            };
            var response = servicioTelefóno.Create(telefono);
        }
    }
}
