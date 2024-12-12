namespace WebAPINatureHub3.Repos
{
    using WebAPINatureHub3.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class RolesRepository : IRepository<Role>
    {
        private readonly NatureHub3Context _context;

        public RolesRepository(NatureHub3Context context)
        {
            _context = context;
        }

        // Get all roles from the database
        public IEnumerable<Role> GetAll()
        {
            return _context.Roles.ToList();
        }

        // Get a role by its ID
        public Role GetById(int id)
        {
            return _context.Roles.FirstOrDefault(r => r.RoleId == id);
        }

        // Add a new role
        public void Add(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
        }

        // Update an existing role
        public void Update(Role role)
        {
            _context.Roles.Update(role);
            _context.SaveChanges();
        }

        // Delete a role by its ID
        public void Delete(int id)
        {
            var role = _context.Roles.Find(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
                _context.SaveChanges();
            }
        }
    }
}
