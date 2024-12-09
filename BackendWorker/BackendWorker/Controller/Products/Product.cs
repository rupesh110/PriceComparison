using Newtonsoft.Json;

namespace BackendWorker.Controller.Products;


public class Product
{
    [JsonProperty("product_name")]
    public string ProductName { get; set; }

    [JsonProperty("product_brand")]
    public string ProductBrand { get; set; }

    [JsonProperty("current_price")]
    public double CurrentPrice { get; set; }
    [JsonProperty("product_size")]
    public string ProductSize { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }
}


