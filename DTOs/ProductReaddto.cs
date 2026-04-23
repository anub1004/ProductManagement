namespace ProductManagement.DTOs
{
    public class ProductReaddto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; }= string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
