using System.ComponentModel.DataAnnotations;

namespace DOTRestAPI.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nama kategori wajib diisi")]
        [StringLength(100, ErrorMessage = "Panjang nama kategori maksimal 100 karakter")]
        public string Name { get; set; } = default!;

        // Contoh Relationship One-to-Many: Satu Category bisa memiliki banyak Products
        // Jika ingin menggunakan lazy loading, pastikan menambahkan 'virtual'.
        public virtual ICollection<Product>? Products { get; set; }
    }
}
