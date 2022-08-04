using System.Net;

namespace ShababTrade.Data.Models
{
    internal class UserLoginInfo
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Exchange { get; set; }
        public string PublicKey { get; }
        public string PrivateKey { get; }

        public UserLoginInfo(int userId, string username, string password, string exchange, string publicKey, string privateKey)
        {
            UserId = userId;
            Username = username;
            Password = password;
            Exchange = exchange;
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }

        public UserLoginInfo(int userId, NetworkCredential credential, string exchange, string publicKey, string privateKey)
        {
            UserId = userId;
            Username = credential.UserName;
            Password = credential.Password;
            Exchange = exchange;
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }
        public UserLoginInfo() { }
    }
}
