using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace CVAnalyzer.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IConfiguration _config;

        public RegisterModel(IConfiguration config)
        {
            _config = config;
        }

        [BindProperty]
        public string Email { get; set; } = "";

        [BindProperty]
        public string Password { get; set; } = "";

        public string Message { get; set; } = "";
        public string Error { get; set; } = "";

        public IActionResult OnPost()
        {
            string cs = _config.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();

                string check = "SELECT COUNT(*) FROM Users WHERE Email=@Email";

                using (SqlCommand cmd = new SqlCommand(check, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", Email);

                    int exists = (int)cmd.ExecuteScalar();

                    if (exists > 0)
                    {
                        Error = "User already exists";
                        return Page();
                    }
                }

                string insert = "INSERT INTO Users (Email, Password) VALUES (@Email, @Password)";

                using (SqlCommand cmd = new SqlCommand(insert, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@Password", Password);

                    cmd.ExecuteNonQuery();
                }
            }

            Message = "Registration successful!";
            return Page();
        }
    }
}