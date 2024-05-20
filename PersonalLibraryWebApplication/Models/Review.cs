using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PersonalLibraryWebApplication.Models;

public partial class Review
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Поле Користувач є обов'язковим.")]
    [Display(Name = "Користувач")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Поле Книга є обов'язковим.")]
    [Display(Name = "Книга")]
    public int BookId { get; set; }

    [Required(ErrorMessage = "Поле Оцінка є обов'язковим.")]
    [Display(Name = "Оцінка")]
    [Range(1, 5, ErrorMessage = "Оцінка повинна бути між 1 і 5")]
    public string Rating { get; set; } = null!;

    [Display(Name = "Коментар")]
    public string? Comment { get; set; }

    [Required]
    [Display(Name = "Створено о")]
    public DateTime CreatedAt { get; set; }

    //Navigation property
    public virtual Book Book { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
