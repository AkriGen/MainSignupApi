namespace WebAPINatureHub3.Repos
{
    using Microsoft.EntityFrameworkCore;
    using WebAPINatureHub3.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class AdminRepository : IRepository<Admin>
    {
        private readonly NatureHub3Context _context;

        public AdminRepository(NatureHub3Context context)
        {
            _context = context;
        }

        public IEnumerable<Admin> GetAll()
        {
            return _context.Admins.Include(a => a.Role).ToList();
        }

        public Admin GetById(int id)
        {
            return _context.Admins.Include(a => a.Role).FirstOrDefault(a => a.AdminId == id);
        }

        public void Add(Admin admin)
        {
            _context.Admins.Add(admin);
            _context.SaveChanges();
        }

        public void Update(Admin admin)
        {
            _context.Admins.Update(admin);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var admin = _context.Admins.Find(id);
            if (admin != null)
            {
                _context.Admins.Remove(admin);
                _context.SaveChanges();
            }
        }
    }
}
