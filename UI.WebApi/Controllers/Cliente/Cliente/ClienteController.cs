using Application.Base;
using Application.Implements.Cliente.ServicioCliente;
using Application.Implements.Cliente.ServicioUsuario;
using Domain.Enum;
using Domain.Factories;
using System;
using System.Linq;
using System.Web.Http;
using UI.WebApi.Generico;
using UI.WebApi.Models;
using UI.WebApi.Models.ClienteModel.ClienteM;
using UI.WebApi.Models.ClienteModel.UsuarioM;

namespace UI.WebApi.Controllers.Cliente.Cliente
{
    [AllowAnonymous]
    [RoutePrefix("api/Cliente")]

    public class ClienteController : ApiController
    {
        private readonly UsuarioModel usuario;


        public ClienteController()
        {
            usuario = new UsuarioModel();
            if (usuario.id == 0)
            {
                throw new ArgumentException(Constants.NO_AUTH);
            }
        }


        [HttpPost]
        [Route("create")]
        public IHttpActionResult Create(ClienteModel clienteModel)
        {
            if (clienteModel.Cliente == null)
            {
                return Json(Mensaje<Domain.Entities.Cliente.Cliente>.MensajeJson(Constants.IS_ERROR, "Objecto no puede estar vacio", Constants.CLIENT_FAIL));
            }

            if (clienteModel.Cliente.Usuario == null)
            {
                return Json(Mensaje<Domain.Entities.Cliente.Usuario>.MensajeJson(Constants.IS_ERROR, "Objecto no puede estar vacio", Constants.USER_FAIL));
            }


            var factoryUser = BuilderFactories.Usuario(clienteModel.Cliente.Usuario.Username, clienteModel.Cliente.Usuario.Password, true, Rol.INVITADO);
            var responseUsuario = usuario.ServicioUsuario.Create(new ServicioUsuarioRequest { Username = factoryUser.Username, Password = factoryUser.Password, Rol = Rol.CLIENTE, Activo = true });

            if (!responseUsuario.Status)
            {
                return Json(Mensaje<Domain.Entities.Cliente.Usuario>.MensajeJson(Constants.IS_ERROR, responseUsuario.Mensaje, Constants.USER_FAIL));

            }

            var FactoryCliente = BuilderFactories.Cliente(clienteModel.Cliente.Documento, clienteModel.Cliente.Nombre, clienteModel.Cliente.Email, responseUsuario.Id);
            var responseCliente = clienteModel.ServicioCliente.Create(new ServicioClienteRequest()
            {
                Documento = FactoryCliente.Documento,
                Email = FactoryCliente.Email,
                Nombre = FactoryCliente.Nombre,
                Usuario_Id = usuario.id
            });

            if (!responseCliente.Status)
            {
                return Json(Mensaje<Domain.Entities.Cliente.Cliente>.MensajeJson(Constants.IS_ERROR, responseCliente.Mensaje, Constants.CLIENT_FAIL));
            }

            if(clienteModel.Cliente.Telefónos != null)
            {
                clienteModel.Cliente.Telefónos.ToList().ForEach(x =>
                {
                    clienteModel.ServicioTelefóno.Create(new ServicioTelefónoRequest
                    {
                        Cliente_Id = responseCliente.Id,
                        Número = x.Número,
                        TipoTelefono = x.TipoTelefono
                    });
                });

            }


            if (clienteModel.Cliente.Direcciónes != null)
            {
                clienteModel.Cliente.Direcciónes.ToList().ForEach(x =>
                {
                    clienteModel.ServicioDirección.Create(new ServicioDireccíonRequest
                    {
                        Cliente_Id = responseCliente.Id,
                        Barrio = x.Barrio,
                        Direccion = x.Direccion,
                        Municipio_Id = x.Municipio_Id,
                        CodigoPostal = x.CodigoPostal
                    });
                });
            }
            return Json(Mensaje<Domain.Entities.Cliente.Cliente>.MensajeJson(Constants.NO_ERROR, responseCliente.Mensaje, Constants.CLIENT_SUCCESS, clienteModel.Cliente));
        }


        [HttpPost]
        [Route("edit")]
        public IHttpActionResult Edit(ClienteModel clienteModel)
        {
            if (clienteModel.Cliente == null)
            {
                return Json(Mensaje<Domain.Entities.Cliente.Cliente>.MensajeJson(Constants.IS_ERROR, "Objecto no puede estar vacio", Constants.CLIENT_FAIL));
            }

            var FactoryCliente = BuilderFactories.Cliente(clienteModel.Cliente.Documento, clienteModel.Cliente.Nombre, clienteModel.Cliente.Email, usuario.id);
            var responseCliente = clienteModel.ServicioCliente.Edit(new ServicioClienteRequest()
            {
                Documento = FactoryCliente.Documento,
                Email = FactoryCliente.Email,
                Nombre = FactoryCliente.Nombre,
                Usuario_Id = usuario.id,
            });

            if(clienteModel.Cliente.Telefónos != null)
            {
                clienteModel.Cliente.Telefónos.ToList().ForEach(x =>
                {
                    clienteModel.ServicioTelefóno.Edit(new ServicioTelefónoRequest
                    {
                        Cliente_Id = responseCliente.Id,
                        Número = x.Número,
                        TipoTelefono = x.TipoTelefono,
                        Id = x.Id
                    });
                });
            }

            
            if (clienteModel.Cliente.Direcciónes != null)
            {
                clienteModel.Cliente.Direcciónes.ToList().ForEach(x =>
                {
                    clienteModel.ServicioDirección.Edit(new ServicioDireccíonRequest
                    {
                        Cliente_Id = responseCliente.Id,
                        Barrio = x.Barrio,
                        Direccion = x.Direccion,
                        Municipio = x.Municipio,
                        CodigoPostal = x.CodigoPostal,
                        Id = x.Id
                    });
                });
            } 
            

            return Json(Mensaje<Domain.Entities.Cliente.Cliente>.MensajeJson(Constants.NO_ERROR, responseCliente.Mensaje, Constants.CLIENT_SUCCESS));
        }

        [HttpGet]
        [Route("get")]
        public IHttpActionResult Get()
        {
            return Json(ClienteModel.Instance.ServicioCliente.Get(new ServicioClienteRequest { Usuario_Id = usuario.id }));
        }

        [HttpGet]
        [Route("get_all")]
        public IHttpActionResult GetAll()
        {
            return Json(ClienteModel.GetAll(usuario.id));
        }


        [HttpPost]
        [Route("pay_create")]
        public IHttpActionResult PayCreate(MetodoPagoModel metodoPago)
        {

            if (metodoPago.ClienteMetodoDePagos == null)
            {
                return Json(Mensaje<Domain.Entities.Cliente.ClienteMetodoDePago>.MensajeJson(Constants.IS_ERROR, "Objecto no puede estar vacio", Constants.PAY_FAIL));
            }
            

            int cont = 0, id = metodoPago.ServicioCliente.Get(new ServicioClienteRequest { Usuario_Id = usuario.id }).Id;
            bool activo = false;
            try
            {
                metodoPago.ClienteMetodoDePagos.ToList().ForEach(x =>
                {
                    if (cont == 0 && x.Saldo > 0)
                    {
                        activo = true;
                    }
                    var response = metodoPago.ServicioMetodoPago.Create(new ServicioMetodoPagoRequest
                    {
                        Activo = activo,
                        Cliente_Id = id,
                        Saldo = x.Saldo,
                        CreditCard = x.CreditCard
                    });

                    if (response.Status) cont++;
                });
            }
            catch (Exception e)
            {
                return Json(Mensaje<Domain.Entities.Cliente.ClienteMetodoDePago>.MensajeJson(Constants.IS_ERROR, e.Message, Constants.PAY_FAIL));
            }

            if (cont == 0)
            {
                return Json(Mensaje<Domain.Entities.Cliente.ClienteMetodoDePago>.MensajeJson(Constants.IS_ERROR, "Error, no se pudo crear ningun metodo de pago", Constants.PAY_FAIL));
            }
            return Json(Mensaje<Domain.Entities.Cliente.ClienteMetodoDePago>.MensajeJson(Constants.NO_ERROR, "Se registraron " + cont + " Metodos de pago", Constants.PAY_SUCCESS));
        }


        [HttpPost]
        [Route("pay_edit")]
        public IHttpActionResult PayEdit(MetodoPagoModel metodoPago)
        {


            if (metodoPago.ClienteMetodoDePagos == null)
            {
                return Json(Mensaje<Domain.Entities.Cliente.ClienteMetodoDePago>.MensajeJson(Constants.IS_ERROR, "Objecto no puede estar vacio", Constants.PAY_FAIL));
            }

            try
            {
                int cont = 0;
                var cliente = ClienteModel.GetAll(usuario.id).Cliente;

                metodoPago.ClienteMetodoDePagos.ToList().ForEach(x =>
                {
                    var response = metodoPago.ServicioMetodoPago.Edit(new ServicioMetodoPagoRequest
                    {
                        Activo = x.Activo,
                        Cliente_Id = cliente.Id,
                        Saldo = x.Saldo,
                        CreditCard = x.CreditCard,
                        Id = x.Id,
                        Cliente = cliente
                    });

                    if (response.Status) { cont++; }
                });
                if (cont == 0)
                {
                    return Json(Mensaje<Domain.Entities.Cliente.ClienteMetodoDePago>.MensajeJson(Constants.IS_ERROR, "Error, No se pudo modificar, ", Constants.PAY_FAIL));
                }
                return Json(Mensaje<Domain.Entities.Cliente.ClienteMetodoDePago>.MensajeJson(Constants.NO_ERROR, "Se Modificaron " + cont + " Metodos de pago", Constants.PAY_SUCCESS));
            }
            catch (Exception e)
            {
                return Json(Mensaje<Domain.Entities.Cliente.ClienteMetodoDePago>.MensajeJson(Constants.NO_ERROR, e.Message, Constants.PAY_FAIL));
            }
        }

        [HttpGet]
        [Route("pay_delete/{id}")]
        public IHttpActionResult PayDelete(int id)
        {

            if (id == 0)
            {
                return Json(Mensaje<Domain.Entities.Cliente.ClienteMetodoDePago>.MensajeJson(Constants.IS_ERROR, "Objecto no puede estar vacio", Constants.PAY_FAIL));
            }

            if (MetodoPagoModel.Instance.Delete(id))
            {
                return Json(Mensaje<Domain.Entities.Cliente.ClienteMetodoDePago>.MensajeJson(Constants.NO_ERROR, "borrado con exito", Constants.PAY_SUCCESS));
            }
            else
            {
                return Json(Mensaje<Domain.Entities.Cliente.ClienteMetodoDePago>.MensajeJson(Constants.NO_ERROR, "borrado fail", Constants.PAY_FAIL));
            }
        }

    }
}
