using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace displayOnCards.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public List<User> Users { get; set; } = new List<User>();

        public void OnGet()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT TOP 3 userID, username, name FROM tblUsers";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Users.Add(new User
                        {
                            UserID = reader["userID"].ToString(),
                            Username = reader["username"].ToString(),
                            Name = reader["name"].ToString()
                        });
                    }
                }
            }
        }
    }

    public class User
    {
        public string UserID { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
    }
}
