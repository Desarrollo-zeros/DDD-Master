using Domain;

namespace UI.WebApi.Models
{
    public class ResponseStatusModel<T> where T : BaseEntity
    {
        public string Result { set; get; }
        public bool Error { set; get; }
        public string Message { set; get; }

        public T Entity { set; get; }
    }
}