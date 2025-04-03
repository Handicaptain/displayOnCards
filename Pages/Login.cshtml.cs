using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace displayOnCards.Pages;

    public class LoginModel : PageModel
    {
        private readonly RolsaDbContext _context;

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public LoginModel(RolsaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Query the database to find the user by their entered username
            var user = await _context.tblUsers
                .FirstOrDefaultAsync(u => u.Username == Username); // Match the Username field

            if (user == null)
            {
                // If the user does not exist
                ErrorMessage = "Invalid username or password.";
                return Page();
            }

            // Directly compare the entered password with the stored plaintext password
            if (user.Passwd != Password)  // Compare plaintext passwords
            {
                // If the password does not match
                ErrorMessage = "Invalid username or password.";
                return Page();
            }

            // If login is successful, redirect to the home page (or any other page)
            return RedirectToPage("/Index");
        }
    }




