using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PersonalLibraryWebApplication.Models;

public partial class Book
{
    public int Id { get; set; }

    [Display(Name = "Автор")]
    public int AuthorId { get; set; }

    [Required(ErrorMessage = "Поле Назва є обов'язковим")]
    [Display(Name = "Назва")]
    public string Title { get; set; } = null!;

    [Display(Name = "Обкладинка")]
    public byte[]? BookCover { get; set; } = null;

    [Required(ErrorMessage = "Поле Мова є обов'язковим")]
    [Display(Name = "Мова")]
    public int LanguageId { get; set; }

    [Required(ErrorMessage = "Поле Рік видання є обов'язковим")]
    [Display(Name = "Рік видання")]
    [Range(1000, 2024, ErrorMessage = "Рік видання повинен бути між 1000 і 2024")]
    public int PublishingYear { get; set; }

    [Required(ErrorMessage = "Поле Опис є обов'язковим")]
    [Display(Name = "Опис")]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "Поле Сторінки є обов'язковим")]
    [Display(Name = "Сторінки")]
    public int Pages { get; set; }

    //Navigation property
    [Display(Name = "Автор")]
    public virtual Author Author { get; set; } = null!;

    public virtual ICollection<BooksInBooklist> BooksInBooklists { get; set; } = new List<BooksInBooklist>();

    [Display(Name = "Мова")]
    public virtual Language Language { get; set; } = null!;

    [Display(Name = "Відгуки")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    
    [Display(Name = "Категорії")]
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    [Display(Name = "Видавництва")]
    public virtual ICollection<Publisher> Publishers { get; set; } = new List<Publisher>();
}
