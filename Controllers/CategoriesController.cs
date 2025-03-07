using DOTRestAPI.Data;
using DOTRestAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DOTRestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly DOTRestDbContext _context;

        public CategoriesController(DOTRestDbContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        // Contoh Eager Loading semua Product pada setiap Category
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _context.Categories
                    .Include(c => c.Products) // Eager loading
                    .ToListAsync();

                // Berhasil
                return Ok(new
                {
                    success = true,
                    message = "Data kategori berhasil diambil",
                    data = categories
                });
            }
            catch (Exception ex)
            {
                // Gagal
                return StatusCode(500, new
                {
                    success = false,
                    message = $"Terjadi kesalahan: {ex.Message}"
                });
            }
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            try
            {
                var category = await _context.Categories
                    .Include(c => c.Products)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (category == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"Category dengan id {id} tidak ditemukan."
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Data kategori berhasil diambil",
                    data = category
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = $"Terjadi kesalahan: {ex.Message}"
                });
            }
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            // Validasi model
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Data kategori tidak valid",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }

            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                // Mengembalikan status 201 (Created)
                // plus data yang baru dibuat.
                // Kita bisa pakai CreatedAtAction, atau sekadar return Ok. Terserah style API Anda.
                return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, new
                {
                    success = true,
                    message = "Category berhasil disimpan",
                    data = category
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = $"Terjadi kesalahan saat menyimpan data: {ex.Message}"
                });
            }
        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Data kategori tidak valid",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }

            try
            {
                var existingCategory = await _context.Categories.FindAsync(id);
                if (existingCategory == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"Category dengan id {id} tidak ditemukan."
                    });
                }

                // Update field
                existingCategory.Name = category.Name;

                _context.Categories.Update(existingCategory);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = $"Category dengan id {id} berhasil di-update.",
                    data = existingCategory
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = $"Terjadi kesalahan saat update data: {ex.Message}"
                });
            }
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"Category dengan id {id} tidak ditemukan."
                    });
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = $"Category dengan id {id} berhasil dihapus."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = $"Terjadi kesalahan saat menghapus data: {ex.Message}"
                });
            }
        }
    }
}