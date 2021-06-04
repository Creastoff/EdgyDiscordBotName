using FitnessDeath.DataAccess;
using FitnessDeath.DataAccess.Entities;
using FitnessDeath.Logic.Entities;
using FitnessDeath.Logic.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessDeath.Logic
{
    public class BusinessLogic
    {
        private Database _database = null;

        public BusinessLogic(Database database)
        {
            _database = database;
        }

        public Response<int> CreateUser(ulong uid)
        {
            var response = new Response<int>();

            try
            {
                _database.CreateUser(uid);
            }
            catch (Exception ex)
            {
                response.Message = ex.ToString();
                response.Status = Status.Error;
            }

            return response;
        }

        public Response<bool> DoesUserExist(ulong uid)
        {
            var response = new Response<bool>();

            try
            {
                response.Data = _database.DoesUserExist(uid);
            }
            catch (Exception ex)
            {
                response.Message = ex.ToString();
                response.Status = Status.Error;
            }

            return response;
        }

        public Response<List<User>> GetAllUsers()
        {
            var response = new Response<List<User>>();

            try
            {
                response.Data = _database.GetAllUsers();

                if(response.Data.Count == 0 || response.Data == null)
                {
                    response.Status = Status.Fail;
                    response.Message = Messages.NO_USERS;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.ToString();
                response.Status = Status.Error;
            }

            return response;
        }

        public Response<User> GetUser(ulong uid)
        {
            var response = new Response<User>();

            if (uid == 0)
            {
                response.Message = Messages.USER_ID_0;
                response.Status = Status.Fail;
            }
            else
            {
                try
                {
                    if (DoesUserExist(uid).Data)
                    {
                        response.Data = _database.GetUser(uid);
                    }
                    else
                    {
                        response.Status = Status.Fail;
                        response.Message = Messages.USER_DOES_NOT_EXIST;
                    }
                }
                catch (Exception ex)
                {
                    response.Message = ex.ToString();
                    response.Status = Status.Error;
                }
            }

            return response;
        }

        public Response<int> AddDeaths(ulong uid, int numDeaths)
        {
            var response = new Response<int>();

            try
            {
                if (!DoesUserExist(uid).Data)
                {
                    CreateUser(uid);
                }

                _database.AddDeaths(uid, numDeaths);
            }
            catch (Exception ex)
            {
                response.Message = ex.ToString();
                response.Status = Status.Error;
            }

            return response;
        }

        public Response<int> AddPayment(ulong uid, int amount)
        {
            var response = new Response<int>();

            try
            {
                if (DoesUserExist(uid).Data)
                {
                    var user = GetUser(uid);

                    if ((user.Data.Deaths - user.Data.Paid - amount) < 0) //you have been naughty comrade
                    {
                        response.Status = Status.Fail;
                        response.Message = Messages.INVALID_PAYMENT_NEGATIVE;
                    }
                    else
                    {
                        _database.AddPayment(uid, amount);
                    }
                }
                else
                {
                    CreateUser(uid);
                    response.Status = Status.Fail;
                    response.Message = Messages.INVALID_PAYMENT_NONE_OWED;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.ToString();
                response.Status = Status.Error;
            }

            return response;
        }
    }
}
