using System.ComponentModel.DataAnnotations;

namespace DOTRestAPI.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nama produk wajib diisi")]
        [StringLength(100, ErrorMessage = "Panjang nama produk maksimal 100 karakter")]
        public string Name { get; set; } = default!;

        [Range(0, int.MaxValue, ErrorMessage = "Harga minimal 0")]
        public decimal Price { get; set; }

        // Foreign Key
        [Required(ErrorMessage = "CategoryId wajib diisi")]
        public int CategoryId { get; set; }

        // Navigation Property
        // Tambahkan 'virtual' untuk lazy loading
        public virtual Category? Category { get; set; }
    }
}
