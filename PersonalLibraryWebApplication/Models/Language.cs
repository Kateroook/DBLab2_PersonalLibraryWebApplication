using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PersonalLibraryWebApplication.Models;

public partial class Language
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Поле Мова є обов'язковим.")]
    [Display(Name = "Мова")]
    public string Name { get; set; } = null!;

    //Navigation property
    [Display(Name = "Книги")]
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
