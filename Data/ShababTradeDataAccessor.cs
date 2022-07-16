using ShababTrade.Data.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;

namespace ShababTrade.Data
{
    internal class ShababTradeDataAccessor
    {
        public static List<ExchangeUser> GetAllAppUsers()
        {
            List<ExchangeUser> appUsers = new List<ExchangeUser>();
            ExchangeUser user;

            var connectionString = ConfigurationManager.ConnectionStrings["ShababTrade"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "Select UserInfo.UserId, UserInfo.Username, UserInfo.Password, KeyPairs.Exchange, " +
                    "KeyPairs.PublicKey, KeyPairs.PrivateKey from UserInfo join KeyPairs on UserInfo.UserId = KeyPairs.UserId";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int userId = Convert.ToInt32(reader[0]);
                    string username = reader[1].ToString();
                    string password = reader[2].ToString();
                    string exchange = reader[3].ToString();
                    string publicKey = reader[4].ToString();
                    string privateKey = reader[5].ToString();

                    user = new ExchangeUser(userId, username, password, exchange, publicKey, privateKey);
                    appUsers.Add(user);
                };
                reader.Close();
                connection.Close();
            }

            return appUsers;
        } 

        public static List<ExchangeUser> GetExchangeUsersByUsernameAndPawwsord(NetworkCredential credential)
        {
            List<ExchangeUser> appUsers = new List<ExchangeUser>();
            ExchangeUser user;

            var connectionString = ConfigurationManager.ConnectionStrings["ShababTrade"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"SELECT UserInfo.UserId, UserInfo.Username, UserInfo.Password, KeyPairs.Exchange, " +
                             $"KeyPairs.PublicKey, KeyPairs.PrivateKey " +
                             $"FROM UserInfo " +
                             $"JOIN KeyPairs on UserInfo.UserId = KeyPairs.UserId " +
                             $"WHERE Username = '{credential.UserName}' AND Password = '{credential.Password}'";

                SqlCommand command = new SqlCommand(sql, connection);

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int userId = Convert.ToInt32(reader[0]);
                    string usernameFromDB = reader[1].ToString();
                    string passwordFromDB = reader[2].ToString();
                    string exchange = reader[3].ToString();
                    string publicKey = reader[4].ToString();
                    string privateKey = reader[5].ToString();

                    user = new ExchangeUser(userId, credential, exchange, publicKey, privateKey);
                    appUsers.Add(user);
                };
                reader.Close();
                connection.Close();
            }

            return appUsers;
        } 

        public static bool TryInsertNewUser(NetworkCredential credential, string exchange, string publicKey, string privateKey, out string resultMessage)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ShababTrade"].ConnectionString;
            string sql;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var isUserRegistered = TryGetUserInfoByUsername(credential.UserName);

                if (!isUserRegistered)
                {
                    isUserRegistered = RegisterUser(credential, exchange, publicKey, privateKey, connection, out resultMessage);
                    if (isUserRegistered) 
                    {
                        return true;
                    }
                }

                bool addKeysResult = AddApiKeysToUser(credential, exchange, publicKey, privateKey, connection, out resultMessage);

                connection.Close();

                return addKeysResult;               
            }
        }

        public static bool TryGetExchangeUserByUsername(string username)
        {
            List<ExchangeUser> exchangeUsers = new List<ExchangeUser>();
            exchangeUsers = GetExchangeUsersByUsername(username);
            if (exchangeUsers.Count > 0)
                return true;

            return false;
        } 

        public static UserInfo GetUserInfoByUsername(string username)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ShababTrade"].ConnectionString;
            UserInfo userInfo;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var sql = $"SELECT UserInfo.UserId, Username, Password " +
                          $"FROM UserInfo " +
                          $"WHERE Username = '{username}'";

                SqlCommand command = new SqlCommand(sql, connection);

                try
                {
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        var userId = (int)reader[0];
                        var usernameFromDb = (string)reader[1];
                        var password = (string)reader[2];

                        userInfo = new UserInfo(userId, usernameFromDb, password);
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    connection.Close();
                }
            }

            return userInfo;
        } 

        public static bool TryGetUserInfoByUsername(string username)
        {
            UserInfo userInfo = GetUserInfoByUsername(username);
            if (userInfo == null)
                return false;

            return true;
        } 

        public static void InsertUserInfo(NetworkCredential credential)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ShababTrade"].ConnectionString;
            string sql;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                sql = $"Insert into UserInfo(Username, Password) " +
                       $"Values ('{credential.UserName}', '{credential.Password}')";

                SqlCommand command = new SqlCommand(sql, connection);

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    connection.Close();
                }
            }
        } 

        public static void InsertKeyPairs(string exchange, string publicKey, string privateKey, int userId)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ShababTrade"].ConnectionString;
            string sql;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                sql = $"Insert into KeyPairs(Exchange, PublicKey, PrivateKey, UserId)" +
                      $"Values ('{exchange}', '{publicKey}', '{privateKey}', '{userId}')";

                SqlCommand command = new SqlCommand(sql, connection);

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    connection.Close();
                }
            }
        } 

        private static List<ExchangeUser> GetExchangeUsersByUsername(string username)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ShababTrade"].ConnectionString;
            List<ExchangeUser> exchangeUsers = new List<ExchangeUser>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var sql = $"SELECT UserInfo.UserId, Username, Password, Exchange, PublicKey, PrivateKey " +
                          $"FROM UserInfo " +
                          $"JOIN KeyPairs on KeyPairs.UserId = UserInfo.UserId " +
                          $"WHERE Username = '{username}'";

                try
                {
                    SqlCommand command = new SqlCommand(sql, connection);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var userId = (int)reader[0];
                            var usernameFromDb = (string)reader[1];
                            var password = (string)reader[2];
                            var exchange = (string)reader[3];
                            var publicKey = (string)reader[4];
                            var privateKey = (string)reader[5];

                            var exchangeUser = new ExchangeUser(userId, usernameFromDb, password, exchange, publicKey, privateKey);
                            exchangeUsers.Add(exchangeUser);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    connection.Close();
                }
            }

            return exchangeUsers;
        } 

        private static bool RegisterUser(NetworkCredential credential, string exchange, string publicKey, string privateKey, SqlConnection connection, out string resultMessage)
        {
            try
            {
                InsertUserInfo(credential);
            }
            catch (Exception ex)
            {
                resultMessage = "Error. Unable to add user into database";
                return false;
            }

            UserInfo userInfo = GetUserInfoByUsername(credential.UserName);

            try
            {
                InsertKeyPairs(exchange, publicKey, privateKey, userInfo.UserId);
            }
            catch (Exception ex)
            {
                resultMessage = "Error. Unable to keys into database";
                return false;
            }

            resultMessage = $"Success. {exchange} api keys was assined to user: {credential.UserName}";
            return true;
        }

        private static bool AddApiKeysToUser(NetworkCredential credential, string exchange, string publicKey, string privateKey, SqlConnection connection, out string resultMessage)
        {
            List<ExchangeUser> exchangeUsers;

            try
            {
                exchangeUsers = GetExchangeUsersByUsername(credential.UserName);
            }
            catch (Exception ex)
            {
                resultMessage = "Error. Unable to get user from database";
                return false;
            }

            var currentExchangeUser = exchangeUsers.Where(user => user.Exchange == exchange);
            ExchangeUser currentUser = new ExchangeUser();

            if (currentExchangeUser.Any())
            {
                foreach (var user in currentExchangeUser)
                {
                    if (user.Username == credential.UserName && user.PublicKey == publicKey && user.PrivateKey == privateKey)
                    {
                        resultMessage = $"Error. User {credential.UserName} already has this pair of keys";
                        return false;
                    }
                }
            }

            try
            {
                InsertKeyPairs(exchange, publicKey, privateKey, exchangeUsers[0].UserId);
                resultMessage = $"Success. {exchange} api keys was assined to user: {credential.UserName}";
                return true;
            }
            catch (Exception ex)
            {
                resultMessage = "Error. Unable to add keys into database";
                return false;
            }
        }
    }
}
