using DegirmenciGida.Order.Application.Interfaces;
using DegirmenciGida.Order.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DegirmenciGida.Order.Infrastructure.Services.Product
{
    public class ProductServiceAccessService : IProductServiceAccessService
    {
        private readonly HttpClient _httpClient;

        public ProductServiceAccessService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> CheckProductStockAsync(Guid productId, int quantity)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7106/api/Product/ControllerStockStatus?ProductId={productId}&Quantity={quantity}");

            if (response.IsSuccessStatusCode)
            {
                var isStockController = await response.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(isStockController);
                var dataElement = jsonDocument.RootElement.GetProperty("data");

                bool checkProductStock = dataElement.GetBoolean();

                return checkProductStock;
            }

            return false;
        }

        public async Task<bool> DecreaseStockAsync(Guid productId, int quantity)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7106/api/Product/ControllerStockStatus?ProductId={productId}&Quantity={quantity}");

            if (response.IsSuccessStatusCode)
            {
                var isStockController = await response.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(isStockController);
                var dataElement = jsonDocument.RootElement.GetProperty("data");

                bool isDecreaseStock = dataElement.GetBoolean();

                return isDecreaseStock;
            }

            return false;
        }

        public async Task<ProductResponse> GetProductByIdAsync(Guid productId)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7106/api/Product/{productId}");
            if (response.IsSuccessStatusCode)
            {
                var productJson = await response.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(productJson);
                var dataElement = jsonDocument.RootElement.GetProperty("data");

                var product = JsonSerializer.Deserialize<ProductResponse>(dataElement.ToString(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return product;
            }

            return null;
        }

        public async Task<bool> RestoreStockAsync(Guid productId, int quantity)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7106/api/Product/RestoreStock?ProductId={productId}&Quantity={quantity}");

            if (response.IsSuccessStatusCode)
            {
                var isStockController = await response.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(isStockController);
                var dataElement = jsonDocument.RootElement.GetProperty("data");

                bool isRestoreStockAsync = dataElement.GetBoolean();

                return isRestoreStockAsync;
            }

            return false;
        }
    }
}
