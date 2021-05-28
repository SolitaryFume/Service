using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proto
{

    public class PorotoServer
    {
        private Dictionary<ProtoID, Type> _map;
        public PorotoServer()
        {
            InitMap();
        }

        private void InitMap()
        {
            _map = new Dictionary<ProtoID, Type>();
            var names = Enum.GetNames(typeof(ProtoID));
            foreach (var key in names)
            {
                var ty = Type.GetType($"Proto.{key}");
                if (ty == null)
                {
                    Console.WriteLine($"no find message : Proto.{key}");
                }
                else
                {
                    if(Enum.TryParse<ProtoID>(key,out var value))
                    {
                        _map.Add(value, ty);
                    }
                }
            }
        }

        public ProtoID GetProtoID<T>()
        {
            var t = typeof(T);
            return GetProtoID(t);
        }

        public ProtoID GetProtoID(Type type)
        {
            foreach (var pair in _map)
            {
                if (type == pair.Value)
                {
                    return pair.Key;
                }
            }
            Console.WriteLine($"no find proto >>> type : {type}");
            throw null;
        }

        public Type GetProtoType(ProtoID id)
        {
            if (_map.TryGetValue(id, out var t))
            {
                return t;
            }
            else
            {
                Console.WriteLine("no find proto >>> id : {id}");
                return null;
            }
        }
    }

    public class MessageHanderServer
    {
        public MessageHanderServer()
        { 
            
        }
    }
}
