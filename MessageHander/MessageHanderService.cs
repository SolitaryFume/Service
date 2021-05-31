using System;
using System.Collections.Generic;
using System.Linq;
using MessageHander;
using Core;
using Proto;
using Core.Client;

namespace Service
{
    public class MessageHanderService
    {
        private Dictionary<Type, IMessageHander> _handerDictionary;
        public MessageHanderService()
        {
            _handerDictionary = new Dictionary<Type, IMessageHander>();
            InitRegister();
            Log.Print("消息处理模块启动！");
        }

        private void InitRegister()
        {
            RegisterHander<LoginRequest>(new LoginHnader());
        }

        private void TryAddHanderType(Type type)
        {
            if (CheckedHnader(type))
            {
                var types = type.GetGenericArguments();
                var genericTy = type.GetGenericArguments().FirstOrDefault();
                if (genericTy != null && !_handerDictionary.ContainsKey(genericTy))
                {
                    var messageHander = Activator.CreateInstance(type) as IMessageHander;
                    if (messageHander != null)
                    {
                        _handerDictionary.Add(genericTy, messageHander);
                    }
                }
            }
        }

        public void RegisterHander<T>(IMessageHander<T> messageHander) where T : class
        {
            _handerDictionary.TryAdd(typeof(T), messageHander);
            Log.Print($"RegisterHander>>>>>{typeof(T).Name}=>{messageHander.GetType().Name}");
        }

        private bool CheckedHnader(Type type)
        {
            if (type == null)
                return false;
            if (type == typeof(LoginHnader))
                Log.Print("LoginHnader");

            if (type.IsClass && !type.IsAbstract && typeof(IMessageHander).IsAssignableFrom(type))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Dispatch(object message, IClient context)
        {
            var ty = message.GetType();

            if (_handerDictionary.TryGetValue(ty, out var hander))
            {
                hander.Hander(message, context);
            }
            else
            {
                Log.Print($"no find message hander , message type : {message.GetType()}");
            }
        }
    }
}