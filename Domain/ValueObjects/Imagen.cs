using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.ValueObjects
{
    public class Imagen
    {
        public Imagen(string imagen)
        {
            if (IsBase64String(imagen))
            {
                this.Img = imagen;
            }
            else
            {
                this.Img = null;
            }
        }

        public Imagen(){}

        [Column("Imagen")]
        public string Img { set; get; }

        public bool IsBase64String(string imagenBase64)
        {
            imagenBase64 = imagenBase64.Trim();
            return (imagenBase64.Length % 4 == 0) && Regex.IsMatch(imagenBase64, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }
    }
}
