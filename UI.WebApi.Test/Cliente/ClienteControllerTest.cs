using Domain.Entities.Cliente;
using Domain.Factories;
using Domain.ValueObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UI.WebApi.Test.Cliente
{
   

    [TestFixture]
    class ClienteControllerTest
    {
       

        public RestClient client;

        
        public Domain.Entities.Cliente.Cliente Cliente;

        [SetUp]
        public void Initialize()
        {
            client = new RestClient("http://localhost/JsonTest/cliente/");
            var nombre = new Nombre("string", "string", "string", "string");
            Cliente = BuilderFactories.Cliente("string", nombre, "string", 1);
            Cliente.Id = 1;
        }

        [Test]
        public void ClienteFormatoSuccessTest1()
        {
            var request = new RestRequest("ClienteSuccess1.json", Method.GET);
            var response = client.Execute<List<Domain.Entities.Cliente.Cliente>>(request);
            JObject jsonResponse = (JObject)JsonConvert.DeserializeObject(response.Content);
            var cliente = JsonConvert.DeserializeObject<Domain.Entities.Cliente.Cliente>(jsonResponse["Cliente"].ToString());
            Assert.AreEqual(cliente.Documento, Cliente.Documento);
            Assert.AreEqual(cliente.Nombre.PrimerApellido, Cliente.Nombre.PrimerApellido);
            Assert.AreEqual(cliente.Nombre.SegundoApellido, Cliente.Nombre.SegundoApellido);
            Assert.AreEqual(cliente.Nombre.PrimerNombre, Cliente.Nombre.PrimerNombre);
            Assert.AreEqual(cliente.Nombre.SegundoNombre, Cliente.Nombre.SegundoNombre);
            Assert.AreEqual(cliente.Email, Cliente.Email);
        }


        [Test]
        public void ClienteFormatoSuccessTest2()
        {
            var request = new RestRequest("ClienteSuccess2.json", Method.GET);
            var response = client.Execute<List<Domain.Entities.Cliente.Cliente>>(request);
            JObject jsonResponse = (JObject)JsonConvert.DeserializeObject(response.Content);
            var cliente = JsonConvert.DeserializeObject<Domain.Entities.Cliente.Cliente>(jsonResponse["Cliente"].ToString());
            Assert.AreEqual(cliente.Documento, Cliente.Documento);
            Assert.AreEqual(cliente.Nombre.PrimerApellido, Cliente.Nombre.PrimerApellido);
            Assert.AreEqual(cliente.Nombre.SegundoApellido, Cliente.Nombre.SegundoApellido);
            Assert.AreEqual(cliente.Nombre.PrimerNombre, Cliente.Nombre.PrimerNombre);
            Assert.AreEqual(cliente.Nombre.SegundoNombre, Cliente.Nombre.SegundoNombre);
            Assert.AreEqual(cliente.Email, Cliente.Email);
            Assert.IsNotNull(cliente.Telefónos);
            Assert.IsNotNull(cliente.Direcciónes);
        }



        [Test]
        public void ClienteFormatoFails1()
        {

            var request = new RestRequest("ClienteFails1.json", Method.GET);
            var response = client.Execute<List<Domain.Entities.Cliente.Cliente>>(request);
            JObject jsonResponse = (JObject)JsonConvert.DeserializeObject(response.Content);
            var cliente = JsonConvert.DeserializeObject<Domain.Entities.Cliente.Cliente>(jsonResponse["Cliente"].ToString());
            Assert.AreEqual(cliente.Nombre, null);
        }

        [Test]
        public void ClienteFormatoFails2()
        {

            var request = new RestRequest("ClienteFails2.json", Method.GET);
            var response = client.Execute<List<Domain.Entities.Cliente.Cliente>>(request);
            JObject jsonResponse = (JObject)JsonConvert.DeserializeObject(response.Content);
            Assert.AreEqual(jsonResponse["Cliente"], null);
        }

        [Test]
        public void PayFormatoSuccess()
        {
            var request = new RestRequest("PaySuccess.json", Method.GET);
            var response = client.Execute<List<ClienteMetodoDePago>>(request);
            JObject jsonResponse = (JObject)JsonConvert.DeserializeObject(response.Content);
            var pay = JsonConvert.DeserializeObject<List<ClienteMetodoDePago>>(jsonResponse["ClienteMetodoDePagos"].ToString());
            Assert.IsNotNull(pay);
            Assert.AreEqual(pay.FirstOrDefault().Saldo, 0);
        }
    }
}
