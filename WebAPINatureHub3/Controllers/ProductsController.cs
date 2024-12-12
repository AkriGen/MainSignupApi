using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAPINatureHub3.Models;

namespace WebAPINatureHub3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly NatureHub3Context _context;

        public ProductController(NatureHub3Context context)
        {
            _context = context;
        }

        // POST: api/Product/upload
        [HttpPost("upload")]
        public async Task<IActionResult> UploadProductImage(IFormFile file, [FromForm] string productName, [FromForm] decimal price, [FromForm] string description, [FromForm] int stockQuantity, [FromForm] int categoryId, [FromForm] int adminId)
        {
            // Validate if file is provided
            if (file == null || file.Length == 0)
            {
                return BadRequest("No image to upload");
            }

            // Validate productName and other parameters
            if (string.IsNullOrWhiteSpace(productName))
            {
                return BadRequest("Product name is required");
            }

            // Ensure the adminId exists in the Admin table
            var adminExists = await _context.Admins.AnyAsync(a => a.AdminId == adminId);
            if (!adminExists)
            {
                return BadRequest("Invalid Admin ID");
            }

            // Convert the image to byte[]
            byte[] fileData;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileData = memoryStream.ToArray();
            }

            // Create the Product object
            var product = new Product
            {
                ProductName = productName,
                Price = price,
                Description = description,
                StockQuantity = stockQuantity,
                CategoryId = categoryId,
                CreatedByAdminId = adminId, // Use the passed adminId
                Productimg = fileData, // Store the image as byte[] in the database
            };

            // Add the product to the database
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = product.Price,
                ImageUrl = product.Productimg.Length > 0 ? "Profile image uploaded" : "No image uploaded"
            });
        }

        // GET: api/Product/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = product.Price,
                ImageUrl = product.Productimg.Length > 0 ? "Profile image available" : "No image available"
            });
        }

        // GET: api/Product/{id}/image
        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetProductImage(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null || product.Productimg == null)
            {
                return NotFound();
            }

            // Return the image as a byte array
            return File(product.Productimg, "image/jpeg"); // Assuming the image is a JPEG
        }

        // PUT: api/Product/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] string productName, [FromForm] decimal price, [FromForm] string description, [FromForm] int stockQuantity, [FromForm] int categoryId, [FromForm] int adminId, IFormFile? file)
        {
            // Find the product
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Ensure the adminId exists in the Admin table
            var adminExists = await _context.Admins.AnyAsync(a => a.AdminId == adminId);
            if (!adminExists)
            {
                return BadRequest("Invalid Admin ID");
            }

            // Update product fields
            product.ProductName = productName;
            product.Price = price;
            product.Description = description;
            product.StockQuantity = stockQuantity;
            product.CategoryId = categoryId;
            product.CreatedByAdminId = adminId; // Use the passed adminId

            // If a new image is uploaded, update the product image
            if (file != null && file.Length > 0)
            {
                byte[] fileData;
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    fileData = memoryStream.ToArray();
                }
                product.Productimg = fileData; // Update image data
            }

            await _context.SaveChangesAsync();
            return Ok(new { Message = "Product updated successfully!" });
        }

        // DELETE: api/Product/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Product deleted successfully!" });
        }

        // GET: api/Product
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            // Fetch all products from the database
            var products = await _context.Products.ToListAsync();

            if (products == null || products.Count == 0)
            {
                return NotFound("No products found");
            }

            // Return a list of products
            return Ok(products.Select(product => new
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = product.Price,
                StockQty=product.StockQuantity,
                Description=product.Description,
                ImageUrl = product.Productimg.Length > 0 ? "Profile image available" : "No image available"
            }));
        }
    }
}
