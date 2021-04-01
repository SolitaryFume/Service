using ProtoBuf;

namespace ProtoMessage
{

    [ProtoContract]
    public class LoginRequest
    {
        [ProtoMember(1)]
        public string UserName { get; set; }
        [ProtoMember(2)]
        public string Password { get; set; }
    }

    [ProtoContract]
    public class LoginResponse
    {
        [ProtoMember(1)]
        public ErrorCode Code { get; set; }
    }

    public enum ErrorCode
    {
        OK = 0,
        Error = 1,

        NoRegisterUser,
        PassworldError,
        NameRepetition
    }

    public class SingUpRequest
    {
        [ProtoMember(1)]
        public string UserName { get; set; }
        [ProtoMember(2)]
        public string Password { get; set; }
    }

    public class SingUpResponse
    {
        [ProtoMember(1)]
        public ErrorCode Code { get; set; }
    }
}