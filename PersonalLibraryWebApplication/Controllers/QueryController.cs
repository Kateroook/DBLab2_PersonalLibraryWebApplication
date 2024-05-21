using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalLibraryWebApplication.Models;
using PersonalLibraryWebApplication.ViewModel;
using System.Security.Policy;
using Publisher = PersonalLibraryWebApplication.Models.Publisher;

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
            var categoryIds = model.SelectedIds;
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

        [HttpGet]
        public IActionResult FilterBooksByAllCategories()
        {
            var model = new QueryViewModel
            {
                AvailableCategories = _context.Categories.ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult FilterBooksByAllCategories(QueryViewModel model)
        {
            var categoryIds = model.SelectedIds;

            if (categoryIds != null && categoryIds.Count > 0)
            {
                string sqlQuery = @"
                    SELECT *
                    FROM Books 
                    WHERE NOT EXISTS (
                        SELECT ID
                        FROM Categories
                        WHERE ID IN ({0})
                        EXCEPT
                        SELECT CategoryId
                        FROM BookCategories 
                        WHERE BookId = Books.ID
                    )";

                string formattedQuery = string.Format(sqlQuery, string.Join(",", categoryIds), categoryIds.Count);
                model.Books = _context.Books.FromSqlRaw(formattedQuery).ToList();
            }
            else
            {
                model.Books = _context.Books.ToList();
            }

            model.AvailableCategories = _context.Categories.ToList();

            return View("FilterBooksByAllCategories", model);
        }

        [HttpGet]
        public IActionResult FilterBooksNotSimilarToSelected()
        {
            var model = new QueryViewModel { AvailableBooks = _context.Books.ToList() };
            return View(model);
        }

        [HttpPost]
        public IActionResult FilterBooksNotSimilarToSelected(QueryViewModel model)
        {
            var bookId = model.SelectedId;

            var sql = @"
            SELECT *
            FROM Books
            WHERE NOT EXISTS (
                SELECT 1
                FROM BookCategories 
                WHERE BookId = Books.ID
                AND CategoryId IN (
                    SELECT CategoryId
                    FROM BookCategories
                    WHERE BookId = {0}
                )
            )";

            var books = _context.Books.FromSqlRaw(sql, bookId).ToList();
            model.Books = books;
            model.AvailableBooks = _context.Books.ToList();

            return View("FilterBooksNotSimilarToSelected", model);
        }
        [HttpGet]
        public IActionResult FilterPublishersByLanguage()
        {
            var model = new QueryViewModel
            {
                AvailableLanguages = _context.Languages.ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult FilterPublishersByLanguage(QueryViewModel model)
        {
            var publishers = new List<Publisher>();

            if (model.SelectedId == 0)
            {
                string sqlQuery = "SELECT * FROM Publishers";
                publishers = _context.Publishers.FromSqlRaw(sqlQuery).ToList();
            }
            else
            {
                string sqlQuery = @"SELECT DISTINCT Publishers.*
                    FROM Publishers, Books, Languages, BookPublishers
                    WHERE Languages.ID = ({0})
	                    AND Languages.ID = Books.LanguageID
	                    AND Books.ID = BookPublishers.BookID
	                    AND Publishers.ID = BookPublishers.PublisherID";
                string formattedQuery = string.Format(sqlQuery, model.SelectedId);
                publishers = _context.Publishers.FromSqlRaw(formattedQuery).ToList();
            }
            model.AvailableLanguages = _context.Languages.ToList();
            model.Publishers = publishers;

            return View("FilterPublishersByLanguage", model);
        }

        [HttpGet]
        public IActionResult FilterPublishersByAuthor()
        {
            var model = new QueryViewModel
            {
                AvailableAuthors = _context.Authors.ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult FilterPublishersByAuthor(QueryViewModel model)
        {
            var selectedAuthorId = model.SelectedId;

            var publishers = new List<Publisher>();
            if (model.SelectedId == 0)
            {
                publishers = _context.Publishers.ToList();
            }
            else
            {
                // SQL запит для отримання авторів, які видають книги в категоріях автора Х
                string sqlQuery = @"
                    SELECT  *
                    FROM Publishers
		            WHERE ID IN(
						SELECT PublisherID FROM BookPublishers
						WHERE BookID IN(
							SELECT ID FROM Books
							WHERE AuthorID = {0}))";

                if (selectedAuthorId > 0)
                {
                    string formattedQuery = string.Format(sqlQuery, selectedAuthorId);
                    publishers = _context.Publishers.FromSqlRaw(formattedQuery).ToList();
                }
            }

            model.Publishers = publishers;
            model.AvailableAuthors = _context.Authors.ToList();
            return View("FilterPublishersByAuthor", model);
        }

        [HttpGet]
        public IActionResult FilterUsersByAuthorInBooklists()
        {
            var model = new QueryViewModel
            {
                AvailableAuthors = _context.Authors.ToList()
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult FilterUsersByAuthorInBooklists(QueryViewModel model)
        {
            var selectedAuthorId = model.SelectedId;
            var users = _context.Users.ToList();

            if (model.SelectedId == 0)
            {
                return NotFound();
            }
            else
            {
                string sqlQuery = @"
                    SELECT DISTINCT u.*
                    FROM Users u
                    JOIN Booklists d ON u.ID = d.UserId
                    JOIN BooksInBooklists kd ON d.Id = kd.BooklistId
                    JOIN Books k ON kd.BookId = k.Id
                    WHERE k.AuthorId = {0}
                ";
                if (selectedAuthorId > 0)
                {
                    string formattedQuery = string.Format(sqlQuery, selectedAuthorId);
                    users = _context.Users.FromSqlRaw(formattedQuery).ToList();
                }
                model.Users = users;
                model.AvailableAuthors = _context.Authors.ToList();
                return View("FilterUsersByAuthorInBooklists", model);
            }
        }

        [HttpGet]
        public IActionResult FilterSimilarBooksInUserBooklist()
        {
            var model = new QueryViewModel
            {
                AvailableUsers = _context.Users.ToList()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult FilterSimilarBooksInUserBooklist(QueryViewModel model)
        {
            var userIds = model.SelectedIds;

            if (userIds != null && userIds.Count > 0)
            {
                string bookIdsSqlQuery = @"
                    SELECT DISTINCT b.ID
                    FROM Books b, BooksInBooklists kd, Booklists d 
                    WHERE b.Id = kd.BookId
                    AND kd.BooklistId = d.Id
                    AND d.UserId IN ({0})
                    AND NOT EXISTS (
                        SELECT 1
                        FROM Booklists d2
                        WHERE d2.UserId NOT IN ({0})
                        AND NOT EXISTS (
                            SELECT 1
                            FROM BooksInBooklists kd2
                            WHERE kd2.BooklistId = d2.Id
                            AND kd2.BookId = b.Id
                        )
                    )";

                string formattedQuery = string.Format(bookIdsSqlQuery, string.Join(",", userIds));
                
                var bookIds = _context.Books.FromSqlRaw(formattedQuery)
                    .Select(b => b.Id)
                    .ToList();
                var books = _context.Books
                    .Where(b => bookIds.Contains(b.Id))
                    .ToList();
                model.Books = books;
            }
            else
            {
                model.Books = _context.Books.ToList();
            }

            model.AvailableUsers = _context.Users.ToList();

            return View("FilterSimilarBooksInUserBooklist", model);
        }

        [HttpGet]
        public IActionResult FilterAuthorsByCategoriesOfAuthor()
        {
            var model = new QueryViewModel { AvailableAuthors = _context.Authors.ToList() };
            return View(model);
        }


        [HttpPost]
        public IActionResult FilterAuthorsByCategoriesOfAuthor(QueryViewModel model)
        {
            var selectedAuthorId = model.SelectedId;
            var authors = new List<Author>();

            if (model.SelectedId == 0)
            {
                authors = _context.Authors.ToList();
            }
            else
            {

                string sqlQuery = @"
            SELECT DISTINCT a.ID
            FROM Authors a, Books b
            WHERE a.Id = b.AuthorId           
            AND EXISTS (
                SELECT  1
                FROM BookCategories bc
                WHERE bc.BookId = b.Id
                AND bc.CategoryId IN (
                    SELECT bc2.CategoryId
                    FROM BookCategories bc2, Books b2
                    WHERE bc2.BookId = b2.Id
                    AND b2.AuthorId = {0}
                )
            )
            AND a.Id <> {0}";

                if (selectedAuthorId > 0)
                {
                    string formattedQuery = string.Format(sqlQuery, selectedAuthorId);
                    var authorsIds = _context.Authors.FromSqlRaw(formattedQuery)
                        .Select(a => a.Id)
                        .ToList();

                    authors = _context.Authors
                        .Where(b => authorsIds.Contains(b.Id))
                        .ToList();                    
                }
            }
            model.Authors = authors;
            model.AvailableAuthors = _context.Authors.ToList();

            return View("FilterAuthorsByCategoriesOfAuthor", model);
        }
    }
}