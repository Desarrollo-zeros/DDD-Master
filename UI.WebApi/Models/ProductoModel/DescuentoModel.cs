using Domain.Entities.Producto;
using Newtonsoft.Json;

namespace UI.WebApi.Models.ProductoModel
{
    public class DescuentoModel : Model<Descuento>
    {
        public Descuento Descuento { set; get; }

        [JsonIgnore]
        private static DescuentoModel descuentoModel;
        public static DescuentoModel Instance
        {
            get
            {
                if (descuentoModel == null)
                {
                    descuentoModel = new DescuentoModel();
                }
                return descuentoModel;
            }
        }
    }

}