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
    public class RemediesController : ControllerBase
    {
        private readonly NatureHub3Context _context;

        public RemediesController(NatureHub3Context context)
        {
            _context = context;
        }

        // POST: api/Remedies/upload
        [HttpPost("upload")]
        public async Task<IActionResult> UploadRemedyImage(IFormFile file, [FromForm] string remedyName, [FromForm] string description, [FromForm] int categoryId, [FromForm] int adminId, [FromForm] string? benefits, [FromForm] string? preparationMethod, [FromForm] string? usageInstructions)
        {
            // Validate if file is provided
            if (file == null || file.Length == 0)
            {
                return BadRequest("No image to upload");
            }

            // Validate remedyName and other parameters
            if (string.IsNullOrWhiteSpace(remedyName))
            {
                return BadRequest("Remedy name is required");
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

            // Create the Remedy object
            var remedy = new Remedy
            {
                RemedyName = remedyName,
                Description = description,
                CategoryId = categoryId,
                CreatedByAdminId = adminId, // Use the passed adminId
                Remediesimg = fileData, // Store the image as byte[] in the database
                Benefits = benefits, // Store the benefits
                PreparationMethod = preparationMethod, // Store the preparation method
                UsageInstructions = usageInstructions // Store the usage instructions
            };

            // Add the remedy to the database
            _context.Remedies.Add(remedy);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                RemedyId = remedy.RemedyId,
                RemedyName = remedy.RemedyName,
                Description = remedy.Description,
                Benefits = remedy.Benefits,
                PreparationMethod = remedy.PreparationMethod,
                UsageInstructions = remedy.UsageInstructions,
                ImageUrl = remedy.Remediesimg.Length > 0 ? "Remedy image uploaded" : "No image uploaded"
            });
        }

        // GET: api/Remedies/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRemedy(int id)
        {
            var remedy = await _context.Remedies.FindAsync(id);
            if (remedy == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                RemedyId = remedy.RemedyId,
                RemedyName = remedy.RemedyName,
                Description = remedy.Description,
                Benefits = remedy.Benefits,
                PreparationMethod = remedy.PreparationMethod,
                UsageInstructions = remedy.UsageInstructions,
                ImageUrl = remedy.Remediesimg.Length > 0 ? "Remedy image available" : "No image available"
            });
        }

        // GET: api/Remedies/{id}/image
        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetRemedyImage(int id)
        {
            var remedy = await _context.Remedies.FindAsync(id);
            if (remedy == null || remedy.Remediesimg == null)
            {
                return NotFound();
            }

            // Return the image as a byte array
            return File(remedy.Remediesimg, "image/jpeg"); // Assuming the image is a JPEG
        }

        // PUT: api/Remedies/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRemedy(int id, [FromForm] string remedyName, [FromForm] string description, [FromForm] int categoryId, [FromForm] int adminId, IFormFile? file, [FromForm] string? benefits, [FromForm] string? preparationMethod, [FromForm] string? usageInstructions)
        {
            var remedy = await _context.Remedies.FindAsync(id);
            if (remedy == null)
            {
                return NotFound();
            }

            // Ensure the adminId exists in the Admin table
            var adminExists = await _context.Admins.AnyAsync(a => a.AdminId == adminId);
            if (!adminExists)
            {
                return BadRequest("Invalid Admin ID");
            }

            // Update remedy fields
            remedy.RemedyName = remedyName;
            remedy.Description = description;
            remedy.CategoryId = categoryId;
            remedy.CreatedByAdminId = adminId; // Use the passed adminId
            remedy.Benefits = benefits; // Update benefits
            remedy.PreparationMethod = preparationMethod; // Update preparation method
            remedy.UsageInstructions = usageInstructions; // Update usage instructions

            // If a new image is uploaded, update the remedy image
            if (file != null && file.Length > 0)
            {
                byte[] fileData;
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    fileData = memoryStream.ToArray();
                }
                remedy.Remediesimg = fileData; // Update image data
            }

            await _context.SaveChangesAsync();
            return Ok(new { Message = "Remedy updated successfully!" });
        }

        // DELETE: api/Remedies/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRemedy(int id)
        {
            var remedy = await _context.Remedies.FindAsync(id);
            if (remedy == null)
            {
                return NotFound();
            }

            _context.Remedies.Remove(remedy);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Remedy deleted successfully!" });
        }

        // GET: api/Remedies
        [HttpGet]
        public async Task<IActionResult> GetAllRemedies()
        {
            // Fetch all remedies from the database
            var remedies = await _context.Remedies.ToListAsync();

            if (remedies == null || remedies.Count == 0)
            {
                return NotFound("No remedies found");
            }

            // Return a list of remedies
            return Ok(remedies.Select(remedy => new
            {
                RemedyId = remedy.RemedyId,
                RemedyName = remedy.RemedyName,
                Description = remedy.Description,
                Benefits = remedy.Benefits,
                PreparationMethod = remedy.PreparationMethod,
                UsageInstructions = remedy.UsageInstructions,
                ImageUrl = remedy.Remediesimg.Length > 0 ? "Remedy image available" : "No image available"
            }));
        }
    }
}
