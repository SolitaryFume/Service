using Proto;
using Service;

namespace MessageHander
{
    public class LoginHnader : MessageHanderBase<LoginRequest>
    {
        public override void Hander(LoginRequest message, IUser user)
        {
            user.SendMessage(new LoginResponse(){
                Code = ResponseCode.OK
            });
        }
    }
}