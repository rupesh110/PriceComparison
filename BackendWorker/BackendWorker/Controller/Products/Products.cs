using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using BackendWorker.Controller.Coles;
using BackendWorker.Controller.Woolies;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;

namespace BackendWorker.Controller.Products
{
    public class Products
    {
        private readonly ILogger<Products> _logger;
        private readonly ProductSearchColesHelper _productSearchColesHelper;
        private readonly ProductSearchWooliesHelper _productSearchWooliesHelper;

        public Products(ILogger<Products> logger,
                        ProductSearchColesHelper productSearchColesHelper,
                        ProductSearchWooliesHelper productSearchWooliesHelper)
        {
            _logger = logger;
            _productSearchColesHelper = productSearchColesHelper;
            _productSearchWooliesHelper = productSearchWooliesHelper;
        }

        [Function("Products")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                var queryParams = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
                var query = queryParams["query"];

                if (string.IsNullOrEmpty(query))
                {
                    _logger.LogWarning("Query parameter is missing or empty.");
                    var badRequestResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    await badRequestResponse.WriteStringAsync("Query parameter is required");
                    return badRequestResponse;
                }

                // Call both helpers to fetch product data from Coles and Woolworths
                var productResultsColes = await _productSearchColesHelper.SearchProductAsync(query);
                var productResultsWoolies = await _productSearchWooliesHelper.SearchProductAsync(query);

                _logger.LogInformation($"Raw woolies Response: {productResultsWoolies}");

                var colesResponse = JsonConvert.DeserializeObject<ProductSearchResponse>(productResultsColes);
                var wooliesResponse = JsonConvert.DeserializeObject<ProductSearchResponse>(productResultsWoolies);
                _logger.LogInformation($"Test Deserialized Response: {JsonConvert.SerializeObject(wooliesResponse)}");
                var filteredColesResults = colesResponse.Results
                    .Where(p => !string.IsNullOrEmpty(p.ProductName))
                    .Select(p => new
                    {
                        p.ProductName,
                        p.ProductBrand,
                        CurrentPrice = $"${p.CurrentPrice:F2}"
                    })
                    .ToList();

                var filteredWooliesResults = wooliesResponse.Results
                    .Where(p => !string.IsNullOrEmpty(p.ProductName))
                    .Select(p => new
                    {
                        p.ProductName,
                        p.ProductBrand,
                        CurrentPrice = $"${p.CurrentPrice:F2}"
                    })
                    .ToList();
                _logger.LogInformation(JsonConvert.SerializeObject(filteredWooliesResults));
               
                var combinedResults = new
                {
                    Coles = filteredColesResults,
                    Woolworths = filteredWooliesResults
                };

                var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "application/json");

                // Serialize combined results to JSON
                await response.WriteStringAsync(System.Text.Json.JsonSerializer.Serialize(combinedResults));

                return response;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error occurred while calling the API.");
                var errorResponse = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                errorResponse.WriteString("An error occurred while processing your requests.");
                return errorResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred.");
                var errorResponse = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                errorResponse.WriteString("An unexpected error occurred.");
                return errorResponse;
            }
        }
    }
}
