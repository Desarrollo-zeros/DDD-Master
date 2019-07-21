using Application.Implements.Cliente.ServicioUsuario;
using Domain.Abstracts;
using Domain.Entities.Cliente;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using UI.WebApi.Singleton;
using UI.WebApi.Models.ClienteModel.ClienteM;
using Application.Implements.Cliente.ServicioCliente;
using Domain.Enum;
using UI.WebApi.Generico;

namespace UI.WebApi.Models.ClienteModel.UsuarioM
{
    public class UsuarioModel : Model<Usuario>
    {

        public Usuario Usuario { set; get; }
       
        [JsonIgnore]
        public readonly int id = 0;
        public readonly Rol rol;




        public static UsuarioModel Instance
        {
            get
            {
                if (usuarioModel == null)
                {
                    usuarioModel = new UsuarioModel();
                }
                return usuarioModel;
            }
        }

        [JsonIgnore]
        private static UsuarioModel usuarioModel;

        public UsuarioModel()
        {
            if (Auth().IsAuthenticated)
            {
                var usuario = ServicioUsuario.Get(new ServicioUsuarioRequest { Username = Auth().Name });
                id = usuario.Id;
                rol = usuario.Rol;
            }

            
        }
        public IIdentity Auth()
        {
            return Thread.CurrentPrincipal.Identity;
        }

        public static UsuarioModel Get(int id)
        {
            Instance.Usuario = Instance.ServicioUsuario.Get(new ServicioUsuarioRequest { Id = id });
            Instance.Usuario.Password = "Secret";
            return Instance;
        }
    }
}