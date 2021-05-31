using Proto;
using Service;
using Core.Client;
using Core;

namespace MessageHander
{
    public class LoginHnader : MessageHanderBase<LoginRequest>
    {
        public override void Hander(LoginRequest message, IClient user)
        {
            Log.Print("Run LoginHnader");
            user.NetSession.SendMessage(new LoginResponse(){
                Code = ResponseCode.OK
            });
        }
    }
}