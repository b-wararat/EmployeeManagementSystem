
using Blazored.LocalStorage;

namespace ClientLibrary.Helpers
{
    public class LocalStorageService(ILocalStorageService localStorageService)
    {
        private const string storegeKey = "authentication-token";
        public async Task<string> GetToken() => await localStorageService.GetItemAsStringAsync(storegeKey);
        public async Task SetToken(string item) => await localStorageService.SetItemAsStringAsync(storegeKey, item);
        public async Task RemoveToken() => await localStorageService.RemoveItemAsync(storegeKey);
    }
}
