using ProtoBuf;

namespace Proto
{
    [ProtoContract]
    public class RegisterAccountResponse: INetMessage
    {
        [ProtoMember(1)]
        public ResponseCode Code { get; set; }
    }
}
