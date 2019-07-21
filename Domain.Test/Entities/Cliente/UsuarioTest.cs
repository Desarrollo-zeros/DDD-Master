using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Cliente;
using NUnit.Framework;
using Domain.Factories;

namespace Domain.Test.Entities.Cliente
{
    

    [TestFixture]
    class UsuarioTest
    {
        private Usuario usuario;


        [SetUp]
        public void Initialize()
        {
            usuario = Factories.BuilderFactories.Usuario("zeros", "toor", true, Enum.Rol.INVITADO);
            usuario.Id = 1;
        }

        //usuarios y contraseña Correctos
        [Test]
        public void AutentificateTestSuccess()
        {
            var auth = usuario.Authenticate("zeros", usuario.EncryptPassword("toor"));
            Assert.AreEqual(auth, true);
        }


        //usuarios y contraseña invalidos
        [Test]
        public void AutentificateTestFails()
        {
            var auth = usuario.Authenticate("zeros", "123");
            Assert.AreEqual(auth, false);
        }

    }
}
