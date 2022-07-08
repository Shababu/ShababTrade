using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShababTrade.Data.Models
{
    internal class UserInfo
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public UserInfo(int id, string login, string password)
        {
            UserId = id;
            Login = login;
            Password = password;
        }
    }
}
