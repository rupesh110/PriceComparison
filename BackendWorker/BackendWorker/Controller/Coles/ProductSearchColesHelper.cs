using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace BackendWorker.Controller.Coles;

public class ProductSearchColesHelper
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProductSearchColesHelper> _logger;
    private const string ColesApiBaseUrl = "https://coles-product-price-api.p.rapidapi.com/coles/product-search/";

    public ProductSearchColesHelper(IHttpClientFactory httpClientFactory, ILogger<ProductSearchColesHelper> logger)
    {
        _httpClient = httpClientFactory.CreateClient();
        _logger = logger;
    }

    public async Task<string> SearchProductAsync(string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            _logger.LogWarning("Query parameter is missing or empty.");
            throw new ArgumentException("Query parameter is required.");
        }

        try
        {
            var apiUrl = $"{ColesApiBaseUrl}?query={query}";
            var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
            request.Headers.Add("x-rapidapi-host", "coles-product-price-api.p.rapidapi.com");
            request.Headers.Add("x-rapidapi-key", "b55dd737e7mshec2849083906f9ap1b4889jsn273c8cc160f6");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("API response received successfully.");

            return content;  // Return the API response content (JSON)
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error occurred while calling the Coles API.");
            throw new Exception("An error occurred while processing your request.", ex);
        }
    }
}
