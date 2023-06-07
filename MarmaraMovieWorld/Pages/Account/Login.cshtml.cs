using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MarmaraMovieWorld.Model;
using Microsoft.EntityFrameworkCore;
using MarmaraMovieWorld.Data;
using System.Security.Claims;

namespace acme.Pages;

public class LoginModel : PageModel
{
    private readonly ApplicationDbContext _dbContext;

    public LoginModel(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task OnGet(string returnUrl = "/Success")
    {
        Console.WriteLine("OnGet metoduna giriþ yapýldý.");
        var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                    .WithRedirectUri(returnUrl)
                    .Build();

        await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        Console.WriteLine("OnGet metodu bitti.");
    }

}