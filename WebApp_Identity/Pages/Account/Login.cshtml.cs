using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using WebApp_Identity.Common;

namespace WebApp_Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty] // 2 ways binding
        public Credential Credential { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            // Verify credential
            if (Credential.UserName == "admin" && Credential.Password == "123")
            {
                // Creating the security context
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Email, "admin@mywebsite.com"),
                    new Claim("department", "HR"),
                    new Claim("admin", "true"),
                    new Claim("manager", "true"),
                    new Claim("employment_date", "2022-10-01"),
                };

                var identity = new ClaimsIdentity(claims, AppConstant.AUTHENTICATION_SCHEME);
                ClaimsPrincipal claimsPrincipal = new(identity);

                // Serialize ClaimsPrincipal into a string,
                // then encrypt that string, save that as a cookie in the HttpContext.
                await HttpContext.SignInAsync(AppConstant.AUTHENTICATION_SCHEME, claimsPrincipal);

                return RedirectToPage("/Index");
            }

            return Page();
        }
    }

    public class Credential
    {
        [Required]
        [DisplayName("User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}