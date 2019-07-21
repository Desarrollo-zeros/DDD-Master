using Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Factories
{
    

    public class BaseFactory<T>
    {
        private BaseFactory() { }

        static readonly Dictionary<int, Type> _dict = new Dictionary<int, Type>();

        public static T Create(int id, params object[] args)
        {
            Type type = null;
            if (_dict.TryGetValue(id, out type))
                return (T)Activator.CreateInstance(type, args);

            throw new ArgumentException("No type registered for this id");
        }

        public static void Register<Tderived>(int id) where Tderived : T
        {
            var type = typeof(Tderived);

            if (type.IsInterface || type.IsAbstract)
                throw new ArgumentException("...");

            _dict.Add(id, type);
        }
    }
}
