using Client_DoMInhKhoa.Session;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Client_DoMInhKhoa.Services
{
    public static class ApiClient
    {
        private static readonly HttpClient _httpClient;

        static ApiClient()
        {
            _httpClient = new HttpClient
            {
                // TODO: sửa lại cho đúng URL API của bạn
                BaseAddress = new Uri("https://localhost:7072"),
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        private static void ApplyAuthHeader()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(SessionHienTai.JwtToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", SessionHienTai.JwtToken);
            }
        }

        public static async Task<T> PostAsync<T>(string url, object data, bool includeAuth = false)
        {
            if (includeAuth)
            {
                ApplyAuthHeader();
            }

            string json = JsonConvert.SerializeObject(data);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await _httpClient.PostAsync(url, content);

            string responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Lỗi API ({(int)response.StatusCode}): {responseContent}");
            }

            if (typeof(T) == typeof(string))
            {
                return (T)(object)responseContent;
            }

            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        public static async Task<T> GetAsync<T>(string url, bool includeAuth = true)
        {
            if (includeAuth)
            {
                ApplyAuthHeader();
            }

            using HttpResponseMessage response = await _httpClient.GetAsync(url);
            string responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Lỗi API ({(int)response.StatusCode}): {responseContent}");
            }

            if (typeof(T) == typeof(string))
            {
                return (T)(object)responseContent;
            }

            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        public static async Task<T> PutAsync<T>(string url, object data, bool includeAuth = true)
        {
            if (includeAuth)
            {
                ApplyAuthHeader();
            }

            string json = JsonConvert.SerializeObject(data);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await _httpClient.PutAsync(url, content);
            string responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Lỗi API ({(int)response.StatusCode}): {responseContent}");
            }

            if (typeof(T) == typeof(string))
            {
                return (T)(object)responseContent;
            }

            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        public static async Task DeleteAsync(string url, bool includeAuth = true)
        {
            if (includeAuth)
            {
                ApplyAuthHeader();
            }

            using HttpResponseMessage response = await _httpClient.DeleteAsync(url);
            string responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Lỗi API ({(int)response.StatusCode}): {responseContent}");
            }
        }
    }
}
