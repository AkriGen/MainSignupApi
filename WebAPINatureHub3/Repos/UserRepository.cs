namespace WebAPINatureHub3.Repos
{
    using WebAPINatureHub3.Models;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public class UserRepository : IRepository<User>
    {
        private readonly NatureHub3Context _context;

        public UserRepository(NatureHub3Context context)
        {
            _context = context;
        }

        // Get all users from the database
        public IEnumerable<User> GetAll()
        {
            return _context.Users
                .Include(u => u.Role)   // Including Role details
                .ToList();
        }

        // Get a user by its ID
        public User GetById(int id)
        {
            return _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.UserId == id);
        }

        // Add a new user
        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        // Update an existing user
        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        // Delete a user by its ID
        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        // Find a user by their email (could be useful for login functionality)
        public User GetByEmail(string email)
        {
            return _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == email);
        }
    }
}
