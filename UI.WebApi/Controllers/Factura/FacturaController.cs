using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UI.WebApi.Generico;
using UI.WebApi.Models.ClienteModel.UsuarioM;
using UI.WebApi.Models.FacturaModel;

namespace UI.WebApi.Controllers.Factura
{
    [RoutePrefix("api/Factura")]
    [AllowAnonymous]
    public class FacturaController : ApiController
    {
        private readonly UsuarioModel usuario;
        public FacturaController() : base()
        {
            usuario = new UsuarioModel();
            if (usuario.id == 0)
            {
                throw new ArgumentException(Constants.NO_AUTH);
            }
        }

        [HttpPost]
        [Route("compra_create")]
        public IHttpActionResult CrearCompra(CompraModel compraModel)
        {
      
            if (compraModel == null || compraModel.Compra == null)
            {
                return Json(Mensaje<Domain.Entities.Factura.Compra>.MensajeJson(Constants.IS_ERROR, "Objecto no puede estar vacio", Constants.COMPRA_FAIL));
            }
            return Json(compraModel.Comprar(compraModel.Compra));
        }

        [HttpGet]
        [Route("compra_completar/{compra_id}")]
        public IHttpActionResult CompletarCompra(int compra_id)
        {
            return Json(CompraModel.Instance.CompletarCompra(compra_id));
        }
    }
}
