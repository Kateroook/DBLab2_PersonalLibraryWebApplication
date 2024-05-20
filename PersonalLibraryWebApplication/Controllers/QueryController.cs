using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalLibraryWebApplication.Models;
using PersonalLibraryWebApplication.ViewModel;

namespace PersonalLibraryWebApplication.Controllers
{
    public class QueryController : Controller
    {
        private readonly DbpersonalLibraryContext _context;
        public QueryController(DbpersonalLibraryContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult FilterBooksByCategories()
        {
            var model = new QueryViewModel
            {
                AvailableCategories = _context.Categories.ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult FilterBooksByCategories(QueryViewModel model)
        {
            var categoryIds = model.SelectedCategoryIds;
            var books = new List<Book>();
            if (categoryIds != null && categoryIds.Count > 0)
            {
                string sqlQuery = @"
                    SELECT * FROM Books b
                    WHERE EXISTS(
                        SELECT 1 
                        FROM BookCategories bc 
                        WHERE b.Id = bc.BookId 
                        AND bc.CategoryId IN ({0})
                    )";

                string formattedQuery = string.Format(sqlQuery, string.Join(",", categoryIds));
                books = _context.Books.FromSqlRaw(formattedQuery).ToList();
            }
            else
            {
                books = _context.Books.ToList();
            }

            model.AvailableCategories = _context.Categories.ToList();
            model.Books = books;

            return View("FilterBooksByCategories", model);
        }

        
    }
}
