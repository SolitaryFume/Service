using SqlSugar;

namespace DB
{
    public class User
    {
        [SugarColumn(IsPrimaryKey = true,IsIdentity = true)]
        public int Id{get;set;}
        public string Name{get;set;}
        public string Password{get;set;}
    }

    public interface IUserServer
    {
        bool Exist(string userName);
        bool Add(User user);
        bool CheckedPassWord(User user);
        User GetByUserName(string userName);
    }
}