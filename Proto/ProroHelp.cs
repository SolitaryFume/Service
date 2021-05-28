using System;
using System.Collections.Generic;
using System.IO;
using ProtoBuf;

namespace Proto
{
    public static class ProroHelp
    {
        private static Dictionary<ProtoID, Type> protocols;
        private static Dictionary<Type, ProtoID> keys;

        static ProroHelp()
        {
            var names = Enum.GetNames(typeof(ProtoID));
            protocols = new Dictionary<ProtoID, Type>(names.Length);
            keys = new Dictionary<Type, ProtoID>();
            for (int i = 0; i < names.Length; i++)
            {
                var key = (ProtoID)Enum.Parse(typeof(ProtoID), names[i]);
                var value = Type.GetType($"Proto.{names[i]}");
                if (value == null)
                {
                    
                }
                else
                {
                    protocols.Add(key, value);
                    keys.Add(value,key);
                }   
            }
        }

        public static INetMessage Decoder(byte[] data)
        {
            var id = (ProtoID)BitConverter.ToUInt16(data, 0);
            using (var stream = new MemoryStream(data,2,data.Length-2))
            {
                return Serializer.Deserialize(protocols[id], stream) as INetMessage;
            }
        }

        public static byte[] Encoder(object message)
        {
            var ty = message.GetType();
            var id = keys[ty];
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, message);
                var data = stream.ToArray();
                var list = new List<byte>(data.Length + 2);
                list.AddRange(BitConverter.GetBytes((ushort)id));
                list.AddRange(data);
                return list.ToArray();
            }
        }
    }
}