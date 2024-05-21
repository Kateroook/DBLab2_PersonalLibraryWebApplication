using PersonalLibraryWebApplication.Models;

namespace PersonalLibraryWebApplication.ViewModel
{
    public class QueryViewModel
    {
        public int SelectedId { get; set; }
        public List<int> SelectedIds { get; set; } = new List<int>();
        public List<Category> AvailableCategories { get; set; } = new List<Category>();       
        public List<Language> AvailableLanguages { get; set; } = new List<Language>();
        public List<Author> AvailableAuthors{ get; set; } = new List<Author>();
        public List<Book> AvailableBooks { get; set; } = new List<Book>();

        public List<Book> Books { get; set; }
        public List<Publisher> Publishers { get; set; }
    }
}
