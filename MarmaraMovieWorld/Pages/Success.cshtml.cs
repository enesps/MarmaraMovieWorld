using Microsoft.AspNetCore.Mvc.RazorPages;
using MarmaraMovieWorld.Model;
using Microsoft.EntityFrameworkCore;
using MarmaraMovieWorld.Data;
using System.Security.Claims;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace acme.Pages
{
    public class CallbackModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public CallbackModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Console.WriteLine("Ongetasync");
            var authenticateResult = await HttpContext.AuthenticateAsync(Auth0Constants.AuthenticationScheme);
            Console.WriteLine(authenticateResult.Succeeded);
            var principal = authenticateResult.Principal;

            foreach (var claim in principal.Claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }

            if (authenticateResult.Succeeded)
            {
                
                var email = authenticateResult.Principal.FindFirstValue("email");
                string name = authenticateResult.Principal.FindFirstValue("name");

                Console.WriteLine(email);
                Console.WriteLine(name);

                if (!string.IsNullOrEmpty(email))
                {
                    // Burada e-posta adresini kullanarak kullanýcýnýn user tablosunda olup olmadýðýný kontrol edebilirsiniz
                    var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

                    if (existingUser == null)
                    {
                        // Kullanýcý user tablosunda bulunamadý, yeni bir kullanýcý ekleyebilirsiniz

                        var newUser = new User
                        {
                            Email = email,
                            Name = name
                            // Diðer gerekli alanlarý da doldurun
                        };

                        _dbContext.Users.Add(newUser);
                        await _dbContext.SaveChangesAsync();

                        // Yeni kullanýcý baþarýyla eklendi, gerekli iþlemleri yapabilirsiniz
                    }
                }
            }

            return RedirectToPage("Index");
        }
    }
}
