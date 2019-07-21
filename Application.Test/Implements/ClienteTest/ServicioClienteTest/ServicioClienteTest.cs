using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Cliente;
using NUnit.Framework;
using Domain.Factories;
using Domain.ValueObjects;
using Domain.Entities.Localizacíon;
using Domain.Entities.Factura;
using Domain.Entities.Producto;
using Domain.Enum;
using Application.Implements.Cliente.ServicioUsuario;
using UI.WebApi.Singleton;
using Application.Implements.Cliente.ServicioCliente;
using Application.Test.Implements.ClienteTest.ServicioUsuarioTest;


namespace Application.Test.Implements.ClienteTest.ServicioClienteTest
{
    [TestFixture]
    public class ServicioClienteTest
    {
        public ServicioCliente ServicioCliente;
        int usuario_id = 0;

        public ServicioClienteTest()
        {
            Initialize();
        }

        [SetUp]
        public void Initialize()
        {
            
            ServicioCliente = new ServicioCliente(FactoriesSingleton<Cliente>.UnitOfWork, FactoriesSingleton<Cliente>.GenericRepository);
            
        }

        [Test]
        public void CreateFailsYSuccessTest()
        {
            new ServicioUsuarioTest.ServicioUsuarioTest().ServicioUsuario.Delete(new ServicioUsuarioTest.ServicioUsuarioTest().ServicioUsuario._repository.FindBy(x => x.Username == "test").LastOrDefault());
            new ServicioUsuarioTest.ServicioUsuarioTest().CreateSuccessTest();

            var nombre = new Nombre("carlos", "andres", "castilla", "garcia");
            var FactoryCliente = BuilderFactories.Cliente("1063969856", nombre, "carloscastilla31@gmail.com", 1);
            var cliente = new ServicioClienteRequest()
            {
                Documento = FactoryCliente.Documento,
                Email = FactoryCliente.Email,
                Nombre = FactoryCliente.Nombre,
                Usuario_Id = 1
            };

            var responseCliente = ServicioCliente.Create(cliente);

            Assert.AreEqual(responseCliente.Mensaje, "Cliente ya existe");

            if (usuario_id == 0)
            {
                usuario_id = new ServicioUsuarioTest.ServicioUsuarioTest().ServicioUsuario.GetId(new ServicioUsuarioRequest { Username = "test" });
            }

            cliente.Usuario_Id = usuario_id;
            responseCliente = ServicioCliente.Create(cliente);
            Assert.AreEqual(responseCliente.Mensaje, "Documento ya existe");
            cliente.Documento = "1063969858";
            responseCliente = ServicioCliente.Create(cliente);
            Assert.AreEqual(responseCliente.Mensaje, "Email ya existe");

            cliente.Email = "carlostest@gmail.com";

            responseCliente = ServicioCliente.Create(cliente);
            Assert.AreEqual(responseCliente.Mensaje, "Cliente creado exito");
            usuario_id = new ServicioUsuarioTest.ServicioUsuarioTest().ServicioUsuario.GetId(new ServicioUsuarioRequest { Username = "test" });
        }


        [Test]
        public void EditFailsYSuccessTest()
        {
            if(usuario_id == 0)
            {
                usuario_id = new ServicioUsuarioTest.ServicioUsuarioTest().ServicioUsuario.GetId(new ServicioUsuarioRequest { Username = "test" });
            }

            var nombre = new Nombre("carlos", "andres", "castilla", "garcia");
            var FactoryCliente = BuilderFactories.Cliente("1063969856", nombre, "carloscastilla31@gmail.com", usuario_id);

            var cliente = new ServicioClienteRequest()
            {
                Documento = FactoryCliente.Documento,
                Email = FactoryCliente.Email,
                Nombre = FactoryCliente.Nombre,
                Usuario_Id = 100
            };

            var responseCliente = ServicioCliente.Edit(cliente);

            Assert.AreEqual(responseCliente.Mensaje, "Cliente no existe");
            cliente.Usuario_Id = usuario_id;
            responseCliente = ServicioCliente.Edit(cliente);
            Assert.AreEqual(responseCliente.Mensaje, "Documento ya existe");
            cliente.Documento = "1063969857";
            responseCliente = ServicioCliente.Edit(cliente);
            Assert.AreEqual(responseCliente.Mensaje, "Email ya existe");
            cliente.Email = "test@test.com";
            responseCliente = ServicioCliente.Edit(cliente);
            Assert.AreEqual(responseCliente.Mensaje, "Cliente Modificado con exito");
        }
    }
}
