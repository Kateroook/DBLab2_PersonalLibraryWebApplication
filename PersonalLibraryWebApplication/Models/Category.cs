using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PersonalLibraryWebApplication.Models;

public partial class Category
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Поле Назва є обов'язковим.")]
    [Display(Name = "Назва")]
    public string Title { get; set; } = null!;

    [Display(Name = "Опис")]
    public string? Description { get; set; }

    //Navigation property
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
