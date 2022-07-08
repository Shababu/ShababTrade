using ShababTrade.Data.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

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
                reader.Close();
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
                reader.Close();
                connection.Close();
            }

            return appUsers;
        }

        public static bool TryInsertNewUser(string username, string password, string exchange, string publicKey, string privateKey, out string resultMessage)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ShababTrade"].ConnectionString;
            string sql;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var isUserRegestered = TryGetUserInfoByLogin(username);

                if (!isUserRegestered)
                {
                    sql = $"Insert into UserInfo(Login, Password)" +
                          $"Values ('{username}', '{password}')";
                    UserInfo userInfo;

                    try
                    {
                        SqlCommand command = new SqlCommand(sql, connection);
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        resultMessage = "Error. Unable to add user into database";
                        return false;
                    }

                    userInfo = GetUserInfoByLogin(username);

                    sql = $"Insert into KeyPairs(Exchange, PublicKey, PrivateKey, UserId)" +
                          $"Values ('{exchange}', '{publicKey}', '{privateKey}', '{userInfo.UserId}')";
                    try
                    {
                        SqlCommand command = new SqlCommand(sql, connection);
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        resultMessage = "Error. Unable to keys into database";
                        return false;
                    }

                    connection.Close();
                    resultMessage = $"Success. Api keys was assined to user: {username}";
                    return true;
                }

                List<ExchangeUser> exchangeUsers;

                try
                {
                    exchangeUsers = GetExchangeUserByLogin(username);
                }
                catch (Exception ex)
                {
                    connection.Close();
                    resultMessage = "Error. Unable to get user from database";
                    return false;
                }

                var currentUsers = exchangeUsers.Where(user => user.Exchange == exchange);
                ExchangeUser currentUser = new ExchangeUser();

                if (currentUsers.Any())
                {
                    currentUser = currentUsers.First();
                }

                if (currentUser.Login == username && currentUser.PublicKey == publicKey && currentUser.PrivateKey == privateKey)
                {
                    resultMessage = $"Error. User {username} already has this pair of keys";
                    return false;
                }


                sql = $"Insert into KeyPairs (Exchange, PublicKey, PrivateKey, UserId)" +
                      $"Values ('{exchange}', '{publicKey}', '{privateKey}', '{exchangeUsers[0].UserId}')";

                try
                {
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.ExecuteNonQuery();
                    resultMessage = $"Success. Api keys was assined to user: {username}";
                    return true;
                }
                catch (Exception ex)
                {
                    resultMessage = "Error. Unable to add keys into database";
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public static List<ExchangeUser> GetExchangeUserByLogin(string username)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ShababTrade"].ConnectionString;
            List<ExchangeUser> exchangeUsers = new List<ExchangeUser>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var sql = $"Select UserInfo.UserId, Login, Password, Exchange, PublicKey, PrivateKey From UserInfo " +
                          $"Join KeyPairs on KeyPairs.UserId = UserInfo.UserId " +
                          $"Where Login = '{username}'";

                try
                {
                    SqlCommand command = new SqlCommand(sql, connection);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var userId = (int)reader[0];
                            var login = (string)reader[1];
                            var password = (string)reader[2];
                            var exchange = (string)reader[3];
                            var publicKey = (string)reader[4];
                            var privateKey = (string)reader[5];

                            var exchangeUser = new ExchangeUser(userId, login, password, exchange, publicKey, privateKey);
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

        public static bool TryGetExchangeUserByLogin(string username)
        {
            List<ExchangeUser> exchangeUsers = new List<ExchangeUser>();
            exchangeUsers = GetExchangeUserByLogin(username);
            if (exchangeUsers.Count > 0)
                return true;

            return false;
        }

        public static UserInfo GetUserInfoByLogin(string username)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ShababTrade"].ConnectionString;
            UserInfo userInfo;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var sql = $"Select UserInfo.UserId, Login, Password " +
                          $"From UserInfo " +
                          $"Where Login = '{username}'";

                try
                {
                    SqlCommand command = new SqlCommand(sql, connection);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        var userId = (int)reader[0];
                        var login = (string)reader[1];
                        var password = (string)reader[2];

                        userInfo = new UserInfo(userId, login, password);
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

        public static bool TryGetUserInfoByLogin(string username)
        {
            UserInfo userInfo = GetUserInfoByLogin(username);
            if (userInfo == null)
                return false;

            return true;
        }

        public ExchangeUser(int userId, string login, string password, string exchange, string publicKey, string privateKey)
        {
            UserId = userId;
            Login = login;
            Password = password;
            Exchange = exchange;
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }
        public ExchangeUser() { }
    }
}
