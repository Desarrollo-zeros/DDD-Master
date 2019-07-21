using Application.Implements.Producto;
using Domain.Enum;
using Domain.Factories;
using System;
using System.Linq;
using System.Web.Http;
using UI.WebApi.Generico;
using UI.WebApi.Models.ClienteModel.UsuarioM;
using UI.WebApi.Models.ProductoModel;

namespace UI.WebApi.Controllers.Producto
{
    [AllowAnonymous]
    [RoutePrefix("api/Producto")]
    public class ProductoController : ApiController
    {

        [HttpPost]
        [Route("categoria_create")]
        public IHttpActionResult CategoriaCreate(CategoriaModel categoriaModel)
        {

            if (UsuarioModel.Instance.rol != Rol.ADMINISTRADOR && UsuarioModel.Instance.rol != Rol.DEV)
            {
                return Json(Mensaje<Domain.Entities.Producto.Categoria>.MensajeJson(Constants.IS_ERROR, "No esta autorizado para realizar esta operacion", Constants.NO_AUTH));
            }

            if (categoriaModel.Categoria == null)
            {
                return Json(Mensaje<Domain.Entities.Producto.Categoria>.MensajeJson(Constants.IS_ERROR, "Objecto no puede estar vacio", Constants.CATEGORIA_FAIL));
            }

            var categoria = categoriaModel.GetByName(categoriaModel.Categoria.Nombre);
            if (categoria == null)
            {
                var c = categoriaModel.Create(BuilderFactories.Categoria(categoriaModel.Categoria.Nombre, categoriaModel.Categoria.Descripción, (categoriaModel.Categoria.FechaCreacion.Year < DateTime.Now.Year) ? DateTime.Now : categoriaModel.Categoria.FechaCreacion));
                if (c != null)
                {
                    return Json(Mensaje<Domain.Entities.Producto.Categoria>.MensajeJson(Constants.NO_ERROR, "Categoria creada con exito", Constants.CATEGORIA_SUCCES));
                }
                else
                {
                    return Json(Mensaje<Domain.Entities.Producto.Categoria>.MensajeJson(Constants.IS_ERROR, "Error al crear la categoria", Constants.CATEGORIA_FAIL));
                }
            }
            else
            {
                return Json(Mensaje<Domain.Entities.Producto.Categoria>.MensajeJson(Constants.IS_ERROR, "Categoria ya existe", Constants.CATEGORIA_FAIL));
            }
        }


        [HttpPost]
        [Route("categoria_Edit")]
        public IHttpActionResult CategoriaEdit(CategoriaModel categoriaModel)
        {
            if (UsuarioModel.Instance.rol != Rol.ADMINISTRADOR && UsuarioModel.Instance.rol != Rol.DEV)
            {
                return Json(Mensaje<Domain.Entities.Producto.Categoria>.MensajeJson(Constants.IS_ERROR, "No esta autorizado para realizar esta operacion", Constants.NO_AUTH));
            }


            if (categoriaModel.Categoria == null)
            {
                return Json(Mensaje<Domain.Entities.Producto.Categoria>.MensajeJson(Constants.IS_ERROR, "Objecto no puede estar vacio", Constants.CATEGORIA_FAIL));
            }

            if (categoriaModel.ServicioProducto.GetExiste(new ServicioProductoRequest { Id = categoriaModel.Categoria.Id , Nombre = categoriaModel.Categoria.Nombre }))
            {
                return Json(Mensaje<Domain.Entities.Producto.Categoria>.MensajeJson(Constants.IS_ERROR, "Nombre Categoria ya existe", Constants.CATEGORIA_FAIL));
            }

           var categoria = categoriaModel.Find(categoriaModel.Categoria.Id);
            if (categoria != null)
            {
                var factoryCategoria = BuilderFactories.Categoria(categoriaModel.Categoria.Nombre, categoriaModel.Categoria.Descripción, (categoriaModel.Categoria.FechaCreacion.Year < DateTime.Now.Year) ? DateTime.Now : categoriaModel.Categoria.FechaCreacion);
                categoria.Nombre = factoryCategoria.Nombre;
                categoria.Descripción = factoryCategoria.Descripción;
                if (categoriaModel.Update(categoria))
                {
                    return Json(Mensaje<Domain.Entities.Producto.Categoria>.MensajeJson(Constants.NO_ERROR, "Categoria Modificada con exito", Constants.CATEGORIA_SUCCES));
                }
                else
                {
                    return Json(Mensaje<Domain.Entities.Producto.Categoria>.MensajeJson(Constants.IS_ERROR, "Error al modificar la categoria", Constants.CATEGORIA_FAIL));
                }
            }
            else
            {
                return Json(Mensaje<Domain.Entities.Producto.Categoria>.MensajeJson(Constants.IS_ERROR, "Categoria no existe", Constants.CATEGORIA_FAIL));
            }
        }


        [HttpGet]
        [Route("categoria_get/{id}")]
        public IHttpActionResult GetCategoria(int id)
        {
            CategoriaModel.Instance.Categoria = CategoriaModel.Instance.Find(id);
            if (CategoriaModel.Instance.Categoria != null)
            {
                CategoriaModel.Instance.Categoria.Productos = ProductoModel.Instance.ServicioProducto.GetByCategory(id);

                if(CategoriaModel.Instance.Categoria.Productos != null)
                {
                    
                    CategoriaModel.Instance.Categoria.Productos.ToList().ForEach(x =>
                    {
                        x.Categoria = null;
                        x.ProductoDescuentos = ProductoDescuentoModel.Instance._repository.FindBy(y => y.Producto_Id == x.Id).ToList();
                        if(x.ProductoDescuentos != null)
                        {
                            x.ProductoDescuentos.ToList().ForEach(y =>
                            {
                                y.Descuento = DescuentoModel.Instance._repository.FindBy(t => t.Id == y.Descuento_Id).FirstOrDefault();
                                if(y.Descuento != null)
                                {
                                    x.Descuento += y.Descuento.Descu;
                                }
                            });
                        } 
                    });
                }
                
            }
            return Json(CategoriaModel.Instance);
        }

        [HttpGet]
        [Route("categoria_get_all")]
        public IHttpActionResult GetAllCategoria()
        {

            var categorias = CategoriaModel.Instance.GetAll().ToList();
            if (categorias != null)
            {
                categorias.ForEach(x =>
                {
                    x.Productos = ProductoModel.Instance._repository.FindBy(y => y.Categoria_Id == x.Id).ToList();
                    if(x.Productos != null)
                    {
                        x.Productos.ToList().ForEach(y => {
                            y.Categoria = null;
                            y.ProductoDescuentos = ProductoDescuentoModel.Instance._repository.FindBy(z => z.Producto_Id == y.Id).ToList();

                            if(y.ProductoDescuentos != null)
                            {
                                y.ProductoDescuentos.ToList().ForEach(r =>
                                {
                                    r.Descuento = DescuentoModel.Instance._repository.FindBy(t => t.Id == r.Descuento_Id).FirstOrDefault();
                                    if(r.Descuento != null)
                                    {
                                        y.Descuento += r.Descuento.Descu;
                                    }
                                });
                            }
                            
                        });
                    }
                   
                });
            }
            return Json(categorias);
        }

        [HttpGet]
        [Route("get_top_product")]
        public IHttpActionResult GetToProduct()
        {
            var productos = ProductoModel.Instance.ProductosTop(10).ToList();
            if (productos != null)
            {
                productos.ForEach(x =>
                {
                    x.Categoria = CategoriaModel.Instance.Find(x.Categoria_Id);
                    x.Categoria.Productos = null;
                    if (x.Categoria != null)
                    {
                        x.ProductoDescuentos = ProductoDescuentoModel.Instance._repository.FindBy(y => x.Id == y.Producto_Id);
                        if (x.ProductoDescuentos != null)
                        {
                            x.ProductoDescuentos.ToList().ForEach(y =>
                            {
                                y.Descuento = DescuentoModel.Instance._repository.FindBy(z => z.Id == y.Descuento_Id).FirstOrDefault();
                                if (y.Descuento != null)
                                {
                                    x.Descuento += y.Descuento.Descu;
                                }
                            });
                        }

                    }

                });
            }
            return Json(productos);
        }


        /*******************************/

        [HttpGet]
        [Route("categoria_new_product")]
        public IHttpActionResult GetNewAllCategoriaProducto()
        {
            var categorias = CategoriaModel.Instance.GetAll().ToList();
            if (categorias != null)
            {

                categorias.ForEach(x =>
                {
                    x.Productos = ProductoModel.Instance._repository.FindBy(y => y.Categoria_Id == x.Id).OrderByDescending(z => z.FechaCreacion).ThenByDescending(z => z.FechaCreacion).Take(10);
                    
                    if (x.Productos != null)
                    {
                        x.Productos.ToList().ForEach(y =>
                        {
                            y.Categoria = null;
                            y.ProductoDescuentos = ProductoDescuentoModel.Instance._repository.FindBy(z => z.Producto_Id == y.Id).ToList();

                            if (y.ProductoDescuentos != null)
                            {
                                y.ProductoDescuentos.ToList().ForEach(r =>
                                {
                                    r.Descuento = DescuentoModel.Instance._repository.FindBy(t => t.Id == r.Descuento_Id).FirstOrDefault();
                                    if (r.Descuento != null)
                                    {
                                        y.Descuento += r.Descuento.Descu;
                                    }
                                });
                            }
                        });
                    }
                });
            }
            return Json(categorias);
        }



        [HttpPost]
        [Route("producto_create")]
        public IHttpActionResult ProductoCreate(ProductoModel productoModel)
        {
            try
            {
                if (UsuarioModel.Instance.rol != Rol.ADMINISTRADOR && UsuarioModel.Instance.rol != Rol.DEV)
                {
                    return Json(Mensaje<Domain.Entities.Producto.Producto>.MensajeJson(Constants.IS_ERROR, "No esta autorizado para realizar esta operacion", Constants.NO_AUTH));
                }

                if (productoModel.Producto == null)
                {
                    return Json(Mensaje<Domain.Entities.Producto.Producto>.MensajeJson(Constants.IS_ERROR, "Objecto no puede estar vacio", Constants.PRODUCTO_FAIL));
                }

                if (productoModel._repository.FindBy(x => x.PrecioVenta == productoModel.Producto.PrecioVenta && x.Nombre == productoModel.Producto.Nombre).Count() > 0)
                {
                    return Json(Mensaje<Domain.Entities.Producto.Producto>.MensajeJson(Constants.IS_ERROR, "Producto ya existe", Constants.PRODUCTO_FAIL));
                }

                var producto = productoModel.Create(BuilderFactories.Producto(productoModel.Producto.Nombre, productoModel.Producto.Descripción, productoModel.Producto.Imagen, productoModel.Producto.PrecioCompra, productoModel.Producto.PrecioVenta, productoModel.Producto.CantidadProducto, productoModel.Producto.Categoria_Id));
                if (producto == null)
                {
                    return Json(Mensaje<Domain.Entities.Producto.Producto>.MensajeJson(Constants.IS_ERROR, "Producto no pudo crearse", Constants.PRODUCTO_FAIL));
                }
                return Json(Mensaje<Domain.Entities.Producto.Producto>.MensajeJson(Constants.NO_ERROR, "Producto Creado con exito", Constants.PRODUCTO_SUCCES));
            }
            catch (Exception e)
            {
                return Json(Mensaje<Domain.Entities.Producto.Producto>.MensajeJson(Constants.IS_ERROR, e.Message, Constants.PRODUCTO_FAIL));
            }
        }


        [HttpPost]
        [Route("producto_edit")]
        public IHttpActionResult ProductoEdit(ProductoModel productoModel)
        {
            try
            {
                if (UsuarioModel.Instance.rol != Rol.ADMINISTRADOR && UsuarioModel.Instance.rol != Rol.DEV)
                {
                    return Json(Mensaje<Domain.Entities.Producto.Producto>.MensajeJson(Constants.IS_ERROR, "No esta autorizado para realizar esta operacion", Constants.NO_AUTH));
                }

                if (productoModel.Producto == null)
                {
                    return Json(Mensaje<Domain.Entities.Producto.Producto>.MensajeJson(Constants.IS_ERROR, "Objecto no puede estar vacio", Constants.PRODUCTO_FAIL));
                }

                if (productoModel.Producto.PrecioCompra > productoModel.Producto.PrecioVenta)
                {
                    return Json(Mensaje<Domain.Entities.Producto.Producto>.MensajeJson(Constants.IS_ERROR, "Precio de compra no puede mayor al precio de venta", Constants.PRODUCTO_FAIL));
                }

                var producto = productoModel._repository.FindBy(x => x.Id == productoModel.Producto.Id).FirstOrDefault();

                if (producto == null)
                {
                    return Json(Mensaje<Domain.Entities.Producto.Producto>.MensajeJson(Constants.IS_ERROR, "Producto no existe", Constants.PRODUCTO_FAIL));
                }


                var factoryProducto = BuilderFactories.Producto(productoModel.Producto.Nombre, productoModel.Producto.Descripción, productoModel.Producto.Imagen, productoModel.Producto.PrecioCompra, productoModel.Producto.PrecioVenta, productoModel.Producto.CantidadProducto, productoModel.Producto.Categoria_Id);

                producto.Nombre = factoryProducto.Nombre;
                producto.Descripción = factoryProducto.Descripción;
                producto.Imagen = factoryProducto.Imagen;
                producto.PrecioCompra = factoryProducto.PrecioCompra;
                producto.PrecioVenta = factoryProducto.PrecioVenta;
                producto.CantidadProducto = factoryProducto.CantidadProducto;
                producto.Categoria_Id = factoryProducto.Categoria_Id;

                if (!productoModel.Update(producto))
                {
                    return Json(Mensaje<Domain.Entities.Producto.Producto>.MensajeJson(Constants.IS_ERROR, "Producto no pudo Modificarse", Constants.PRODUCTO_FAIL));
                }
                return Json(Mensaje<Domain.Entities.Producto.Producto>.MensajeJson(Constants.NO_ERROR, "Producto Modificado con exito", Constants.PRODUCTO_SUCCES));
            }
            catch (Exception e)
            {
                return Json(Mensaje<Domain.Entities.Producto.Producto>.MensajeJson(Constants.IS_ERROR, e.Message, Constants.PRODUCTO_FAIL));
            }
        }

        [HttpGet]
        [Route("producto_get/{id}")]
        public IHttpActionResult GetProducto(int id)
        {
            ProductoModel.Instance.Producto = ProductoModel.Instance.Find(id);

            if (ProductoModel.Instance.Producto != null)
            {
                ProductoModel.Instance.Producto.Categoria = CategoriaModel.Instance.Find(ProductoModel.Instance.Producto.Categoria_Id);
                ProductoModel.Instance.Producto.Categoria.Productos = null;
                ProductoModel.Instance.Producto.ProductoDescuentos = ProductoDescuentoModel.Instance._repository.FindBy(x => x.Producto_Id == id);
                if(ProductoModel.Instance.Producto.ProductoDescuentos != null)
                {
                    ProductoModel.Instance.Producto.ProductoDescuentos.ToList().ForEach(x =>
                    {
                        x.Descuento = DescuentoModel.Instance._repository.FindBy(y => y.Id == x.Descuento_Id).FirstOrDefault();
                        if (x.Descuento != null)
                        {
                            ProductoModel.Instance.Producto.Descuento += x.Descuento.Descu;
                        }
                    });
                }
                
            }
            return Json(ProductoModel.Instance);
        }

        [HttpGet]
        [Route("producto_get_all")]
        public IHttpActionResult GetAllProducto()
        {
            var productos = ProductoModel.Instance.GetAll().ToList();

            if (productos != null)
            {
                productos.ForEach(x =>
                {
                    x.Categoria = CategoriaModel.Instance.Find(x.Categoria_Id);
                    if(x.Categoria != null)
                    {
                        x.Categoria.Productos = null;
                        x.ProductoDescuentos = ProductoDescuentoModel.Instance._repository.FindBy(y => x.Id == y.Producto_Id);
                        if(x.ProductoDescuentos != null)
                        {
                            x.ProductoDescuentos.ToList().ForEach(y =>
                            {
                                y.Descuento = DescuentoModel.Instance._repository.FindBy(z => z.Id == y.Descuento_Id).FirstOrDefault();
                                if(y.Descuento != null)
                                {
                                    x.Descuento += y.Descuento.Descu;
                                }
                            });
                        }
                           
                    }
                    
                });
            }
            return Json(productos);
        }

        [HttpPost]
        [Route("descuento_create")]
        public IHttpActionResult DescuentoCreate(DescuentoModel descuentoModel)
        {
            try
            {
                if (UsuarioModel.Instance.rol != Rol.ADMINISTRADOR && UsuarioModel.Instance.rol != Rol.DEV)
                {
                    return Json(Mensaje<Domain.Entities.Producto.Descuento>.MensajeJson(Constants.IS_ERROR, "No esta autorizado para realizar esta operacion", Constants.NO_AUTH));
                }

                if (descuentoModel.Descuento == null)
                {
                    return Json(Mensaje<Domain.Entities.Producto.Descuento>.MensajeJson(Constants.IS_ERROR, "Objecto no puede estar vacio", Constants.DESCUENTO_FAIL));
                }

                var descuento = descuentoModel.Create(BuilderFactories.Descuento(descuentoModel.Descuento.TipoDescuento, descuentoModel.Descuento.Acomulable, descuentoModel.Descuento.FechaYHoraInicio, descuentoModel.Descuento.FechaYHoraTerminación, descuentoModel.Descuento.Descu));
                if (descuento == null)
                {
                    return Json(Mensaje<Domain.Entities.Producto.Descuento>.MensajeJson(Constants.IS_ERROR, "Descuento no pudo crearse", Constants.DESCUENTO_FAIL));
                }
                return Json(Mensaje<Domain.Entities.Producto.Descuento>.MensajeJson(Constants.NO_ERROR, "Descuento Creado con exito", Constants.DESCUENTO_SUCCES));
            }
            catch (Exception e)
            {
                return Json(Mensaje<Domain.Entities.Producto.Descuento>.MensajeJson(Constants.IS_ERROR, e.Message, Constants.DESCUENTO_FAIL));
            }
        }


        [HttpPost]
        [Route("descuento_edit")]
        public IHttpActionResult DescuentoEdit(DescuentoModel descuentoModel)
        {
            try
            {
                if (UsuarioModel.Instance.rol != Rol.ADMINISTRADOR && UsuarioModel.Instance.rol != Rol.DEV)
                {
                    return Json(Mensaje<Domain.Entities.Producto.Descuento>.MensajeJson(Constants.IS_ERROR, "No esta autorizado para realizar esta operacion", Constants.NO_AUTH));
                }

                if (descuentoModel.Descuento == null)
                {
                    return Json(Mensaje<Domain.Entities.Producto.Descuento>.MensajeJson(Constants.IS_ERROR, "Objecto no puede estar vacio", Constants.DESCUENTO_FAIL));
                }

                var descuento = descuentoModel.Find(descuentoModel.Descuento.Id);


                if (descuento == null)
                {
                    return Json(Mensaje<Domain.Entities.Producto.Descuento>.MensajeJson(Constants.IS_ERROR, "No existe Descuento", Constants.DESCUENTO_FAIL));
                }

                //si esta mal arroja una excepcion
                var factoryDescuento = BuilderFactories.Descuento(descuentoModel.Descuento.TipoDescuento, descuentoModel.Descuento.Acomulable, descuentoModel.Descuento.FechaYHoraInicio, descuentoModel.Descuento.FechaYHoraTerminación, descuentoModel.Descuento.Descu);

                descuento.Descu = factoryDescuento.Descu;
                descuento.TipoDescuento = factoryDescuento.TipoDescuento;
                descuento.Acomulable = factoryDescuento.Acomulable;
                descuento.FechaYHoraInicio = factoryDescuento.FechaYHoraInicio;
                descuento.FechaYHoraTerminación = factoryDescuento.FechaYHoraTerminación;


                if (!descuentoModel.Update(descuento))
                {
                    return Json(Mensaje<Domain.Entities.Producto.Descuento>.MensajeJson(Constants.IS_ERROR, "Descuento no pudo Modificarse", Constants.DESCUENTO_FAIL));
                }
                return Json(Mensaje<Domain.Entities.Producto.Descuento>.MensajeJson(Constants.NO_ERROR, "Descuento Modificado con exito", Constants.DESCUENTO_SUCCES));
            }
            catch (Exception e)
            {
                return Json(Mensaje<Domain.Entities.Producto.Descuento>.MensajeJson(Constants.IS_ERROR, e.Message, Constants.DESCUENTO_FAIL));
            }
        }

        [HttpGet]
        [Route("descuento_delete/{id}")]
        public IHttpActionResult DeleteDescuento(int id)
        {
            if (UsuarioModel.Instance.rol != Rol.ADMINISTRADOR && UsuarioModel.Instance.rol != Rol.DEV)
            {
                return Json(Mensaje<Domain.Entities.Producto.Descuento>.MensajeJson(Constants.IS_ERROR, "No esta autorizado para realizar esta operacion", Constants.NO_AUTH));
            }

            if (DescuentoModel.Instance.Delete(id))
            {
                return Json(Mensaje<Domain.Entities.Producto.Descuento>.MensajeJson(Constants.NO_ERROR, "Descuento eliminado", Constants.DESCUENTO_SUCCES));
            }

            return Json(Mensaje<Domain.Entities.Producto.Descuento>.MensajeJson(Constants.IS_ERROR, "Descuento no pudo ser eliminado", Constants.DESCUENTO_FAIL));
        }

        [HttpGet]
        [Route("descuento_get/{id}")]
        public IHttpActionResult GetDescuento(int id)
        {
            DescuentoModel.Instance.Descuento = DescuentoModel.Instance.Find(id);
            if (DescuentoModel.Instance.Descuento != null)
            {
                DescuentoModel.Instance.Descuento.ProductoDescuentos = ProductoDescuentoModel.Instance._repository.FindBy(x => x.Descuento_Id == id).ToList();

            }
            return Json(DescuentoModel.Instance);
        }

        [HttpGet]
        [Route("descuento_get_all")]
        public IHttpActionResult GetAllDescuento()
        {
            var descuentos = DescuentoModel.Instance.GetAll().ToList();

            if(descuentos != null)
            {
                descuentos.ForEach(x =>
                {
                    x.ProductoDescuentos = ProductoDescuentoModel.Instance._repository.FindBy(y => y.Descuento_Id == x.Id);
                });

            }
            return Json(DescuentoModel.Instance);
        }


        /*************************************/

        [HttpPost]
        [Route("producto_descuento_create")]
        public IHttpActionResult ProductoDescuentoCreate(ProductoDescuentoModel productoDescuentoModel)
        {

            try
            {
                if (UsuarioModel.Instance.rol != Rol.ADMINISTRADOR && UsuarioModel.Instance.rol != Rol.DEV)
                {
                    return Json(Mensaje<Domain.Entities.Producto.ProductoDescuento>.MensajeJson(Constants.IS_ERROR, "No esta autorizado para realizar esta operacion", Constants.NO_AUTH));
                }

                if (productoDescuentoModel.ProductoDescuento == null)
                {
                    return Json(Mensaje<Domain.Entities.Producto.ProductoDescuento>.MensajeJson(Constants.IS_ERROR, "Objecto no puede estar vacio", Constants.PRODUCTO_DESCUENTO_FAIL));
                }

                var ProductoDescuento = productoDescuentoModel._repository.FindBy(x => x.Producto_Id == productoDescuentoModel.ProductoDescuento.Producto_Id && x.Descuento_Id == productoDescuentoModel.ProductoDescuento.Descuento_Id);
                if (ProductoDescuento != null)
                {
                    return Json(Mensaje<Domain.Entities.Producto.ProductoDescuento>.MensajeJson(Constants.IS_ERROR, "Producto Descuento ya existe", Constants.PRODUCTO_DESCUENTO_FAIL));
                }

                var pd = productoDescuentoModel.Create(BuilderFactories.ProductoDescuento(productoDescuentoModel.ProductoDescuento.Producto_Id, productoDescuentoModel.ProductoDescuento.Descuento_Id, productoDescuentoModel.ProductoDescuento.EstadoDescuento));
                if (pd == null)
                {
                    return Json(Mensaje<Domain.Entities.Producto.ProductoDescuento>.MensajeJson(Constants.IS_ERROR, "Producto Descuento no pudo crearse", Constants.PRODUCTO_DESCUENTO_FAIL));
                }
                return Json(Mensaje<Domain.Entities.Producto.ProductoDescuento>.MensajeJson(Constants.NO_ERROR, "Producto Descuento creado con exito", Constants.PRODUCTO_DESCUENTO_SUCCES));

            }
            catch (Exception e)
            {
                return Json(Mensaje<Domain.Entities.Producto.ProductoDescuento>.MensajeJson(Constants.IS_ERROR, e.Message, Constants.PRODUCTO_DESCUENTO_FAIL));
            }

        }


        [HttpPost]
        [Route("producto_descuento_edit")]
        public IHttpActionResult ProductoDescuentoEdit(ProductoDescuentoModel productoDescuentoModel)
        {

            try
            {
                if (UsuarioModel.Instance.rol != Rol.ADMINISTRADOR && UsuarioModel.Instance.rol != Rol.DEV)
                {
                    return Json(Mensaje<Domain.Entities.Producto.ProductoDescuento>.MensajeJson(Constants.IS_ERROR, "No esta autorizado para realizar esta operacion", Constants.NO_AUTH));
                }

                if (productoDescuentoModel.ProductoDescuento == null)
                {
                    return Json(Mensaje<Domain.Entities.Producto.ProductoDescuento>.MensajeJson(Constants.IS_ERROR, "Objecto no puede estar vacio", Constants.PRODUCTO_DESCUENTO_FAIL));
                }

                var ProductoDescuento = productoDescuentoModel._repository.FindBy(x => x.Producto_Id == productoDescuentoModel.ProductoDescuento.Producto_Id && x.Descuento_Id == productoDescuentoModel.ProductoDescuento.Descuento_Id).FirstOrDefault();
                if (ProductoDescuento == null)
                {
                    return Json(Mensaje<Domain.Entities.Producto.ProductoDescuento>.MensajeJson(Constants.IS_ERROR, "Producto Descuento no existe", Constants.PRODUCTO_DESCUENTO_FAIL));
                }

                var pd = BuilderFactories.ProductoDescuento(productoDescuentoModel.ProductoDescuento.Producto_Id, productoDescuentoModel.ProductoDescuento.Descuento_Id, productoDescuentoModel.ProductoDescuento.EstadoDescuento);

                ProductoDescuento.Descuento_Id = pd.Descuento_Id;
                ProductoDescuento.Producto_Id = pd.Producto_Id;
                ProductoDescuento.EstadoDescuento = pd.EstadoDescuento;

                productoDescuentoModel.Update(ProductoDescuento);
                if (pd == null)
                {
                    return Json(Mensaje<Domain.Entities.Producto.ProductoDescuento>.MensajeJson(Constants.IS_ERROR, "Producto Descuento no pudo crearse", Constants.PRODUCTO_DESCUENTO_FAIL));
                }
                return Json(Mensaje<Domain.Entities.Producto.ProductoDescuento>.MensajeJson(Constants.NO_ERROR, "Producto Descuento creado con exito", Constants.PRODUCTO_DESCUENTO_SUCCES));

            }
            catch (Exception e)
            {
                return Json(Mensaje<Domain.Entities.Producto.ProductoDescuento>.MensajeJson(Constants.IS_ERROR, e.Message, Constants.PRODUCTO_DESCUENTO_FAIL));
            }

        }

        [HttpGet]
        [Route("producto_descuento_delete/{id}")]
        public IHttpActionResult DeleteProductoDescuento(int id)
        {
            if (UsuarioModel.Instance.rol != Rol.ADMINISTRADOR && UsuarioModel.Instance.rol != Rol.DEV)
            {
                return Json(Mensaje<Domain.Entities.Producto.ProductoDescuento>.MensajeJson(Constants.IS_ERROR, "No esta autorizado para realizar esta operacion", Constants.NO_AUTH));
            }

            if (ProductoDescuentoModel.Instance.Delete(id))
            {
                return Json(Mensaje<Domain.Entities.Producto.ProductoDescuento>.MensajeJson(Constants.NO_ERROR, "Producto Descuento eliminado", Constants.PRODUCTO_DESCUENTO_SUCCES));
            }

            return Json(Mensaje<Domain.Entities.Producto.ProductoDescuento>.MensajeJson(Constants.IS_ERROR, "Producto Descuento no pudo ser eliminado", Constants.PRODUCTO_DESCUENTO_FAIL));
        }


        [HttpGet]
        [Route("producto_descuento_get/{id}")]
        public IHttpActionResult GetProductoDescuento(int id)
        {
            ProductoDescuentoModel.Instance.ProductoDescuento = ProductoDescuentoModel.Instance.Find(id);
            if (ProductoDescuentoModel.Instance.ProductoDescuento != null)
            {
                ProductoDescuentoModel.Instance.ProductoDescuento.Descuento = DescuentoModel.Instance._repository.FindBy(x => x.Id == ProductoDescuentoModel.Instance.ProductoDescuento.Descuento_Id).FirstOrDefault();
                ProductoDescuentoModel.Instance.ProductoDescuento.Producto = ProductoModel.Instance._repository.FindBy(x=>x.Id == ProductoDescuentoModel.Instance.ProductoDescuento.Producto_Id).FirstOrDefault();
            }
            return Json(ProductoDescuentoModel.Instance);
        }

        [HttpGet]
        [Route("producto_descuento_get_all")]
        public IHttpActionResult GetAllProductoDescuento()
        {
            var productoDescuentos = ProductoDescuentoModel.Instance.GetAll().ToList();

            if(productoDescuentos != null)
            {
                productoDescuentos.ForEach(x =>
                {
                    x.Descuento = DescuentoModel.Instance._repository.FindBy(y => y.Id == x.Descuento_Id).FirstOrDefault();
                    x.Producto = ProductoModel.Instance._repository.FindBy(y => y.Id == x.Producto_Id).FirstOrDefault();
                });
            }
            return Json(ProductoDescuentoModel.Instance);
        }

    }
}
