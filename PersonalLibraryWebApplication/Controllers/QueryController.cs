﻿using Microsoft.AspNetCore.Mvc;
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


    }
}
