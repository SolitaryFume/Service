using System;
using Service;

namespace MessageHander
{
    public interface IMessageHander
    {
        void Hander(object message,IUser user);
    }

    public interface IMessageHander<T>:IMessageHander where T : class
    {
        void Hander(T message, IUser user);
    }

    public abstract class MessageHanderBase<T> : IMessageHander<T> 
        where T : class
    {
        public abstract void Hander(T message, IUser user);

        public void Hander(object message, IUser user)
        {
            if(message==null)
                return;
            T m = message as T;
            this.Hander(m,user);
        }
    }
}