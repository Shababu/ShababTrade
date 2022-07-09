using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShababTrade.Data.Models
{
    internal class ExchangeUser
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Exchange { get; set; }
        public string PublicKey { get; }
        public string PrivateKey { get; }

        public ExchangeUser(int userId, string username, string password, string exchange, string publicKey, string privateKey)
        {
            UserId = userId;
            Username = username;
            Password = password;
            Exchange = exchange;
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }
        public ExchangeUser() { }
    }
}
