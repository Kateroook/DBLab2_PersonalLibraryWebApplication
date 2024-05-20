using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PersonalLibraryWebApplication.Models;

public partial class BooksInBooklist
{
    [Display(Name = "Добірка")]
    public int BooklistId { get; set; }

    [Display(Name = "Книга")]
    public int BookId { get; set; }

    [Display(Name = "Додано книгу о")]
    [DataType(DataType.Date)]
    public DateTime BookAddedAt { get; set; }

    //Navigation property
    public virtual Book Book { get; set; } = null!;

    public virtual Booklist Booklist { get; set; } = null!;
}
