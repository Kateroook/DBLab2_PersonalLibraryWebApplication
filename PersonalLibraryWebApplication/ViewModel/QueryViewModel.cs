using PersonalLibraryWebApplication.Models;

namespace PersonalLibraryWebApplication.ViewModel
{
    public class QueryViewModel
    {
        public List<int> SelectedCategoryIds { get; set; } = new List<int>();
        public List<Category> AvailableCategories { get; set; } = new List<Category>();
        public List<Book> Books { get; set; }


    }
}
