using MCPApproach.Model;
using Microsoft.Data.SqlClient;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace MCPApproach.Tools
{
    [McpServerToolType]
    internal class ProductTool
    {
        [McpServerTool, Description("Gets top products which sold the most")]
        [return: Description("Product Id, Name, Code and TotalSale")]
        public List<Product> GetTopProducts(int count)
        {
            var products = new List<Product>();
            var connectionString = "YOUR CONNECTION STRING";

            using (var connection = new SqlConnection(connectionString))
            {
                const string query = @"
                SELECT TOP (@count) 
                    Id, 
                    Name, 
                    Code, 
                    TotalSales 
                FROM Product 
                ORDER BY TotalSales DESC";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@count", count);
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Code = reader.GetString(2),
                                TotalSales = reader.GetInt32(3)
                            });
                        }
                    }
                }
            }

            return products;
        }
    }
}
