using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ProtoBuf;
using ProtoMessage;
using Core;

namespace Service
{
    public class ProtoService : ServiceBase
    {
        private ProtoService()
        {
            _map = new Dictionary<int, Type>();


        }

        private static Dictionary<int, Type> _map;

        public static Dictionary<int, Type> Map
        {
            get
            {
                if (_map == null)
                {
                    InitMap();
                }
                return _map;
            }
        }

        private static void InitMap()
        {
            _map = new Dictionary<int, Type>();

            foreach (var id in Enum.GetValues<MessageID>())
            {
                var ty =Type.GetType($"ProtoMessage.{id.ToString()}");
                if(ty==null)
                {
                    Debug.Error($"no find message :{id.ToString()}");
                }
                else
                {
                    _map.Add((int)id,ty);
                }
            }
        }


        public static int GetProtoID<T>()
        {
            var t = typeof(T);
            return GetProtoID(t);
        }

        public static int GetProtoID(Type type)
        {
            foreach (var pair in Map)
            {
                if (type == pair.Value)
                {
                    return pair.Key;
                }
            }
            Debug.Error($"no find proto >>> type : {type}");
            return -1;
        }

        public static Type GetProtoType(int id)
        {
            if (Map.TryGetValue(id, out var t))
            {
                return t;
            }
            else
            {
                Debug.Error("no find proto >>> id : {id}");
                return null;
            }
        }

        public static byte[] ProtoToMessage<T>(T proto)
        {
            var data = Serialize(proto);
            var l = data.Length + 8;
            var id =GetProtoID<T>();
            var message = new byte[l];
            Array.Copy(BitConverter.GetBytes(l), 0, message, 0, 4);
            Array.Copy(BitConverter.GetBytes(id), 0, message, 4, 4);
            Array.Copy(data, 0, message, 8, data.Length);
            return message;
        }

        public static object MessageToProto(byte[] message)
        {
            var id = BitConverter.ToInt32(message, 4);
            var protoType = GetProtoType(id);
            var obj = Deserialize(message, protoType, 8, message.Length - 8);
            return obj;
        }

        public static byte[] Serialize<T>(T t)
        {
            var stream = new MemoryStream();
            Serializer.Serialize<T>(stream, t);
            var bytes = stream.ToArray();
            stream.Close();
            stream.Dispose();
            return bytes;
        }

        public static T Deserialize<T>(byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            var t = Serializer.Deserialize<T>(stream);
            stream.Close();
            stream.Dispose();
            return t;
        }

        public static object Deserialize(byte[] bytes, System.Type protoType)
        {
            var stream = new MemoryStream(bytes);
            var proto = Serializer.Deserialize(protoType, stream);
            stream.Close();
            stream.Dispose();
            return proto;
        }

        public static object Deserialize(byte[] bytes, System.Type protoType, int index, int count)
        {
            var stream = new MemoryStream(bytes, index, count);
            var proto = Serializer.Deserialize(protoType, stream);
            stream.Close();
            stream.Dispose();
            return proto;
        }
    }
}