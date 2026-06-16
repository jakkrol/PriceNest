using Microsoft.SemanticKernel;
using PriceNest.Api.Services;
using System.ComponentModel;

namespace PriceNest.Api.Plugins;

public class ProductPlugin
{
    private readonly ProductService _productService;
    private readonly ScraperService _scraperService;

    public ProductPlugin(ProductService productService, ScraperService scraperService)
    {
        _productService = productService;
        _scraperService = scraperService;
    }

    [KernelFunction]
    [Description("Pobiera informacje o produkcie na podstawie jego numeru ID")]
    public async Task<string> GetProductDetails(
        [Description("Unikalny identyfikator produktu (ID)")] int id)
    {
        var product = await _productService.GetProductByIdWithOffersAsync(id);

        if (product == null) return "Niestety nie znalazłem produktu o takim ID.";

        return $"Produkt: {product.Name}, Oferty: {product.Offers}";
    }

    [KernelFunction]
    [Description("Pobiera z internetu informacje o produkcie lub wielu produktach na podstawie nazwy")]
    public async Task<string> ScrapeProductData(
        [Description("Nazwa produktu do wyszukania")] string item)
    {
        var scrapedJson = await _scraperService.ScrapeData(item);
        return scrapedJson;
    }
}