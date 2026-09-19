using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Anketa.Pages;

public class IndexModel : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Укажите ФИО")]
    public string FullName { get; set; } = "";

    [BindProperty]
    [Range(1, 120, ErrorMessage = "Возраст от 1 до 120")]
    public int Age { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Укажите email")]
    [EmailAddress(ErrorMessage = "Неверный email")]
    public string Email { get; set; } = "";

    [BindProperty]
    [Required(ErrorMessage = "Укажите телефон")]
    public string Phone { get; set; } = "";

    [BindProperty]
    [Required(ErrorMessage = "Укажите город")]
    public string City { get; set; } = "";

    [BindProperty]
    public string About { get; set; } = "";

    public bool Submitted { get; set; }

    public void OnPost()
    {
        if (ModelState.IsValid)
        {
            Submitted = true;
        }
    }
}
