using Domain.Entities.Cliente;
using Domain.Entities.Factura;
using Domain.Entities.Producto;
using Domain.Factories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;

namespace UI.WebApi.Test.Factura
{
    [TestFixture]
    class FacturaControllerTest
    {
        public RestClient client;
        [SetUp]
        public void Initialize()
        {
            client = new RestClient("http://localhost/JsonTest/factura/");
        }

        [Test]
        public void CompraFormatoSuccessTest()
        {
            var request = new RestRequest("CompraSuccess.json", Method.GET);
            var response = client.Execute<List<Compra>>(request);
            JObject jsonResponse = (JObject)JsonConvert.DeserializeObject(response.Content);
            var categoria = JsonConvert.DeserializeObject<Compra>(jsonResponse["Compra"].ToString());

            Assert.AreEqual(categoria.Cliente_Id, 1);
            Assert.AreEqual(categoria.FechaCompra, new DateTime(2019, 7, 21));

        }


        [Test]
        public void CompraFormatoFailsTest()
        {
            var request = new RestRequest("CompraFails.json", Method.GET);
            var response = client.Execute<List<Compra>>(request);
            JObject jsonResponse = (JObject)JsonConvert.DeserializeObject(response.Content);
            Assert.AreEqual(jsonResponse["Compra"], null);

        }
    }
}
