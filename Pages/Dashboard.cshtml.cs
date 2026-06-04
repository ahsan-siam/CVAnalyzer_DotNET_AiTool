using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CVAnalyzer.Pages
{
    public class DashboardModel : PageModel
    {
        public string Email { get; set; } = "";

        public IActionResult OnGet()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var email = HttpContext.Session.GetString("Email");

            if (userId == null)
            {
                return RedirectToPage("/Login");
            }

            Email = email ?? "User";

            return Page();
        }

        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Login");
        }
    }
}