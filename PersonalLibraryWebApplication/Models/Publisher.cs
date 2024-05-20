using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PersonalLibraryWebApplication.Models;

public partial class Publisher
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Поле Назва є обов'язковим.")]
    [Display(Name = "Назва")]
    public string Name { get; set; } = null!;

    [Display(Name = "Про видавництво")]
    public string? Description { get; set; }


    //Navigation property
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
