using Domain.Entities.Producto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.WebApi.Models.ProductoModel
{
    public class CategoriaModel : Model<Categoria>
    {
        public Categoria Categoria { set; get; }

        [JsonIgnore]
        private static CategoriaModel categoriaModel;
        public static CategoriaModel Instance
        {
            get
            {
                if (categoriaModel == null)
                {
                    categoriaModel = new CategoriaModel();
                }
                return categoriaModel;
            }
        }


        public Categoria GetByName(string nombre)
        {
            var categoria = _repository.FindBy(x => x.Nombre == nombre).FirstOrDefault();
            return categoria;
        }

        public Categoria GetById(int id)
        {
            if (id < 1) return null;
            return Find(id);
        }

         



    }

}