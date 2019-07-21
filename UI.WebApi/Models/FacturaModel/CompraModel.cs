using Application.Base;
using Application.Implements.Cliente.ServicioCliente;
using Application.Implements.Factura;
using Domain.Entities.Factura;
using Domain.Enum;
using Domain.Factories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UI.WebApi.Models.ClienteModel.ClienteM;
using UI.WebApi.Models.ProductoModel;

namespace UI.WebApi.Models.FacturaModel
{
    public class CompraModel : Model<Compra>
    {
        public Compra Compra { set; get; }


        [JsonIgnore]
        private static CompraModel compraModel;
        public static CompraModel Instance
        {
            get
            {
                if (compraModel == null)
                {
                    compraModel = new CompraModel();
                }
                return compraModel;
            }
        }

        public CompraModel Comprar(Compra compra)
        {
            if(compra == null || compra.CompraClientes == null)
            {
                return null;
            }
            Instance.Compra = compra;
            var response = Instance.ServicioCompra.Create(new ServicioCompraRequest
            {
                Cliente_Id = compra.Cliente_Id,
                FechaCompra = compra.FechaCompra.Year < DateTime.Now.Year ? DateTime.Now : compra.FechaCompra,   
            });

            if (response.Status)
            {
                Instance.Compra.CompraClientes = compra.CompraClientes;
                Instance.Compra.CompraClientes.ToList().ForEach(x =>
                {
                    ServicioCompraCliente.Create(new ServicioCompraClienteRequest
                    {
                        Cantidad = x.Cantidad,
                        Compra_Id = response.Id,
                        Producto_Id = x.Producto_Id,
                        EstadoClienteArticulo = x.EstadoClienteArticulo
                    });
                    x.Producto = ProductoModel.ProductoModel.Instance.Find(x.Producto_Id);
                    x.Producto.ProductoDescuentos = ProductoDescuentoModel.Instance._repository.FindBy(y => y.Producto_Id == x.Producto_Id);
                    x.Producto.ProductoDescuentos.ToList().ForEach(z => {
                        z.Descuento = DescuentoModel.Instance.Find(z.Descuento_Id);
                    });
                    x.Compra_Id = response.Id;
                });
                Instance.Compra.Id = response.Id;
                if (compra.ComprobanteDePagos == null)
                {
                    compra.ComprobanteDePagos = new List<ComprobanteDePago>()
                    {
                        BuilderFactories.ComprobanteDePago(EstadoDePago.EN_ESPERA, Instance.Compra.ObtenerTotal(), Instance.Compra.ObtenerSubTotal(),MedioPago.EFECTIVO,0,DateTime.Now,Instance.Compra.ObtenerDescuento(), response.Id)
                    };
                }
                Instance.Compra.ComprobanteDePagos = compra.ComprobanteDePagos;
                ComprobanteDePagoModel.Instance.Create(Instance.Compra.ComprobanteDePagos.FirstOrDefault());
            }
            return Instance;
        }

        public CompraModel CompletarCompra(int compra_id)
        {
            Instance.Compra = Instance.Find(compra_id);

            if(Instance.Compra != null)
            {
                Instance.Compra.Cliente = ClienteModel.ClienteM.ClienteModel.Instance.Find(Instance.Compra.Cliente_Id);
                Instance.Compra.CompraClientes = CompraClienteModel.Instance._repository.FindBy(x => x.Compra_Id == compra_id);
                Instance.Compra.ComprobanteDePagos = ComprobanteDePagoModel.Instance._repository.FindBy(x => x.Compra_Id == compra_id);
                if (Instance.Compra.CompraClientes != null)
                {
                    Instance.Compra.CompraClientes.ToList().ForEach(x => {
                        x.Producto = ProductoModel.ProductoModel.Instance.Find(x.Producto_Id);
                        x.Producto.ProductoDescuentos = ProductoDescuentoModel.Instance._repository.FindBy(y => y.Producto_Id == x.Producto_Id);
                        if(x.Producto.ProductoDescuentos != null)
                        {
                            x.Producto.ProductoDescuentos.ToList().ForEach(y => {
                                y.Descuento = DescuentoModel.Instance.Find(y.Descuento_Id);
                            });
                        }
                    });
                }

                Instance.Compra.Cliente.ClienteMetodoDePagos = MetodoPagoModel.Instance._repository.FindBy(x => x.Cliente_Id == Instance.Compra.Cliente_Id).ToList();


                if (Instance.Compra.CompletarCompras())
                {
                    
                    ComprobanteDePagoModel.Instance.Update(Instance.Compra.ComprobanteDePagos.FirstOrDefault());
                    Instance.Compra.CompraClientes.ToList().ForEach(x => {
                        CompraClienteModel.Instance.Update(x);
                    });
                    Instance.Compra.Cliente.ClienteMetodoDePagos.ToList().ForEach(x =>
                    {
                        x.Cliente = null;
                        MetodoPagoModel.Instance.Update(x);
                    });
                    //Instance.ServicioCompra.Update(Instance.Compra);
                }
            }
            return Instance;
        }
    }
}