using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace frontend_razor.Pages.Account;

public class RegisterModel : PageModel
{
    [BindProperty]
    public RegisterRequest RegisterRequest { get; set; } = new();

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        // Appel API backend
        using var client = new HttpClient();
        var response = client.PostAsJsonAsync(
            "https://localhost:5001/api/account/register",
            RegisterRequest).Result;

        if (response.IsSuccessStatusCode)
            return RedirectToPage("/Account/Login");

        ModelState.AddModelError("", "Erreur d'inscription");
        return Page();
    }
}

public class RegisterRequest
{
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}