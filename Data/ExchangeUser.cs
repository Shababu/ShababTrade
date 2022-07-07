using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace ShababTrade.Data
{
    internal class ExchangeUser
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Exchange { get; set; }
        public string PublicKey { get; }
        public string PrivateKey { get; }

        public ExchangeUser(int userId, string login, string password, string exchange, string publicKey, string privateKey)
        {
            UserId = userId;
            Login = login;
            Password = password;
            Exchange = exchange;
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }

        public static List<ExchangeUser> GetAllAppUsers()
        {
            List<ExchangeUser> appUsers = new List<ExchangeUser>();
            ExchangeUser user;

            var connectionString = ConfigurationManager.ConnectionStrings["ShababTrade"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "Select UserInfo.UserId, UserInfo.Login, UserInfo.Password, KeyPairs.Exchange, " +
                    "KeyPairs.PublicKey, KeyPairs.PrivateKey from UserInfo join KeyPairs on UserInfo.UserId = KeyPairs.UserId";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int userId = Convert.ToInt32(reader[0]);
                    string login = reader[1].ToString();
                    string password = reader[2].ToString();
                    string exchange = reader[3].ToString();
                    string publicKey = reader[4].ToString();
                    string privateKey = reader[5].ToString();

                    user = new ExchangeUser(userId, login, password, exchange, publicKey, privateKey);
                    appUsers.Add(user);
                };
                connection.Close();
            }

            return appUsers;
        }

        public static List<ExchangeUser> GetAppUserByLoginAndPawwsord(string login, string password)
        {
            List<ExchangeUser> appUsers = new List<ExchangeUser>();
            ExchangeUser user;

            var connectionString = ConfigurationManager.ConnectionStrings["ShababTrade"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"Select UserInfo.UserId, UserInfo.Login, UserInfo.Password, KeyPairs.Exchange, " +
                    $"KeyPairs.PublicKey, KeyPairs.PrivateKey from UserInfo join KeyPairs on UserInfo.UserId = KeyPairs.UserId where Login = '{login}' AND Password = '{password}'";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int userId = Convert.ToInt32(reader[0]);
                    string loginFromDB = reader[1].ToString();
                    string passwordFromDB = reader[2].ToString();
                    string exchange = reader[3].ToString();
                    string publicKey = reader[4].ToString();
                    string privateKey = reader[5].ToString();

                    user = new ExchangeUser(userId, login, password, exchange, publicKey, privateKey);
                    appUsers.Add(user);
                };
                connection.Close();
            }

            return appUsers;
        }
    }
}
