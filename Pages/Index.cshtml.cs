using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace displayOnCards.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public string SelectedCategory { get; set; }
        public List<SelectListItem> ListOfCategories { get; set; } = new List<SelectListItem>();
        public List<Product> Products { get; set; } = new List<Product>();

        public void OnGet(string productCategory = "")
        {
            var allProducts = GetProductsSQLVersion();

            PopulateProductsSelectListItems(allProducts);

            Products = string.IsNullOrEmpty(productCategory) ? allProducts : allProducts.Where(p => p.productCategory == productCategory).ToList();

            SelectedCategory = productCategory;
        }

        public IActionResult OnPost()
        {
            var allProducts = GetProductsSQLVersion();

            PopulateProductsSelectListItems(allProducts);

            Products = string.IsNullOrEmpty(SelectedCategory) ? allProducts : allProducts.Where(p => p.productCategory == SelectedCategory).ToList();

            return Page(); 
        }

        private void PopulateProductsSelectListItems(List<Product> products)
        {
            ListOfCategories = products.Select(p => p.productCategory).Distinct()
                .Select(category => new SelectListItem
                {
                    Value = category,
                    Text = category
                }).ToList();

        }

        private List<Product> GetProductsSQLVersion()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            var productList = new List<Product>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM tblProducts";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        productList.Add(new Product
                        {
                            productID = Convert.ToInt32(reader["productID"]),
                            productName = reader["productName"].ToString(),
                            productDescription = reader["productDescription"].ToString(),
                            productPrice = Convert.ToDecimal(reader["productPrice"]),
                            productCategory = reader["productCategory"].ToString(),
                            productIsFeatured = Convert.ToBoolean(reader["productIsFeatured"])
                        });
                    }
                }
            }
            return productList;
        }
    }

    public class Product
    {
        public int productID { get; set; }
        public string productName { get; set; }
        public string productDescription { get; set; }
        public decimal productPrice { get; set; }
        public string productCategory { get; set; }
        public bool productIsFeatured { get; set; }
    }
}
