using EdgyDiscordBotName.Commands.MemeGenerator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace EdgyDiscordBotName.Commands.MemeGenerator
{
    public static class ImgFlipClient
    {
        private static HttpClient client = new HttpClient();
        private static JsonSerializerOptions jsOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        public static async Task<GetTemplatesResponse> GetTemplates()
        {
            var response = await client.GetAsync(@"https://api.imgflip.com/get_memes");
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<GetTemplatesResponse>(json, jsOptions);
        }

        public static async Task<CreateMemeResponse> CreateMeme(MemeParameters parameters)
        {
            var values = CreateQueryObject(parameters);
            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync(@"https://api.imgflip.com/caption_image", content);

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CreateMemeResponse>(json, jsOptions);
        }

        private static Dictionary<string, string> CreateQueryObject(MemeParameters parameters)
        {
            var result = new Dictionary<string, string>();

            var properties = typeof(MemeParameters).GetProperties();

            foreach(var property in properties.Where(p => p.PropertyType == typeof(string) || p.PropertyType == typeof(int)))
            {
                var value = parameters.GetType().GetProperty(property.Name).GetValue(parameters, null).ToString();

                if(!String.IsNullOrEmpty(value))
                {
                    result.Add(property.Name.ToLower(), value);
                }

            }

            return result;
        }
    }
}
