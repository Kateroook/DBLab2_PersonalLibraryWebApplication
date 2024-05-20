using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PersonalLibraryWebApplication.Models;

namespace PersonalLibraryWebApplication.Controllers
{
    public class BooksController : Controller
    {
        private readonly DbpersonalLibraryContext _context;

        public BooksController(DbpersonalLibraryContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var dbpersonalLibraryContext = _context.Books.Include(b => b.Author).Include(b => b.Language);
            return View(await dbpersonalLibraryContext.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Language)
                .Include(b => b.Categories)
                .Include(b => b.Publishers)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name");
            ViewData["LanguageId"] = new SelectList(_context.Languages, "Id", "Name");
            ViewData["CategoryIds"] = new SelectList(_context.Categories, "Id", "Title");
            ViewData["PublisherIds"] = new SelectList(_context.Publishers, "Id", "Name");

            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AuthorId,Title,BookCover,LanguageId,PublishingYear,Description,Pages")] Book book, int[] categories, int[] publishers)
        {
            Author author = await _context.Authors.FindAsync(book.AuthorId);
            book.Author = author;
            Language language = await _context.Languages.FindAsync(book.LanguageId);
            book.Language = language;
            ModelState.Clear();
            TryValidateModel(book);

            if (ModelState.IsValid)
            {
                _context.Add(book);
                foreach (var categoryId in categories)
                {
                    var bookCategory = _context.Categories.Find(categoryId);
                    if (bookCategory != null)
                        @book.Categories.Add(bookCategory);
                }

                foreach (var publisherId in publishers)
                {
                    var bookPublisher = _context.Publishers.Find(publisherId);
                    if (bookPublisher != null)
                        @book.Publishers.Add(bookPublisher);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", book.AuthorId);
            ViewData["LanguageId"] = new SelectList(_context.Languages, "Id", "Name", book.LanguageId);
            ViewData["CategoryIds"] = new SelectList(_context.Categories, "Id", "Title", book.Categories);
            ViewData["PublisherIds"] = new SelectList(_context.Publishers, "Id", "Name", book.Publishers);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(e => e.Language)
                .Include(e => e.Author)
                .Include(e => e.Categories)
                .Include(e => e.Publishers)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            ViewBag.SelectedCategories = book.Categories.Select(i => i.Id).ToList();
            ViewBag.SelectedPublishers = book.Publishers.Select(i => i.Id).ToList();

            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", book.AuthorId);
            ViewData["LanguageId"] = new SelectList(_context.Languages, "Id", "Name", book.LanguageId);
            ViewData["CategoryIds"] = new MultiSelectList(_context.Categories, "Id", "Title");
            ViewData["PublisherIds"] = new MultiSelectList(_context.Publishers, "Id", "Name");
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AuthorId,Title,BookCover,LanguageId,PublishingYear,Description,Pages")] Book book, int[] categories, int[] publishers)
        {
            Author author = await _context.Authors.FindAsync(book.AuthorId);
            book.Author = author;
            Language language = await _context.Languages.FindAsync(book.LanguageId);
            book.Language = language;
            ModelState.Clear();
            TryValidateModel(book);

            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingBook = await _context.Books
                        .Include(e => e.Categories)
                        .Include(e => e.Publishers)
                        .FirstOrDefaultAsync(e => e.Id == id);
                    if (existingBook == null)
                    {
                        return NotFound();
                    }


                    existingBook.AuthorId = book.AuthorId;
                    existingBook.Title = book.Title;
                    existingBook.BookCover = book.BookCover;
                    existingBook.LanguageId = book.LanguageId;
                    existingBook.PublishingYear = book.PublishingYear;
                    existingBook.Description = book.Description;
                    existingBook.Pages = book.Pages;

                    existingBook.Categories.Clear();
                    foreach (var categoryId in categories)
                    {
                        var category = await _context.Categories.FindAsync(categoryId);
                        if (category != null)
                        {
                            existingBook.Categories.Add(category);
                        }
                    }

                    existingBook.Publishers.Clear();
                    foreach (var publisherId in publishers)
                    {
                        var publisher = await _context.Publishers.FindAsync(publisherId);
                        if (publisher != null)
                        {
                            existingBook.Publishers.Add(publisher);
                        }
                    }


                    _context.Update(existingBook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SelectedCategories = categories.ToList();
            ViewBag.SelectedPublishers = publishers.ToList();
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Id", book.AuthorId);
            ViewData["LanguageId"] = new SelectList(_context.Languages, "Id", "Id", book.LanguageId);
            ViewData["CategoryIds"] = new SelectList(_context.Categories, "Id", "Title", book.Categories);
            ViewData["PublisherIds"] = new SelectList(_context.Publishers, "Id", "Name", book.Publishers);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Language)
                .Include(b => b.Categories)
                .Include(b => b.Publishers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books
                .Include(b => b.Categories)
                .Include(b => b.Publishers)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            var categoriesToRemove = book.Categories.ToList();
            foreach (var category in categoriesToRemove)
            {
                book.Categories.Remove(category);
            }
            var publishersToRemove = book.Publishers.ToList();
            foreach (var publisher in publishersToRemove)
            {
                book.Publishers.Remove(publisher);
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
