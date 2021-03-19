using System;
using Service;
using ProtoMessage;

namespace MessageHander
{
    public interface IMessageHander<T>
    {
        void Hander(T message, IToken user);
    }

    public abstract class MessageHanderBase<T> : IMessageHander<T>
    {
        public abstract void Hander(T message, IToken user);
    }

    public class LoginHnader : MessageHanderBase<LoginRequest>
    {
        public override void Hander(LoginRequest message, IToken user)
        {
            
        }
    }
}