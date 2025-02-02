using BaseLibrary.Responses;
using ClientLibrary.Helpers;
using ClientLibrary.Services.Contracts;
using System.Net.Http.Json;

namespace ClientLibrary.Services.Implementations
{
    public class GenericService<T> : IGenericService<T>
    {
        private readonly GetHttpClient getHttpClient;
        public GenericService(GetHttpClient getHttpClient)
        {
            this.getHttpClient = getHttpClient;
        }
        public async Task<List<T>> GetAll(string baseUrl)
        {
            var httpClient = getHttpClient.GetPublicHttpClient();
            var response = await httpClient.GetFromJsonAsync<List<T>>($"{baseUrl}/all");
            return response!;
        }

        public async Task<T> GetById(int id, string baseUrl)
        {
            var httpClient = getHttpClient.GetPublicHttpClient();
            var response = await httpClient.GetFromJsonAsync<T>($"{baseUrl}/get/{id}");
            return response!;
        }

        public async Task<GeneralResponse> Insert(T entity, string baseUrl)
        {
            var httpClient = getHttpClient.GetPublicHttpClient();
            var response = await httpClient.PostAsJsonAsync($"{baseUrl}/add", entity);
            var result = await response.Content.ReadFromJsonAsync<GeneralResponse>();
            return result!;
        }

        public async Task<GeneralResponse> Update(T entity, string baseUrl)
        {
            var httpClient = getHttpClient.GetPublicHttpClient();
            var response = await httpClient.PutAsJsonAsync($"{baseUrl}/update", entity);
            var result = await response.Content.ReadFromJsonAsync<GeneralResponse>();
            return result!;
        }
        public async Task<GeneralResponse> DeleteById(int id, string baseUrl)
        {
            var httpClient = getHttpClient.GetPublicHttpClient();
            var response = await httpClient.DeleteAsync($"{baseUrl}/delete/{id}");
            var result = await response.Content.ReadFromJsonAsync<GeneralResponse>();
            return result!;
        }

    }
}
