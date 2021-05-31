using System;
//using Service;
using Core.Client;

namespace MessageHander
{
    public interface IMessageHander
    {
        void Hander(object message,IClient user);
    }

    public interface IMessageHander<T> : IMessageHander where T : class
    {
        void Hander(T message, IClient user);
    }

    public abstract class MessageHanderBase<T> : IMessageHander<T>
        where T : class
    {
        public abstract void Hander(T message, IClient user);

        public void Hander(object message, IClient user)
        {
            Hander(message as T, user);
        }
    }
}