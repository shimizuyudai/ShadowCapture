using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;

namespace UtilPack4Unity
{
    public static class HttpRequestUtils
    {

        public static async Task<string> POST4GetString(string url, string message)
        {
            var response = await POST(url, message);
            var result = await response.Content.ReadAsStringAsync();
            response.Dispose();
            return result;
        }

        public static async Task<byte[]> POST4GetBytes(string url, string message)
        {
            var response = await POST(url, message);
            var result = await response.Content.ReadAsByteArrayAsync();
            response.Dispose();
            return result;
        }

        public static async Task<HttpResponseMessage> POST(string url, string message)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(message);
                var response = await client.PostAsync(url, content);
                return response;
            }
        }

        public static async Task<HttpResponseMessage> GET(string url)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                return response;
            }
        }

        public static async Task<string> GET4GetString(string url)
        {
            var response = await GET(url);
            var result = await response.Content.ReadAsStringAsync();
            response.Dispose();
            return result;
        }

        public static async Task<byte[]> GET4GetBytes(string url)
        {
            var response = await GET(url);
            var result = await response.Content.ReadAsByteArrayAsync();
            response.Dispose();
            return result;
        }

    }
}
