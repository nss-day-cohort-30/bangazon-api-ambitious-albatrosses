/// <summary>
/// Author: Kirren Covey
/// Purpose: This class contains methods for CRUD functionality for the Product resource
/// </summary>

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BangazonAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;

namespace BangazonAPI.Controllers
{
    [Route("[controller]")]
    [EnableCors("BangazonOnly")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IConfiguration _config;

        public ProductsController(IConfiguration config)
        {
            _config = config;
        }

        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        /// <summary>
        /// Purpose: get all products in the database
        /// </summary>
        /// <returns>
        /// All products
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT 
                                            Id, ProductTypeId, 
                                            CustomerId, Price, 
                                            Title, Description, 
                                            Quantity 
                                        FROM Product";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Product> products = new List<Product>();

                    while (reader.Read())
                    {
                        Product product = new Product
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId")),
                            CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"))
                        };

                        products.Add(product);
                    }
                    reader.Close();

                    return Ok(products);
                }
            }
        }

        /// <summary>
        /// Purpose: get one specific product in the database using its ID
        /// </summary>
        /// <param name="id">
        /// Id of the specific product to be retrieved
        /// </param>
        /// <returns>
        /// One product
        /// </returns>
        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (!ProductExists(id))
            {
                return NotFound();
            }
            else
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"SELECT 
                                            Id, ProductTypeId, 
                                            CustomerId, Price, 
                                            Title, Description, 
                                            Quantity 
                                        FROM Product
                                        WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        SqlDataReader reader = cmd.ExecuteReader();

                        Product product = null;

                        if (reader.Read())
                        {
                            product = new Product
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId")),
                                CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"))
                            };
                        }
                        reader.Close();

                        return Ok(product);

                    }
                }
            }
        }

        /// <summary>
        /// Purpose: add a new product to the database
        /// </summary>
        /// <returns>
        /// Newly created product
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Product 
                                            (ProductTypeId, 
                                            CustomerId, Price, 
                                            Title, Description, 
                                            Quantity)
                                        OUTPUT INSERTED.Id
                                        VALUES 
                                            (@productTypeId, 
                                            @customerId, @price, 
                                            @title, @description, 
                                            @quantity)";
                    cmd.Parameters.Add(new SqlParameter("@productTypeId", product.ProductTypeId));
                    cmd.Parameters.Add(new SqlParameter("@customerId", product.CustomerId));
                    cmd.Parameters.Add(new SqlParameter("@price", product.Price));
                    cmd.Parameters.Add(new SqlParameter("@title", product.Title));
                    cmd.Parameters.Add(new SqlParameter("@description", product.Description));
                    cmd.Parameters.Add(new SqlParameter("@quantity", product.Quantity));

                    int newId = (int)cmd.ExecuteScalar();
                    product.Id = newId;
                    return CreatedAtRoute("GetProduct", new { id = newId }, product);
                }
            }
        }

        /// <summary>
        /// Purpose: edit a product in the database using its ID to ensure the proper product is changed
        /// </summary>
        /// /// <param name="id">
        /// Id of the specific product to be updated
        /// </param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Product product)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Product
                                            SET 
                                                ProductTypeId = @productTypeId, 
                                                CustomerId = @customerId, 
                                                Price = @price, 
                                                Title = @title, 
                                                Description = @description, 
                                                Quantity = @quantity
                                            WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@productTypeId", product.ProductTypeId));
                        cmd.Parameters.Add(new SqlParameter("@customerId", product.CustomerId));
                        cmd.Parameters.Add(new SqlParameter("@price", product.Price));
                        cmd.Parameters.Add(new SqlParameter("@title", product.Title));
                        cmd.Parameters.Add(new SqlParameter("@description", product.Description));
                        cmd.Parameters.Add(new SqlParameter("@quantity", product.Quantity));
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return new StatusCodeResult(StatusCodes.Status204NoContent);
                        }
                        throw new Exception("No rows affected");
                    }
                }
            }
            catch (Exception)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Purpose: delete a product from the database using its ID to ensure the proper product is removed
        /// </summary>
        /// /// <param name="id">
        /// Id of the specific product to be deleted
        /// </param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"DELETE FROM Product WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return new StatusCodeResult(StatusCodes.Status204NoContent);
                        }
                        throw new Exception("No rows affected");
                    }
                }
            }
            catch (Exception)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Purpose: check if a product exists in the database, using its ID
        /// </summary>
        private bool ProductExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, ProductTypeId, 
                            CustomerId, Price, 
                            Title, Description, 
                            Quantity
                        FROM Product
                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();
                    return reader.Read();
                }
            }
        }
    }
}