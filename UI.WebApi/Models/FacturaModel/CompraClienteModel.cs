using Domain.Entities.Factura;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.WebApi.Models.FacturaModel
{
    public class CompraClienteModel : Model<CompraCliente>
    {
        [JsonIgnore]
        private static CompraClienteModel compraClienteModel;
        public static CompraClienteModel Instance
        {
            get
            {
                if (compraClienteModel == null)
                {
                    compraClienteModel = new CompraClienteModel();
                }
                return compraClienteModel;
            }
        }
    }
}