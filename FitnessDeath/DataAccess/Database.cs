using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FitnessDeath.Config;
using FitnessDeath.DataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace FitnessDeath.DataAccess
{
    public class Database
    {
        private MySqlConfig _config;
        private MySqlConnection _conn;

        public Database(MySqlConfig config)
        {
            _config = config;
            _conn = new MySqlConnection(_config.GenerateConnectionString());

            _conn.Open();
            _conn.Close();
        }

        public void CreateUser(ulong uid)
        {
            MySqlCommand cmd = new MySqlCommand("sp_CreateUser");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("uid", uid);

            ExecuteCommand<Entities.ValueType>(cmd);
        }

        public bool DoesUserExist(ulong uid)
        {
            bool exists = false;

            MySqlCommand cmd = new MySqlCommand("sp_DoesUserExist");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("uid", uid);

            var result = ExecuteCommand<Entities.ValueType>(cmd);
            
            if(result.Number != 0)
            {
                exists = true;
            }

            return exists;
        }

        public void AddDeaths(ulong uid, int amount)
        {
            MySqlCommand cmd = new MySqlCommand("sp_AddDeaths");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("uid", uid);
            cmd.Parameters.AddWithValue("numDeaths", amount);

            ExecuteCommand<Entities.ValueType>(cmd);
        }

        public void AddPayment(ulong uid, int amount)
        {
            MySqlCommand cmd = new MySqlCommand("sp_AddPayment");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("uid", uid);
            cmd.Parameters.AddWithValue("numPaid", amount);

            ExecuteCommand<Entities.ValueType>(cmd);
        }

        public User GetUser(ulong uid)
        {
            User user = null;

            MySqlCommand cmd = new MySqlCommand("sp_GetUser");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("uid", uid);

            user = ExecuteCommand<User>(cmd);

            return user;
        }

        public List<User> GetAllUsers()
        {
            List<User> users = null;

            MySqlCommand cmd = new MySqlCommand("sp_GetAllUsers");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            users = ExecuteCommandList<User>(cmd);

            return users;
        }

        private T ExecuteCommand<T>(MySqlCommand cmd) where T : IDatabaseEntity, new()
        {
            using (_conn)
            {
                using (cmd)
                {
                    cmd.Connection = _conn;

                    using (var sda = new MySqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        sda.Fill(dt);

                        var result = new T();

                        if (dt.Rows.Count > 0)
                        {
                            result.Populate(dt.Rows[0]);
                        }

                        return result;
                    }
                }
            }
        }

        private List<T> ExecuteCommandList<T>(MySqlCommand cmd) where T : IDatabaseEntity, new()
        {
            using (_conn)
            {
                using (cmd)
                {
                    cmd.Connection = _conn;

                    using (var sda = new MySqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        sda.Fill(dt);

                        var result = new List<T>();

                        foreach(DataRow row in dt.Rows)
                        {
                            var t = new T();
                            t.Populate(row);
                            result.Add(t);
                        }

                        return result;
                    }
                }
            }
        }
    }
}
