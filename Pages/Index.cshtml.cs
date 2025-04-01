using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace displayOnCards.Pages
{
    public class IndexModel : PageModel
    {
        //public void apply1882()
        //{
        //    string newQuery = "SELECT * FROM tblUsers WHERE userID = 1882;";
        //}

        //public void apply1883()
        //{
        //    string newQuery = "SELECT * FROM tblUsers WHERE userID = 1883;";
        //}

        private readonly IConfiguration _configuration;

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public string SelectedUser { get; set; }
        public List<SelectListItem> UserOptions { get; set; }

        public List<User> Users { get; set; } = new List<User>();

        public List<User> FilterUsers(List<User> users, string selectedUser)
        {
            if (String.IsNullOrEmpty(selectedUser))
            {
                return users;
            }
            return users.Where(u => u.UserID == selectedUser).ToList();
        }

        //public List<User> FilterUsersSQLVersion(string selectedUser)
        //{
        //    string connectionString = _configuration.GetConnectionString("DefaultConnection");

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        string query = @"SELECT * FROM tblUsers WHERE UserId = @UserId";

        //        using (SqlCommand command = new SqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@UserId", selectedUser);

        //            SqlDataReader reader = command.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                Users.Add(new User
        //                {
        //                    UserID = reader["userID"].ToString(),
        //                    Username = reader["username"].ToString(),
        //                    Name = reader["name"].ToString()
        //                });
        //            }
        //        }
        //    }
        //    return Users.ToList();
        //}

        public IActionResult OnPost()
        {
            return RedirectToPage("Index", new { userID = SelectedUser });
        }

        public IActionResult OnGet(string userID = "")
        {
            Users = GetUsers();
            //Users = GetUsersSQLVersion();

            PopulateUsersSelectListItems(Users);

            Users = FilterUsers(Users, userID);
            //Users = FilterUsersSQLVersion(userID);

            return Page();
        }

        public List<SelectListItem> PopulateUsersSelectListItems(List<User> users)
        {
            UserOptions = Users.Distinct().Select(a =>
                new SelectListItem
                {
                    Value = a.UserID,
                    Text = a.UserID,
                }).ToList();

            UserOptions.Insert(0, new SelectListItem { Value = "", Text = "all" });

            return UserOptions;
        }

        public List<User> GetUsers()
        {
            List<User> users = new List<User>();
            users.Add(new User
            {
                UserID = "1883",
                Username = "username1",
                Name = "Brian"
            });
            users.Add(new User
            {
                UserID = "1881",
                Username = "username2",
                Name = "Paul"
            });
            users.Add(new User
            {
                UserID = "1882",
                Username = "username3",
                Name = "Gary"
            });
            users.Add(new User
            {
                UserID = "1883",
                Username = "username4",
                Name = "John"
            });

            return users;
        }

        public List<User> GetUsersSQLVersion()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM tblUsers";

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

            return Users.ToList();
        }

        
    }

    

    public class User
    {
        public string UserID { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
    }
}
