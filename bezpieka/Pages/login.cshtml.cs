using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace bezpieka.Pages
{
    public class loginModel : PageModel
    {

        public loginModel()
        {
        }

        [BindProperty]
        public LoginInputModel Input { get; set; }

        public string ErrorMessage { get; set; }

        public class LoginInputModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var name = "";

            using (var connection = new SqliteConnection("Data Source=hello.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    SELECT Username
                    FROM users
                    WHERE Password = $password AND Username = $username
                ";
                command.Parameters.AddWithValue("$password", Input.Password);
                command.Parameters.AddWithValue("$username", Input.Username);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        name = reader.GetString(0);

                        Console.WriteLine($"Hello, {name}!");
                    }
                }
            }

            if (name == "")
            {
                ErrorMessage = "Nieprawid³owa nazwa u¿ytkownika lub has³o.";
                return Page();
            }

            return RedirectToPage("/Index");
        }
    }
}
