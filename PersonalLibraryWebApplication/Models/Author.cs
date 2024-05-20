using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PersonalLibraryWebApplication.Models;

public partial class Author
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Поле Ім'я є обов'язковим")]
    [Display(Name = "Ім'я")]
    public string Name { get; set; } = null!;

    [Display(Name = "Про автора")]
    public string? About { get; set; }

    [Display(Name = "Дата народження")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
    public DateOnly? BirthDate { get; set; }

    //Navigation property
    [Display(Name = "Книги")]
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
