using Service;
using ProtoMessage;

namespace MessageHander
{
    public class LoginHnader : MessageHanderBase<LoginRequest>
    {
        public override void Hander(LoginRequest message, IUser user)
        {
            user.SendMessage(new LoginResponse(){
                Code = ErrorCode.OK
            });
        }
    }

    public class SingUpHander : MessageHanderBase<SingUpRequest>
    {
        public override void Hander(SingUpRequest message, IUser user)
        {
            
        }
    }
}