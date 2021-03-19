using System.IO;
using ProtoBuf;

namespace Service
{
    public static class ProtoHelper
    {
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
    }
}
