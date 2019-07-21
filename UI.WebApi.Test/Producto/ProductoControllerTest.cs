using Domain.Entities.Cliente;
using Domain.Entities.Producto;
using Domain.Factories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;

namespace UI.WebApi.Test.Producto
{
    [TestFixture]
    class ProductoControllerTest
    {
        public RestClient client;
        [SetUp]
        public void Initialize()
        {
            client = new RestClient("http://localhost/JsonTest/producto/");
            

        }

        [Test]
        public void CategoriaFormatoSuccessTest()
        {
            var request = new RestRequest("CategoriaSuccess.json", Method.GET);
            var response = client.Execute<List<Categoria>>(request);
            JObject jsonResponse = (JObject)JsonConvert.DeserializeObject(response.Content);
            var categoria = JsonConvert.DeserializeObject<Categoria>(jsonResponse["Categoria"].ToString());

            Assert.AreEqual(categoria.Nombre, "string");
            Assert.AreEqual(categoria.Descripción, "string");
            Assert.AreEqual(categoria.FechaCreacion, new DateTime(2019,7,21));

        }

        [Test]
        public void ProductoFormatoSuccessTest()
        {
            var request = new RestRequest("ProductoSuccess.json", Method.GET);
            var response = client.Execute<List<Domain.Entities.Producto.Producto>>(request);
            JObject jsonResponse = (JObject)JsonConvert.DeserializeObject(response.Content);
            var producto = JsonConvert.DeserializeObject<Domain.Entities.Producto.Producto>(jsonResponse["Producto"].ToString());
            Assert.AreEqual(producto.Nombre, "string");
            Assert.AreEqual(producto.Descripción, "string");
            Assert.AreEqual(producto.Imagen, "string");

        }

        [Test]
        public void DescuentoFormatoSuccessTest()
        {
            var request = new RestRequest("DescuentoSuccess.json", Method.GET);
            var response = client.Execute<List<Descuento>>(request);
            JObject jsonResponse = (JObject)JsonConvert.DeserializeObject(response.Content);
            var descuento = JsonConvert.DeserializeObject<Descuento>(jsonResponse["Descuento"].ToString());
            Assert.AreEqual(descuento.Acomulable, true);
        }

        [Test]
        public void ProductoDescuentoFormatoSuccessTest()
        {
            var request = new RestRequest("ProductoDescuentoSuccess.json", Method.GET);
            var response = client.Execute<List<ProductoDescuento>>(request);
            JObject jsonResponse = (JObject)JsonConvert.DeserializeObject(response.Content);
            var descuento = JsonConvert.DeserializeObject<ProductoDescuento>(jsonResponse["ProductoDescuento"].ToString());
            Assert.AreEqual(descuento.EstadoDescuento, Domain.Enum.EstadoDescuento.EN_PAUSA);
            Assert.AreEqual(descuento.Producto_Id, 1);
            Assert.AreEqual(descuento.Descuento_Id, 1);
        }


    }
}
