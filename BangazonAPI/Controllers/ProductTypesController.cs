/// <summary>
/// Author: Kirren Covey
/// Purpose: This class contains methods for CRUD functionality for the ProductTypes resource
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
    public class ProductTypesController : Controller
    {
        private readonly IConfiguration _config;

        public ProductTypesController(IConfiguration config)
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
        /// Purpose: get all product types in the database
        /// </summary>
        /// <returns>
        /// All products types
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, [Name] FROM ProductType";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<ProductType> productTypes = new List<ProductType>();

                    while (reader.Read())
                    {
                        ProductType productType = new ProductType
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };

                        productTypes.Add(productType);
                    }
                    reader.Close();

                    return Ok(productTypes);
                }
            }
        }

        /// <summary>
        /// Purpose: get one specficic product type in the database using its ID
        /// </summary>
        /// <param name="id">
        /// Id of the specific product type to be retrieved
        /// </param>
        /// <returns>
        /// One product
        /// </returns>
        [HttpGet("{id}", Name = "GetProductType")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (!ProductTypeExists(id))
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
                        cmd.CommandText = @"
                            SELECT
                                Id, [Name]
                            FROM ProductType
                            WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        SqlDataReader reader = cmd.ExecuteReader();

                        ProductType productType = null;

                        if (reader.Read())
                        {
                            productType = new ProductType
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                        }
                        reader.Close();

                        return Ok(productType);
                    }
                }
            }
        }

        /// <summary>
        /// Purpose: add a new product type to the database
        /// </summary>
        /// <returns>
        /// Newly created product type
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductType productType)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO ProductType ([Name])
                                        OUTPUT INSERTED.Id
                                        VALUES (@name)";
                    cmd.Parameters.Add(new SqlParameter("@name", productType.Name));

                    int newId = (int)cmd.ExecuteScalar();
                    productType.Id = newId;
                    return CreatedAtRoute("GetProductType", new { id = newId }, productType);
                }
            }
        }

        /// <summary>
        /// Purpose: edit a product type in the database using its ID to ensure the proper product type is changed
        /// </summary>
        /// /// <param name="id">
        /// Id of the specific product type to be updated
        /// </param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] ProductType productType)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE ProductType
                                            SET [Name] = @name
                                            WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@name", productType.Name));
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
                if (!ProductTypeExists(id))
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
        /// Purpose: delete a product type from the database using its ID to ensure the proper product type is removed
        /// </summary>
        /// /// <param name="id">
        /// Id of the specific product type to be deleted
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
                        cmd.CommandText = @"DELETE FROM ProductType WHERE Id = @id";
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
                if (!ProductTypeExists(id))
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
        /// Purpose: check if a product type exists in the database, using its ID
        /// </summary>
        private bool ProductTypeExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, [Name]
                        FROM ProductType
                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();
                    return reader.Read();
                }
            }
        }
    }
}