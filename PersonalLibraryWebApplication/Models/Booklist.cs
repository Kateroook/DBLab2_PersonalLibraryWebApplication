using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PersonalLibraryWebApplication.Models;

public partial class Booklist
{
    public int Id { get; set; }

    [Display(Name = "Користувач")]
    public int UserId { get; set; }

    [Display(Name = "Назва")]
    public string Title { get; set; } = null!;

    [Display(Name = "Опис")]
    public string? Description { get; set; }

    [Display(Name = "Дата створення")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateOnly CreatedAt { get; set; }

    //Navigation property
    public virtual ICollection<BooksInBooklist> BooksInBooklists { get; set; } = new List<BooksInBooklist>();

    public virtual User User { get; set; } = null!;
}
