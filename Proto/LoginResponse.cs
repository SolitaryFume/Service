using ProtoBuf;

namespace Proto
{
    [ProtoContract]
    public class LoginResponse: INetMessage
    {
        [ProtoMember(1)]
        public ResponseCode Code { get; set; }
    }
}
