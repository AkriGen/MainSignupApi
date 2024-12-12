namespace WebAPINatureHub3.Repos
{
    using Microsoft.EntityFrameworkCore;
    using WebAPINatureHub3.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class HealthTipsRepository : IRepository<HealthTip>
    {
        private readonly NatureHub3Context _context;

        public HealthTipsRepository(NatureHub3Context context)
        {
            _context = context;
        }

        public IEnumerable<HealthTip> GetAll()
        {
            return _context.HealthTips.Include(h => h.Category).Include(h => h.CreatedByAdmin).ToList();
        }

        public HealthTip GetById(int id)
        {
            return _context.HealthTips.Include(h => h.Category).Include(h => h.CreatedByAdmin).FirstOrDefault(h => h.TipId == id);
        }

        public void Add(HealthTip healthTip)
        {
            _context.HealthTips.Add(healthTip);
            _context.SaveChanges();
        }

        public void Update(HealthTip healthTip)
        {
            _context.HealthTips.Update(healthTip);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var healthTip = _context.HealthTips.Find(id);
            if (healthTip != null)
            {
                _context.HealthTips.Remove(healthTip);
                _context.SaveChanges();
            }
        }
    }
}
