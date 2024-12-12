namespace WebAPINatureHub3.Repos
{
    using WebAPINatureHub3.Models;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public class BookmarkRepository : IRepository<Bookmark>
    {
        private readonly NatureHub3Context _context;

        public BookmarkRepository(NatureHub3Context context)
        {
            _context = context;
        }

        // Get all bookmarks from the database
        public IEnumerable<Bookmark> GetAll()
        {
            return _context.Bookmarks
                .Include(b => b.User)     // Including User details associated with the bookmark
                .Include(b => b.Remedy)   // Including Remedy details associated with the bookmark
                .ToList();
        }

        // Get a bookmark by its ID
        public Bookmark GetById(int id)
        {
            return _context.Bookmarks
                .Include(b => b.User)     // Including User details
                .Include(b => b.Remedy)   // Including Remedy details
                .FirstOrDefault(b => b.BookmarkId == id);
        }

        // Add a new bookmark
        public void Add(Bookmark bookmark)
        {
            _context.Bookmarks.Add(bookmark);
            _context.SaveChanges();
        }

        // Update an existing bookmark
        public void Update(Bookmark bookmark)
        {
            _context.Bookmarks.Update(bookmark);
            _context.SaveChanges();
        }

        // Delete a bookmark by its ID
        public void Delete(int id)
        {
            var bookmark = _context.Bookmarks.Find(id);
            if (bookmark != null)
            {
                _context.Bookmarks.Remove(bookmark);
                _context.SaveChanges();
            }
        }

        // Get all bookmarks for a specific user
        public IEnumerable<Bookmark> GetByUserId(int userId)
        {
            return _context.Bookmarks
                .Where(b => b.UserId == userId)
                .Include(b => b.User)     // Including User details
                .Include(b => b.Remedy)   // Including Remedy details
                .ToList();
        }

        // Get all bookmarks for a specific remedy
        public IEnumerable<Bookmark> GetByRemedyId(int remedyId)
        {
            return _context.Bookmarks
                .Where(b => b.RemedyId == remedyId)
                .Include(b => b.User)     // Including User details
                .Include(b => b.Remedy)   // Including Remedy details
                .ToList();
        }
    }
}
