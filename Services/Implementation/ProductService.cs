using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.DTOs;
using ProductManagement.Models;
using ProductManagement.Repositories.Implementation;
using ProductManagement.Repositories.Interfaces;
using ProductManagement.Services.Interfaces;
using System.Globalization;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace ProductManagement.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<ProductReaddto>> GetAllProductsAsync()
        {
            var products = await _repository.GetAllProductsAsync();
            return products.Select(p => new ProductReaddto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                CreatedAt = p.CreatedAt
            }).ToList();
        }
        public async Task<ProductReaddto?> GetProductByIdAsync(string id)
        {
            var product = await _repository.GetProductByIdAsync(id);
            if (product == null)
            {
                return null;
            }
            return new ProductReaddto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                CreatedAt = product.CreatedAt
            };
        }
        public async Task<string> CreateProductAsync(ProductCreatedto productDto)
        {
            var product = new Models.Product
            {
                Id = Guid.NewGuid().ToString(),
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Stock = productDto.Stock,
                CreatedAt = DateTime.UtcNow
            };
            await _repository.AddAsync(product);
            return product.Id;
        }
        
       

        public async Task UpdateProduct(string id, ProductUpdatedto productDto)
        {
            var existingProduct = await _repository.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                throw new Exception("Product not found");
            }
            existingProduct.Name = productDto.Name;
            existingProduct.Description = productDto.Description;
            existingProduct.Price = productDto.Price;
            existingProduct.Stock = productDto.Stock;
            await _repository.UpdateProductAsync(id, existingProduct);
        }

        public async Task DeleteProductAsync(string id)
        {
           var existingProduct = await _repository.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                throw new Exception("Product not found");
            }
             await _repository.DeleteProductAsync(id, existingProduct);
        
        }
        public async Task<object> ProcessCsvAsync(IFormFile file)
        {
            using var stream = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(stream, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<ProductcsvModel>().ToList();

          
            var (validData, errors) = ValidateCsv(records);

        
            var products = validData.Select(x => new Product
            {
                Id = Guid.NewGuid().ToString(),
                Name = x.Name,
                Price = x.Price,
                Stock = x.Stock,
                Description = "Imported from CSV",
                CreatedAt = DateTime.UtcNow
            }).ToList();

            await _repository.AddRangeAsync(products);

            return new
            {
                success = true,
                inserted = products.Count,
                failed = errors.Count,
                errors = errors
            };
        }

        private static (List<ProductcsvModel> validData, List<string> errors) ValidateCsv(List<ProductcsvModel> records)
        {
            var validData = new List<ProductcsvModel>();
            var errors = new List<string>();

            for (var i = 0; i < records.Count; i++)
            {
                var record = records[i];
                var rowNumber = i + 1;
                var hasError = false;

                if (string.IsNullOrWhiteSpace(record.Name))
                {
                    errors.Add($"Row {rowNumber}: Name is required.");
                    hasError = true;
                }

                if (record.Price < 0)
                {
                    errors.Add($"Row {rowNumber}: Price must be non-negative.");
                    hasError = true;
                }

                if (record.Stock < 0)
                {
                    errors.Add($"Row {rowNumber}: Stock must be non-negative.");
                    hasError = true;
                }

                if (!hasError)
                {
                    validData.Add(record);
                }
            }

            return (validData, errors);
        }

        Task<object> IProductService.ProcessCsvAsync(IFormFile file)
        {
            return ProcessCsvAsync(file);
        }
    }
}
