using Microsoft.EntityFrameworkCore;
using ProductManagement.Data;
using ProductManagement.DTOs;
using ProductManagement.Models;
using ProductManagement.Repositories.Interfaces;

namespace ProductManagement.Repositories.Implementation
{
    public class ProductRepository: IProductRepository
    {  private readonly AppDbContext _context;      
        public ProductRepository(AppDbContext context) { 
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await  _context.Products.ToListAsync();
        }
        public async Task<Product?> GetProductByIdAsync(string id)
        {
            return await _context.Products.FindAsync(id);
        }
        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }
  
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(string id, Models.Product productDto)
        {
            _context.Products.Update(productDto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(string id, Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        public async Task AddRangeAsync(List<Product> products)
        {
            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();
        }
    }
}
