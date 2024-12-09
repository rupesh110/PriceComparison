using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BackendWorker.Controller.Woolies
{
    public  class ProductSearchWooliesHelper
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductSearchWooliesHelper> _logger;
        private const string WooliesApiBaseUrl = "https://woolworths-products-api.p.rapidapi.com/woolworths/product-search/";

        public ProductSearchWooliesHelper(IHttpClientFactory httpClientFactory, ILogger<ProductSearchWooliesHelper> logger)
        {
            // Initialize HttpClient from IHttpClientFactory to support DI and avoid socket exhaustion
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
        }

        public async Task<string> SearchProductAsync(string query)
        {
            // Check for empty or null query and log the warning
            if (string.IsNullOrEmpty(query))
            {
                _logger.LogWarning("Query parameter is missing or empty.");
                throw new ArgumentException("Query parameter is required.", nameof(query));
            }

            try
            {
                // Build the full URL with the query parameter
                var apiUrl = $"{WooliesApiBaseUrl}?query={query}";

                // Create the HTTP request message with necessary headers
                var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                request.Headers.Add("x-rapidapi-host", "woolworths-products-api.p.rapidapi.com");
                request.Headers.Add("x-rapidapi-key", "b55dd737e7mshec2849083906f9ap1b4889jsn273c8cc160f6");

                // Send the request asynchronously
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();  // Throw exception for non-success status codes

                // Read the response content as a string (assumed to be JSON)
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("API response received successfully.");

                // Return the API response content (JSON)
                return content;
            }
            catch (HttpRequestException ex)
            {
                // Log the specific error that occurred while making the HTTP request
                _logger.LogError(ex, "Error occurred while calling the Woolworths API.");
                throw new Exception("An error occurred while processing your request.", ex);
            }
            catch (Exception ex)
            {
                // Catch any other unexpected exceptions
                _logger.LogError(ex, "Unexpected error occurred during API call.");
                throw new Exception("An unexpected error occurred.", ex);
            }
        }
    }
}
