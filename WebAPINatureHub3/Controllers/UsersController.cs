using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPINatureHub3.Models;

namespace WebAPINatureHub3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        private readonly NatureHub3Context _context;

        public UsersController(NatureHub3Context context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
        [HttpGet("by-username/{username}")]
        public async Task<ActionResult<User>> GetUser(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u=> u.UserName == username);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // GET: api/Users/{id}/image
        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetUserImage(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null || user.UserImage == null)
            {
                return NotFound("Image not found");
            }

            return File(user.UserImage, "image/jpeg"); // Assuming the image is a JPEG
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, [FromForm] string userName, [FromForm] string email, [FromForm] string password, [FromForm] int roleId, IFormFile? image)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.UserName = userName;
            user.Email = email;
            user.Password = password;
            user.RoleId = roleId;

            if (image != null && image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    user.UserImage = memoryStream.ToArray();
                }
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users

        [HttpPost]

        public async Task<ActionResult<User>> PostUser([FromForm] string userName, [FromForm] string email, [FromForm] string password, [FromForm] int roleId, IFormFile? image)
        {
            byte[]? imageData = null;

            if (image != null && image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    imageData = memoryStream.ToArray();
                }
            }

            var user = new User
            {
                UserName = userName,
                Email = email,
                Password = password,
                RoleId = roleId,
                UserImage = imageData
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
