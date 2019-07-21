using Domain.Entities.Factura;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.WebApi.Models.FacturaModel
{
    public class ComprobanteDePagoModel : Model<ComprobanteDePago>
    {
        public ComprobanteDePago ComprobanteDePago { set; get; }

        [JsonIgnore]
        private static ComprobanteDePagoModel comprobanteDePagoModel;
        public static ComprobanteDePagoModel Instance
        {
            get
            {
                if (comprobanteDePagoModel == null)
                {
                    comprobanteDePagoModel = new ComprobanteDePagoModel();
                }
                return comprobanteDePagoModel;
            }
        }

    }
}