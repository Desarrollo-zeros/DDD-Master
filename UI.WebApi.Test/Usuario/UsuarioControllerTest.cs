using Domain.Entities.Cliente;
using Domain.Factories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;

namespace UI.WebAPi.Test
{
    [TestFixture]
    public class UsuarioControllerTest
    {
        public RestClient client;

        public IRestResponse<Usuario> response;
        public Usuario Usuario;

        [SetUp]
        public void Initialize()
        {
            client = new RestClient("http://localhost/JsonTest/usuario/");
            Usuario = BuilderFactories.Usuario("ejemplo", "ejemplo", true, Domain.Enum.Rol.DEV);
        }

        [Test]
        public void FormatoSuccessTest()
        {

            var request = new RestRequest("usuarioSuccess.json", Method.GET);
            var response = client.Execute<List<Usuario>>(request);
            JObject jsonResponse = (JObject)JsonConvert.DeserializeObject(response.Content);
            var usuario = JsonConvert.DeserializeObject<Usuario>(jsonResponse["Usuario"].ToString());
            Assert.AreEqual(usuario.Username, Usuario.Username);
            Console.WriteLine(usuario.Password);
            Assert.AreEqual(usuario.Password, "ejemplo");
        }

        [Test]
        public void FormatoFails()
        {

            var request = new RestRequest("usuarioFails.json", Method.GET);
            var response = client.Execute(request);
            JObject jsonResponse = (JObject)JsonConvert.DeserializeObject(response.Content);
            Assert.AreEqual(jsonResponse["usuario"], null);
        }

      
    }
}