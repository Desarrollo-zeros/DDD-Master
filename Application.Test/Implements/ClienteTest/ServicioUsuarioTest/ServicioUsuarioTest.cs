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

namespace Application.Test.Implements.ClienteTest.ServicioUsuarioTest
{
    [TestFixture]
    public class ServicioUsuarioTest
    {
        public ServicioUsuario ServicioUsuario;
        public ServicioUsuarioRequest servicioUsuarioRequest;

        public ServicioUsuarioTest()
        {
            Initialize();
        }

        [SetUp]
        public void Initialize()
        {
           
            ServicioUsuario = new ServicioUsuario(FactoriesSingleton<Usuario>.UnitOfWork, FactoriesSingleton<Usuario>.GenericRepository);
           
        }

        [Test]
        public void CreateFailsTest1()
        {
            var factoryUser = BuilderFactories.Usuario("zeros", "toor", true, Rol.CLIENTE);
            var response = ServicioUsuario.Create(new ServicioUsuarioRequest
            {
                Username = factoryUser.Username,
                Password = factoryUser.Password,
                Activo = factoryUser.Activo,
                Rol = factoryUser.Rol

            });
            Assert.AreEqual(response.Status,false);
        }

        [Test]
        public void CreateFailsTest2()
        {
            Assert.That(Assert.Throws<Exception>(() => BuilderFactories.Usuario("", "", true, Rol.INVITADO)).Message, Is.EqualTo("Factories Usuario no puede ser creado"));
        }


        [Test]
        public void CreateSuccessTest()
        {
            var factoryUser = BuilderFactories.Usuario("test", "toor", true, Rol.CLIENTE);

            var response = ServicioUsuario.Create(new ServicioUsuarioRequest {
                Username = factoryUser.Username,
                Password = factoryUser.Password,
                Activo = factoryUser.Activo,
                Rol = factoryUser.Rol

            });
            Console.WriteLine(response.Mensaje);
        }


        [Test]
        public void LogearSuccessTest()
        {
            
            var response = ServicioUsuario.Autenticar(new ServicioUsuarioRequest
            {
                Username = "test",
                Password = new Usuario().EncryptPassword("toor"),
                Activo = true,
                Rol = Rol.INVITADO
            });
            Assert.AreEqual("test", response.Username);
        }

        [Test]
        public void LogearFailsTest()
        {
            var response = ServicioUsuario.Autenticar(new ServicioUsuarioRequest
            {
                Username = "zerox",
                Password = new Usuario().EncryptPassword("toor"),
                Activo = true,
                Rol = Rol.INVITADO
            });
            Assert.AreEqual(response, null);
        }

    }
}
