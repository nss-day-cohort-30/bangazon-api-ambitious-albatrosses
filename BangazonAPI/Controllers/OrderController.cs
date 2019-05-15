// Author: Connor Bailey
// Purpose: This class contains methods for CRUD functionality for the Order resource

using System;
using System.Collections.Generic;
using System.Data;
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
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration _config;

        public OrderController(IConfiguration config)
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

        // Purpose: get all orders in the database, including product info for every product on the orders
        [HttpGet]
        public async Task<IActionResult> Get(string _include)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (_include == null)
                    {
                        cmd.CommandText = @"SELECT o.Id, o.CustomerId, o.PaymentTypeId,
                                            p.Id ProductId, p.Title, p.[Description], p.Price, p.Quantity, p.ProductTypeId, p.CustomerId
                                        FROM [Order] o 
                                        LEFT JOIN OrderProduct op ON o.Id = op.OrderId
                                        LEFT JOIN Product p ON p.Id = op.ProductId;
                                        ;";
                        SqlDataReader reader = await cmd.ExecuteReaderAsync();

                        List<Order> orders = new List<Order>();

                        while (reader.Read())
                        {
                            if (orders.Count < reader.GetInt32(reader.GetOrdinal("Id")))
                            {
                                Order order = new Order
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                    PaymentTypeId = reader.GetInt32(reader.GetOrdinal("PaymentTypeId"))
                                };

                                if (!reader.IsDBNull(reader.GetOrdinal("ProductId")))
                                {
                                    Product product = new Product
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                        Title = reader.GetString(reader.GetOrdinal("Title")),
                                        Description = reader.GetString(reader.GetOrdinal("Description")),
                                        Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                        Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                        CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                        ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId"))
                                    };

                                    order.Products.Add(product);
                                }
                                orders.Add(order);
                            }
                            else
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal("ProductId")))
                                {
                                    Product product = new Product
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                        Title = reader.GetString(reader.GetOrdinal("Title")),
                                        Description = reader.GetString(reader.GetOrdinal("Description")),
                                        Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                        Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                        CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                        ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId"))
                                    };

                                    orders[reader.GetInt32(reader.GetOrdinal("Id")) - 1].Products.Add(product);
                                }
                            }

                        }
                        reader.Close();

                        return Ok(orders);
                    }
                    else if (_include == "customers")
                    {
                        cmd.CommandText = @"
                        SELECT o.Id, o.CustomerId, o.PaymentTypeId, 
                        pr.Id ProductId, pr.Title, pr.[Description], pr.Price, pr.Quantity, pr.ProductTypeId, pr.CustomerId ProdCustId,
                        c.FirstName, c.LastName
                        FROM [Order] o
                        LEFT JOIN [OrderProduct] op 
                        ON o.id=op.OrderId
                        INNER JOIN Product pr 
                        ON op.ProductId=pr.Id
                        INNER JOIN ProductType pt 
                        ON pr.ProductTypeId=pt.Id 
                        INNER JOIN [Customer] c 
                        ON o.CustomerId=c.Id";
                        SqlDataReader reader = await cmd.ExecuteReaderAsync();

                        List<Order> orders = new List<Order>();

                        while (reader.Read())
                        {
                            if (orders.Count < reader.GetInt32(reader.GetOrdinal("Id")))
                            {
                                Order order = new Order
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                    PaymentTypeId = reader.GetInt32(reader.GetOrdinal("PaymentTypeId"))
                                };

                                if (!reader.IsDBNull(reader.GetOrdinal("ProductId")))
                                {
                                    Product product = new Product
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                        Title = reader.GetString(reader.GetOrdinal("Title")),
                                        Description = reader.GetString(reader.GetOrdinal("Description")),
                                        Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                        Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                        CustomerId = reader.GetInt32(reader.GetOrdinal("prodCustId")),
                                        ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId"))
                                    };

                                    order.Products.Add(product);
                                }

                                if(!reader.IsDBNull(reader.GetOrdinal("CustomerId")))
                                {
                                    order.customer = new Customer
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                        LastName = reader.GetString(reader.GetOrdinal("LastName"))
                                    };
                                }
                                orders.Add(order);
                            }
                            else
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal("ProductId")))
                                {
                                    Product product = new Product
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                        Title = reader.GetString(reader.GetOrdinal("Title")),
                                        Description = reader.GetString(reader.GetOrdinal("Description")),
                                        Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                        Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                        CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                        ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId"))
                                    };

                                    orders[reader.GetInt32(reader.GetOrdinal("Id")) - 1].Products.Add(product);
                                }
                            }

                        }
                        reader.Close();

                        return Ok(orders);
                    }
                    else
                    {
                        return StatusCode(418);
                    }
                }
            }
        }

        // Purpose: get one specficic order in the database using its ID, including product info for every product on the order
        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT o.Id, o.CustomerId, o.PaymentTypeId,
                                            p.Id ProductId, p.Title, p.[Description], p.Price, p.Quantity, p.ProductTypeId, p.CustomerId
                                        FROM [Order] o 
                                        LEFT JOIN OrderProduct op ON o.Id = op.OrderId
                                        LEFT JOIN Product p ON p.Id = op.ProductId
                                        WHERE o.Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    Order order = null;
                    while (reader.Read())
                    {
                        if (order == null)
                        {
                            order = new Order
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                PaymentTypeId = reader.GetInt32(reader.GetOrdinal("PaymentTypeId"))
                            };
                        }


                        if (!reader.IsDBNull(reader.GetOrdinal("ProductId")))
                        {
                            Product product = new Product
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId"))
                            };

                            order.Products.Add(product);
                        }
                    }

                    reader.Close();

                    return Ok(order);
                }
            }
        }

        // Purpose: add a new product to the database
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order order)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // More string interpolation
                    cmd.CommandText = @"
                        INSERT INTO [Order] (CustomerId, PaymentTypeId)
                        OUTPUT INSERTED.Id
                        VALUES (@customerId, @paymentTypeId)
                    ";
                    cmd.Parameters.Add(new SqlParameter("@customerId", order.CustomerId));
                    cmd.Parameters.Add(new SqlParameter("@paymentTypeId", order.PaymentTypeId));

                    order.Id = (int)await cmd.ExecuteScalarAsync();

                    return CreatedAtRoute("GetOrder", new { id = order.Id }, order);
                }
            }
        }

        // Purpose: edit an order in the database using its ID to ensure the proper order is changed
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Order order)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                            UPDATE [Order]
                            SET CustomerId = @customerId,
                                PaymentTypeId = @paymentTypeId
                            WHERE Id = @id
                        ";
                        cmd.Parameters.Add(new SqlParameter("@customerId", order.CustomerId));
                        cmd.Parameters.Add(new SqlParameter("@paymentTypeId", order.PaymentTypeId));
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

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
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // Purpose: delete an order from the database using its ID to ensure the proper order is removed
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
                        cmd.CommandText = @"DELETE FROM [Order] " +
                            "WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
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
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // Purpose: check if an order exists in the database, using its ID
        private bool OrderExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id " +
                        "FROM [Order] " +
                        "WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();

                    return reader.Read();
                }
            }
        }
    }
}
