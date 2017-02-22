using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Api.Extensions
{
    public enum RequestType { Get, Post }

    public class WebService
    {
        public static async Task<T> Request<T>(RequestType type, string uri, object content = null, string token = "")
        {
            StringContent jsonContent = null;
            var request = new HttpClient();
            request.DefaultRequestHeaders.Accept.Clear();
            request.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(token))
                request.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (content != null)
                jsonContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            switch (type)
            {
                case RequestType.Get:
                    response = await request.GetAsync(uri);
                    break;
                case RequestType.Post:
                    response = await request.PostAsync(uri, jsonContent);
                    break;
            }

            if (response == null || !response.IsSuccessStatusCode)
                throw new ArgumentException("Service returned an error.");
            var result = await response.Content.ReadAsStringAsync();
            return (typeof(T) == typeof(string)) ? (T)(object)result : JsonConvert.DeserializeObject<T>(result);
        }
    }
}
