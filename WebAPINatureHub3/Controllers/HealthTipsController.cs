using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPINatureHub3.Models;

namespace WebAPINatureHub3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthTipsController : ControllerBase
    {
        private readonly NatureHub3Context _context;

        public HealthTipsController(NatureHub3Context context)
        {
            _context = context;
        }

        // GET: api/HealthTips
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HealthTip>>> GetHealthTips()
        {
            return await _context.HealthTips.ToListAsync();
        }

        // GET: api/HealthTips/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HealthTip>> GetHealthTip(int id)
        {
            var healthTip = await _context.HealthTips.FindAsync(id);

            if (healthTip == null)
            {
                return NotFound();
            }

            return healthTip;
        }

        // GET: api/HealthTips/{id}/image
        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetHealthTipImage(int id)
        {
            var healthTip = await _context.HealthTips.FindAsync(id);

            if (healthTip == null || healthTip.HealthTipsimg == null)
            {
                return NotFound("Image not found");
            }

            return File(healthTip.HealthTipsimg, "image/jpeg"); // Assuming the image is a JPEG
        }

        // PUT: api/HealthTips/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHealthTip(int id, [FromForm] string tipTitle, [FromForm] string? tipDescription, [FromForm] int categoryId, [FromForm] int createdByAdminId, IFormFile? image)
        {
            var healthTip = await _context.HealthTips.FindAsync(id);

            if (healthTip == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(tipTitle))
            {
                return BadRequest("Tip title is required");
            }

            healthTip.TipTitle = tipTitle;
            healthTip.TipDescription = tipDescription;
            healthTip.CategoryId = categoryId;
            healthTip.CreatedByAdminId = createdByAdminId;

            if (image != null && image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    healthTip.HealthTipsimg = memoryStream.ToArray();
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HealthTipExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/HealthTips
        [HttpPost]
        public async Task<ActionResult<HealthTip>> PostHealthTip([FromForm] string tipTitle, [FromForm] string? tipDescription, [FromForm] int categoryId, [FromForm] int createdByAdminId, IFormFile? image)
        {
            if (string.IsNullOrWhiteSpace(tipTitle))
            {
                return BadRequest("Tip title is required");
            }

            byte[]? imageData = null;
            if (image != null && image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    imageData = memoryStream.ToArray();
                }
            }

            var healthTip = new HealthTip
            {
                TipTitle = tipTitle,
                TipDescription = tipDescription,
                CategoryId = categoryId,
                CreatedByAdminId = createdByAdminId,
                
                HealthTipsimg = imageData
            };

            _context.HealthTips.Add(healthTip);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHealthTip", new { id = healthTip.TipId }, healthTip);
        }

        // DELETE: api/HealthTips/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHealthTip(int id)
        {
            var healthTip = await _context.HealthTips.FindAsync(id);
            if (healthTip == null)
            {
                return NotFound();
            }

            _context.HealthTips.Remove(healthTip);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HealthTipExists(int id)
        {
            return _context.HealthTips.Any(e => e.TipId == id);
        }
    }
}
