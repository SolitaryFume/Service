using ProtoBuf;

namespace Proto
{
    [ProtoContract]
    public class RegisterAccountRequest: INetMessage
    {
        [ProtoMember(1)]
        public string Account { get; set; }
        [ProtoMember(2)]
        public string PassWorld { get; set; }
    }
}
