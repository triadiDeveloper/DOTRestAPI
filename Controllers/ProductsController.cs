using DOTRestAPI.Data;
using DOTRestAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DOTRestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly DOTRestDbContext _context;

        public ProductsController(DOTRestDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        // Eager Loading Category
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _context.Products
                    .Include(p => p.Category)
                    .ToListAsync();

                return Ok(new
                {
                    success = true,
                    message = "Data produk berhasil diambil",
                    data = products
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

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                var product = await _context.Products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"Produk dengan id {id} tidak ditemukan."
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Data produk berhasil diambil",
                    data = product
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

        // POST: api/Products
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            // Validasi ModelState
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Data produk tidak valid",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }

            try
            {
                // Cek CategoryId valid
                var category = await _context.Categories.FindAsync(product.CategoryId);
                if (category == null)
                {
                    ModelState.AddModelError("CategoryId", "CategoryId tidak ditemukan");
                    return BadRequest(new
                    {
                        success = false,
                        message = "Data produk tidak valid",
                        errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                    });
                }

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, new
                {
                    success = true,
                    message = "Produk berhasil disimpan",
                    data = product
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

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Data produk tidak valid",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }

            try
            {
                var existingProduct = await _context.Products.FindAsync(id);
                if (existingProduct == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"Produk dengan id {id} tidak ditemukan."
                    });
                }

                // Cek CategoryId valid
                var category = await _context.Categories.FindAsync(product.CategoryId);
                if (category == null)
                {
                    ModelState.AddModelError("CategoryId", "CategoryId tidak ditemukan");
                    return BadRequest(new
                    {
                        success = false,
                        message = "Data produk tidak valid",
                        errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                    });
                }

                // Update field
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.CategoryId = product.CategoryId;

                _context.Products.Update(existingProduct);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = $"Produk dengan id {id} berhasil di-update",
                    data = existingProduct
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

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"Produk dengan id {id} tidak ditemukan."
                    });
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = $"Produk dengan id {id} berhasil dihapus."
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