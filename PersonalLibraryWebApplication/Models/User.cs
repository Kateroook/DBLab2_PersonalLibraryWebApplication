using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PersonalLibraryWebApplication.Models;

public partial class User
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Поле Ім'я є обов'язковим.")]
    [Display(Name = "Ім'я")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Поле Електронна адреса є обов'язковим.")]
    [EmailAddress(ErrorMessage = "Невірний формат електронної адреси")]
    [Display(Name = "Електрона адреса")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Поле Пароль є обов'язковим.")]
    [StringLength(100, ErrorMessage = "Поле {0} повинно містити принаймні {2} символів.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; } = null!;

    public virtual ICollection<Booklist> Booklists { get; set; } = new List<Booklist>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
