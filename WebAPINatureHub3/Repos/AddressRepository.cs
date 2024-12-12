namespace WebAPINatureHub3.Repos
{
    using Microsoft.EntityFrameworkCore;
    using WebAPINatureHub3.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class AddressRepository :IRepository<Address>
    {
        private readonly NatureHub3Context _context;

        public AddressRepository(NatureHub3Context context)
        {
            _context = context;
        }

        public IEnumerable<Address> GetAll()
        {
            return _context.Addresses.Include(a => a.User).ToList();
        }

        public Address GetById(int id)
        {
            return _context.Addresses.Include(a => a.User).FirstOrDefault(a => a.AddressId == id);
        }

        public void Add(Address address)
        {
            _context.Addresses.Add(address);
            _context.SaveChanges();
        }

        public void Update(Address address)
        {
            _context.Addresses.Update(address);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var address = _context.Addresses.Find(id);
            if (address != null)
            {
                _context.Addresses.Remove(address);
                _context.SaveChanges();
            }
        }
    }
}
