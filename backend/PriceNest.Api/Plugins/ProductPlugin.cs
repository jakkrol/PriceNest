using Microsoft.SemanticKernel;
using PriceNest.Api.Services;
using System.ComponentModel;

namespace PriceNest.Api.Plugins;

public class ProductPlugin
{
    private readonly ProductService _productService;

    public ProductPlugin(ProductService productService)
    {
        _productService = productService;
    }

    [KernelFunction]
    [Description("Pobiera informacje o produkcie na podstawie jego numeru ID")]
    public async Task<string> GetProductDetails(
        [Description("Unikalny identyfikator produktu (ID)")] int id)
    {
        var product = await _productService.GetProductByIdAsync(id);

        if (product == null) return "Niestety nie znalazłem produktu o takim ID.";

        return $"Produkt: {product.Name}, Cena: {product.Price} PLN";
    }
}