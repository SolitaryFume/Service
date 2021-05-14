using System;
using System.Collections.Generic;
using System.Linq;
using MessageHander;
using ProtoMessage;
using Core;

namespace Service
{
    public class MessageHanderService : ServiceBase
    {
        public static bool AUTOREGISTER = false;//自动注册

        private Dictionary<Type, IMessageHander> _handerDictionary;
        private MessageHanderService()
        {
            _handerDictionary = new Dictionary<Type,IMessageHander>();

            if(AUTOREGISTER)
            {
                AutoRegister();
            }
            else
            {
                InitRegister();
            }
        }

        private void InitRegister()
        {
            RegisterHander<LoginRequest>(new LoginHnader());
        }

        private void AutoRegister()
        {
            Debug.Log(">>>>> MessageHanderService.AutoRegister");

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assemblie in assemblies)
            {
                var types = assemblie.GetTypes();
                foreach (var ty in types)
                {
                    TryAddHanderType(ty);
                }
            }
        }

        private void TryAddHanderType(Type type)
        {
            if (CheckedHnader(type))
            {
                var types = type.GetGenericArguments();
                var genericTy = type.GetGenericArguments().FirstOrDefault();
                if(genericTy!=null &&  !_handerDictionary.ContainsKey(genericTy))
                {
                    var messageHander = Activator.CreateInstance(type) as IMessageHander;
                    if(messageHander!=null)
                    {
                        Debug.Log($"MessageHanderService Register :[{genericTy.Name}:{messageHander.GetType()}]");
                        _handerDictionary.Add(genericTy,messageHander);
                    }
                }
            }
        }

        public void RegisterHander<T>(IMessageHander<T> messageHander) where T :class
        {
            _handerDictionary.TryAdd(typeof(T),messageHander);
            Debug.Log($"RegisterHander>>>>>{typeof(T).Name}=>{messageHander.GetType().Name}");
        }

        private bool CheckedHnader(Type type)
        {
            if (type == null)
                return false;
            if(type==typeof(LoginHnader))
                Debug.Log("LoginHnader");
            
            if (type.IsClass && !type.IsAbstract &&typeof(IMessageHander).IsAssignableFrom(type))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Dispatch(object message,IUser context)
        {
            var ty = message.GetType();

            if(_handerDictionary.TryGetValue(ty,out var hander))
            {
                hander.Hander(message,context);
            }
            else
            {
                Debug.Error($"no find message hander , message type : {message.GetType()}");
            }
        }
    }
}