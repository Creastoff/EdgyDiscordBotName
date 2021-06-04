using FitnessDeath.DataAccess.Entities;
using FitnessDeath.Logic.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EdgyDiscordBotName.Commands.FitnessDeath
{
    public static class FitnessDeathClient
    {
        public static string BaseUrl = "";
        public static HttpClient client = new HttpClient();
        private static JsonSerializerOptions jsOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        public static async Task<Response<int>> AddDeaths(ulong uid, int amount)
        {
            string address = BaseUrl + $"/AddDeaths?uid={uid}&amount={amount}";
            var response = await client.GetAsync(address);

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Response<int>>(json, jsOptions);
        }

        public static async Task<Response<int>> AddPayment(ulong uid, int amount)
        {
            string address = BaseUrl + $"/AddPayment?uid={uid}&amount={amount}";
            var response = await client.GetAsync(address);

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Response<int>>(json, jsOptions);
        }

        public static async Task<Response<User>> GetUser(ulong uid)
        {
            string address = BaseUrl + $"?uid={uid}";
            var response = await client.GetAsync(address);

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Response<User>>(json, jsOptions);
        }

        public static async Task<Response<List<User>>> GetAllUsers()
        {
            string address = BaseUrl + $"/AllUsers";
            var response = await client.GetAsync(address);

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Response<List<User>>>(json, jsOptions);
        }

        public static async Task<Response<int>> CreateUser(ulong uid)
        {
            string address = BaseUrl + $"/CreateUser?uid={uid}";
            var response = await client.GetAsync(address);

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Response<int>>(json, jsOptions);
        }
    }
}
