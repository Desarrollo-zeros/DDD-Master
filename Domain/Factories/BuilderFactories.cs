using Domain.Base;
using Domain.Entities.Cliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.ValueObjects;
using Domain.Enum;
using Domain.Entities.Producto;
using Domain.Entities.Factura;

namespace Domain.Factories
{
    public class BuilderFactories
    {

        
        private BuilderFactories() { }

        public static Usuario Usuario(string username, string password, bool active, Enum.Rol rol)
        {
            if(username == "" || password == "")
            {
                throw new Exception("Factories Usuario no puede ser creado");
            }
            return new Usuario(username, password, active, rol);
        }

        public static Cliente Cliente(string documento, Nombre nombre, string Email, int Usuario_Id)
        {
            if(documento == null|| nombre == null|| Email == ""|| Usuario_Id < 1){
                throw new Exception("Factories Cliente no puede ser creado");
            }
            
            return new Cliente(documento, nombre, Email, Usuario_Id);
        }

        public static Telefóno Telefóno(string número, TipoTelefono tipoTelefono, int cliente_id)
        {
            if (número == null || tipoTelefono == TipoTelefono.DESCONOCIDO || cliente_id < 1)
            {
                throw new Exception("Factories Cliente no puede ser creado");
            }
            return new Telefóno(número, tipoTelefono, cliente_id);
        }

       

        public static Dirección Dirección(string barrio, string direccion, string codigoPostal, int municipio_Id, int cliente_Id)
        {
            if(string.Empty == barrio || string.Empty == direccion || string.Empty == codigoPostal || municipio_Id < 1 || cliente_Id < 1)
            {
                throw new Exception("Factories Dirección no puede ser creado");
            }
            return new Dirección(barrio, direccion, codigoPostal, municipio_Id, cliente_Id);
        }

        public static ClienteMetodoDePago ClienteMetodoDePago(int cliente_Id, bool activo, double saldo, CreditCardType cardType, string cardNumber, string securityNumber, string ownerName, DateTime expiration)
        {
            if (cliente_Id < 1  || saldo < 1 || cardType == CreditCardType.Unknown || cardNumber == null || null == securityNumber || ownerName == null || expiration == null)
            {
                throw new Exception("Factories ClienteMetodoDePago no puede ser creado");
            }
           
            if (!CreditCard.VerificarTarjeta(cardNumber))
            {
                throw new Exception("Factories ClienteMetodoDePago no puede ser creado, Tarjeta invalidad");
            }

            if(expiration < DateTime.Now)
            {
                throw new Exception("Factories ClienteMetodoDePago no puede ser creado, tarjeta vencida");
            }
            return new ClienteMetodoDePago(cliente_Id, activo, new CreditCard(cardType, cardNumber, securityNumber, ownerName, expiration), saldo);
        }

        public static Categoria Categoria(string nombre, string descripción, DateTime fecha)
        {
            if(nombre == "" || descripción == "")
            {
                throw new Exception("Factories Categoria no puede ser creado");
            }

            if(fecha == null)
            {
                fecha = new DateTime();
            }

            return new Categoria(nombre, descripción, fecha);
        }

        public static Producto Producto(string nombre, string descripción, string imagen, double precioCompra, double precioVenta, int cantidadProducto, int categoria_Id)
        {
            if(string.Empty == nombre || string.Empty == descripción || precioCompra < 1 ||  precioVenta < 1  || cantidadProducto < 1 ||  categoria_Id < 1 || precioCompra > precioVenta)
            {
                throw new Exception("Factories Producto no puede ser creado");
            }

            
            return new Producto(nombre,descripción,imagen,precioCompra,precioVenta,cantidadProducto,categoria_Id);
        }

        public static Descuento Descuento(TipoDescuento tipoDescuento, bool acomulable, DateTime fechaYHoraInicio, DateTime fechaYHoraTerminación, double descuento)
        {
            if (tipoDescuento == TipoDescuento.DESCONOCIDO || fechaYHoraInicio == null || fechaYHoraTerminación == null || descuento == 0)
            {
                throw new Exception("Factories Descuento no puede ser creado");
            }
            return new Descuento(tipoDescuento, acomulable, fechaYHoraInicio, fechaYHoraTerminación, descuento);
        }

        public static ProductoDescuento ProductoDescuento(int producto_Id, int descuento_Id, EstadoDescuento estadoDescuento)
        {
            if(producto_Id < 1 || descuento_Id < 1 || estadoDescuento == EstadoDescuento.DESCONOCIDO)
            {
                throw new Exception("Factories ProductoDescuento no puede ser creado");
            }
            return new ProductoDescuento(producto_Id,descuento_Id, estadoDescuento);
        }


        public static Compra Compra(int cliente_id, DateTime fecha)
        {
            if(cliente_id < 1 || fecha == null)
            {
                throw new Exception("Factories Compra no puede ser creado");
            }
            return new Compra(cliente_id, fecha);
        }

        public static CompraCliente CompraCliente(int producto_Id, int compra_Id, int cantidad, Enum.EstadoClienteArticulo estadoProductoCliente)
        {
            if(producto_Id < 1 || compra_Id < 1 || cantidad < 1)
            {
                throw new Exception("Factories CompraCliente Compra no puede ser creado");
            }
            return new CompraCliente(producto_Id, compra_Id, cantidad, estadoProductoCliente);
        }

        public static ComprobanteDePago ComprobanteDePago(EstadoDePago estadoDePago, double total, double subTotal, MedioPago medioPago, double monto, DateTime fechaDePago, double totalDescuentoAplicados, int compra_id)
        {
            if(total < 1 || subTotal < 1 || compra_id < 1)
            {
                throw new Exception("Factories ComprobanteDePago Compra no puede ser creado");
            }
            return new ComprobanteDePago(estadoDePago, total,subTotal,medioPago,monto,fechaDePago,totalDescuentoAplicados,compra_id);
        }

    }

}
