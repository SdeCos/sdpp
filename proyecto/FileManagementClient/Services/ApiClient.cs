using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FileManagementClient.Models;
using Newtonsoft.Json;

namespace FileManagementClient.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://localhost:5019/api/files";
        private int? _userId;

        public ApiClient()
        {
            _httpClient = new HttpClient();
        }

        private void AddAuthHeader()
        {
            if (_userId.HasValue)
            {
                if (_httpClient.DefaultRequestHeaders.Contains("X-User-Id"))
                {
                    _httpClient.DefaultRequestHeaders.Remove("X-User-Id");
                }
                _httpClient.DefaultRequestHeaders.Add("X-User-Id", _userId.Value.ToString());
            }
        }

        public async Task<List<FileMetadata>> GetFilesAsync(int? parentId = null)
        {
            AddAuthHeader();
            var url = _baseUrl;
            if (parentId.HasValue)
            {
                url += $"?parentId={parentId.Value}";
            }
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<FileMetadata>>(content);
        }

        public async Task<FileMetadata> UploadFileAsync(string filePath, int? parentId = null)
        {
            AddAuthHeader();
            using (var content = new MultipartFormDataContent())
            {
                var fileContent = new ByteArrayContent(File.ReadAllBytes(filePath));
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
                content.Add(fileContent, "file", Path.GetFileName(filePath));

                if (parentId.HasValue)
                {
                    content.Add(new StringContent(parentId.Value.ToString()), "parentId");
                }

                var response = await _httpClient.PostAsync($"{_baseUrl}/upload", content);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<FileMetadata>(responseContent);
            }
        }

        public async Task<FileMetadata> CreateFolderAsync(string name, int? parentId = null)
        {
            AddAuthHeader();
            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StringContent(name), "name");
                if (parentId.HasValue)
                {
                    content.Add(new StringContent(parentId.Value.ToString()), "parentId");
                }

                var response = await _httpClient.PostAsync($"{_baseUrl}/folder", content);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<FileMetadata>(responseContent);
            }
        }

        public async Task<Stream> DownloadFileAsync(int id)
        {
            AddAuthHeader();
            var response = await _httpClient.GetAsync($"{_baseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();
        }

        public async Task<Stream> DownloadFolderAsZipAsync(int folderId)
        {
            AddAuthHeader();
            var response = await _httpClient.GetAsync($"{_baseUrl}/folder/{folderId}/download");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();
        }

        public async Task DeleteFileAsync(int id)
        {
            AddAuthHeader();
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });

            var response = await _httpClient.PostAsync("http://localhost:5019/api/auth/login", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject(responseContent);
                _userId = (int)result.id;
                return true;
            }
            return false;
        }


        public async Task<bool> RegisterAsync(string username, string password)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });

            var response = await _httpClient.PostAsync("http://localhost:5019/api/auth/register", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject(responseContent);
                _userId = (int)result.id;
                return true;
            }
            return false;
        }
    }
}
