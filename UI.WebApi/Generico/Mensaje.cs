using Domain;
using UI.WebApi.Models;

namespace UI.WebApi.Generico
{
    public static class Mensaje<T> where T : BaseEntity
    {
        public static ResponseStatusModel<T> MensajeJson(bool error, string mensaje, string result, T t = null)
        {
            return (new ResponseStatusModel<T>() { Error = error, Message = mensaje, Result = result, Entity = t });
        }
    }
}