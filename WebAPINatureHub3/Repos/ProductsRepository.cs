using Microsoft.EntityFrameworkCore;
using WebAPINatureHub3.Models;

namespace WebAPINatureHub3.Repos
{
    public class ProductsRepository : IRepository<Product>
    {
        private readonly NatureHub3Context _context;

        public ProductsRepository(NatureHub3Context context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products.Include(p => p.Category).Include(p => p.CreatedByAdmin).ToList();
        }

        public Product GetById(int id)
        {
            return _context.Products.Include(p => p.Category).Include(p => p.CreatedByAdmin).FirstOrDefault(p => p.ProductId == id);
        }

        public void Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}
