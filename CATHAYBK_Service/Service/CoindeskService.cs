using CATHAYBK_Model.WEBAPI.Coindesk;
using Newtonsoft.Json;

namespace CATHAYBK_Service.Service
{
    public class CoindeskService
    {
        private readonly HttpClient _httpClient;

        public CoindeskService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CoindeskResponse> GetCurrentPriceAsync()
        {
            var response = await _httpClient.GetAsync("https://api.coindesk.com/v1/bpi/currentprice.json");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CoindeskResponse>(json);
        }
    }
}