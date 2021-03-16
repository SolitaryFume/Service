using System.Collections.Generic;

namespace Service
{
    public abstract class TokenService : ServiceBase
    {
        private List<IToken> m_list;
        private TokenService()
        {
            m_list = new List<IToken>();
        }

        public void Register(IToken token)
        {
            Log.Print("用户登录");
            m_list.Add(token);
        }

        public bool Unregister(IToken token)
        {
            token.Dispose();
            return m_list.Remove(token);
        }

        public IToken GetToken()
        {
            return new UserToken();   
        }
    }
}