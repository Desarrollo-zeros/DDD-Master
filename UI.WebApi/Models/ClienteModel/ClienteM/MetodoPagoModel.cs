using Application.Implements.Cliente.ServicioCliente;
using Domain.Abstracts;
using Domain.Entities.Cliente;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UI.WebApi.Singleton;

namespace UI.WebApi.Models.ClienteModel.ClienteM
{
    public class MetodoPagoModel : Model<ClienteMetodoDePago>
    {
        public IEnumerable<ClienteMetodoDePago> ClienteMetodoDePagos { set; get; }

        public MetodoPagoModel() 
        {
           
        }

        public static MetodoPagoModel Instance
        {
            get
            {
                if (metodoPagoModel == null)
                {
                    metodoPagoModel = new MetodoPagoModel();
                }
                return metodoPagoModel;
            }
        }

        [JsonIgnore]
        private static MetodoPagoModel metodoPagoModel;

        public static MetodoPagoModel GetAll(int idUsuario)
        {
            Instance.ClienteMetodoDePagos = ClienteModel.GetAll(idUsuario).Cliente.ClienteMetodoDePagos;
            return Instance;
        }

        public static MetodoPagoModel Get(int idUsuario, int idMetodoPago)
        {
            Instance.ClienteMetodoDePagos = ClienteModel.GetAll(idUsuario).Cliente.ClienteMetodoDePagos.Where(x=>x.Id == idMetodoPago);
            return Instance;
        }
    }
}