namespace WebAPINatureHub3.Repos
{
    using Microsoft.EntityFrameworkCore;
    using WebAPINatureHub3.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class CartRepository : IRepository<Cart>
    {
        private readonly NatureHub3Context _context;

        public CartRepository(NatureHub3Context context)
        {
            _context = context;
        }

        public IEnumerable<Cart> GetAll()
        {
            return _context.Carts.Include(c => c.User).Include(c => c.Product).ToList();
        }

        public Cart GetById(int id)
        {
            return _context.Carts.Include(c => c.User).Include(c => c.Product).FirstOrDefault(c => c.CartId == id);
        }

        public void Add(Cart cart)
        {
            _context.Carts.Add(cart);
            _context.SaveChanges();
        }

        public void Update(Cart cart)
        {
            _context.Carts.Update(cart);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var cart = _context.Carts.Find(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
                _context.SaveChanges();
            }
        }
    }
}
