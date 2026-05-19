namespace PriceNest.Api.Services;

using System.Net.Http;
using System.Net.Http.Json;
public class ScraperService
{
    private readonly HttpClient _httpClient;
    public ScraperService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> ScrapeData(string item)
    {
        var data = new { item = item };
        var res = await _httpClient.PostAsJsonAsync("api/scrape", data);
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadAsStringAsync();
    }
}