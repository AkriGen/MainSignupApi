namespace WebAPINatureHub3.Repos
{
    using Microsoft.EntityFrameworkCore;
    using WebAPINatureHub3.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class RemediesRepository : IRepository<Remedy>
    {
        private readonly NatureHub3Context _context;

        public RemediesRepository(NatureHub3Context context)
        {
            _context = context;
        }

        public IEnumerable<Remedy> GetAll()
        {
            return _context.Remedies.Include(r => r.Category).Include(r => r.CreatedByAdmin).ToList();
        }

        public Remedy GetById(int id)
        {
            return _context.Remedies.Include(r => r.Category).Include(r => r.CreatedByAdmin).FirstOrDefault(r => r.RemedyId == id);
        }

        public void Add(Remedy remedy)
        {
            _context.Remedies.Add(remedy);
            _context.SaveChanges();
        }

        public void Update(Remedy remedy)
        {
            _context.Remedies.Update(remedy);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var remedy = _context.Remedies.Find(id);
            if (remedy != null)
            {
                _context.Remedies.Remove(remedy);
                _context.SaveChanges();
            }
        }
    }
}
