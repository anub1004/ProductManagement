using ProductManagement.DTOs;
using ProductManagement.Models;

namespace ProductManagement.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(string id);
        Task AddAsync(Product product);
        Task SaveChangesAsync();
        Task UpdateProductAsync(string id, Product productDto);
        Task DeleteProductAsync(string id, Product productReaddto);
        Task AddRangeAsync(List<Product> products);
    }
}
