using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendWorker.Controller.Products;

public class ProductSearchResponse
{
    public string Query { get; set; }

    [JsonProperty("results")]
    public List<Product> Results { get; set; }

    [JsonProperty("total_results")]
    public int TotalResults { get; set; }

    [JsonProperty("total_pages")]
    public int TotalPages { get; set; }

    [JsonProperty("current_page")]
    public int CurrentPage { get; set; }
}
