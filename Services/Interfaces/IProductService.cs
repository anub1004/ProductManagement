using ProductManagement.DTOs;

namespace ProductManagement.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductReaddto>> GetAllProductsAsync();

        Task<ProductReaddto?> GetProductByIdAsync(string id);
        Task<string> CreateProductAsync(ProductCreatedto productDto);
        Task UpdateProduct(string id, ProductUpdatedto productDto);
        Task DeleteProductAsync(string id) ;
        Task<object> ProcessCsvAsync(IFormFile file);
    }
}   
