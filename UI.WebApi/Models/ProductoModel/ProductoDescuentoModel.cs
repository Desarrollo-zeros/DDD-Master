using Domain.Entities.Producto;
using Newtonsoft.Json;

namespace UI.WebApi.Models.ProductoModel
{
    public class ProductoDescuentoModel : Model<ProductoDescuento>
    {
        public ProductoDescuento ProductoDescuento { set; get; }

        [JsonIgnore]
        private static ProductoDescuentoModel productoDescuentoModel;
        public static ProductoDescuentoModel Instance
        {
            get
            {
                if (productoDescuentoModel == null)
                {
                    productoDescuentoModel = new ProductoDescuentoModel();
                }
                return productoDescuentoModel;
            }
        }
    }

}