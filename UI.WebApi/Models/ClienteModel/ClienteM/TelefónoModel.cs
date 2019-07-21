using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Application.Implements.Cliente.ServicioCliente;
using Domain.Abstracts;
using Domain.Entities.Cliente;
using Newtonsoft.Json;
using UI.WebApi.Singleton;

namespace UI.WebApi.Models.ClienteModel.ClienteM
{
    public class TelefónoModel : Model<Telefóno>
    {
     
        public Telefóno Telefóno { set; get; }

        public TelefónoModel()
        {
        }

        public static TelefónoModel Instance
        {
            get
            {
                if (telefónoModel == null)
                {
                    telefónoModel = new TelefónoModel();
                }
                return telefónoModel;
            }
        }

        [JsonIgnore]
        private static TelefónoModel telefónoModel;

    }
}