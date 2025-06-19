using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Frontend.Pages.Account;

public class LoginModel : PageModel
{
    [BindProperty]
    public LoginRequest LoginRequest { get; set; } = new();

    public string ErrorMessage { get; set; } = string.Empty;

    public IActionResult OnGet()
    {
        // Redirection si déjà authentifié
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToPage("/Index");
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:5001/"); // URL du backend

            var response = await client.PostAsJsonAsync("api/account/login", LoginRequest);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

                // Stocker le token JWT (exemple avec cookie)
                Response.Cookies.Append("auth_token", result!.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(1)
                });

                return RedirectToPage("/Index");
            }

            // Gestion des erreurs HTTP
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                ErrorMessage = "Identifiants incorrects";
            }
            else
            {
                ErrorMessage = "Erreur lors de la connexion. Veuillez réessayer.";
            }
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Impossible de se connecter au serveur. Veuillez réessayer plus tard.";
        }
        catch (Exception)
        {
            ErrorMessage = "Une erreur inattendue s'est produite.";
        }

        return Page();
    }
}

public class LoginRequest
{
    [Required(ErrorMessage = "L'email est requis")]
    [EmailAddress(ErrorMessage = "Format d'email invalide")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le mot de passe est requis")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
}