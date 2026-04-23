using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace ProductManagement.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }= string.Empty;
        [Required]
        [StringLength(50)]                                                     
        public decimal Price { get; set; }
        [Required, MaxLength(50)]
        public string Description { get; set; } = string.Empty;
        [Required, MaxLength(50)]
        public int Stock { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
