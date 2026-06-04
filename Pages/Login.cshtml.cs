using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace CVAnalyzer.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IConfiguration _config;

        public LoginModel(IConfiguration config)
        {
            _config = config;
        }

        [BindProperty]
        public string Email { get; set; } = "";

        [BindProperty]
        public string Password { get; set; } = "";

        public string Error { get; set; } = "";

        public IActionResult OnPost()
        {
            string cs = _config.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();

                string query = " USE CVAnalyzerDb; SELECT Id FROM Users WHERE Email=@Email AND Password=@Password";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@Password", Password);

                    var userId = cmd.ExecuteScalar();

                    // DEBUG LOGS (Terminal output)
                    Console.WriteLine("=================================");
                    Console.WriteLine("Login Attempt");
                    Console.WriteLine("Email: " + Email);
                    Console.WriteLine("UserId: " + (userId ?? "NOT FOUND"));
                    Console.WriteLine("=================================");

                    if (userId != null)
                    {
                        HttpContext.Session.SetString("UserId", userId.ToString());
                        HttpContext.Session.SetString("Email", Email);

                        return RedirectToPage("/Dashboard");
                    }
                }
            }

            Error = "Invalid login";
            return Page();
        }
    }
}