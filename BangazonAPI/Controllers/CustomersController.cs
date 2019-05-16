// Author: Connor Bailey
// Purpose: This class contains methods for CRUD functionality for the Customers resource

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BangazonAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors;

namespace BangazonAPI.Controllers
{
    [Route("[controller]")]
    [EnableCors ("BangazonOnly")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CustomerController(IConfiguration config)
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

        // Purpose: get all customers in the database. User can specify that they only want to see customers without an order.
        [HttpGet]
        public async Task<IActionResult> Get(string _include)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (_include == "noOrders")
                    {
                        cmd.CommandText = "SELECT c.Id, c.FirstName, c.LastName " +
                            "FROM Customer c " +
                            "LEFT JOIN [Order] o " +
                            "ON c.Id = o.CustomerId " +
                            "WHERE o.CustomerId IS NULL";
                        SqlDataReader reader = await cmd.ExecuteReaderAsync();

                        List<Customer> customers = new List<Customer>();
                        while (reader.Read())
                        {
                            Customer customer = new Customer
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName"))
                            };

                            customers.Add(customer);
                        }

                        reader.Close();

                        return Ok(customers);
                    }
                    else if (_include == "products")
                    {
                        cmd.CommandText = "SELECT c.Id, c.FirstName, c.LastName, p.Id ProductId, p.ProductTypeId, p.Price, p.Title, p.Description, p.Quantity " +
                            "FROM Customer c " +
                            "LEFT JOIN Product p " +
                            "ON c.Id = p.CustomerId";
                        SqlDataReader reader = await cmd.ExecuteReaderAsync();

                        List<Customer> customers = new List<Customer>();
                        while (reader.Read())
                        {
                            if (customers.Count < reader.GetInt32(reader.GetOrdinal("Id")))
                            {
                                Customer customer = new Customer
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName"))
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
                                        CustomerId = reader.GetInt32(reader.GetOrdinal("Id")),
                                        ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId"))
                                    };

                                    customer.Products.Add(product);
                                }

                                customers.Add(customer);
                            }
                            else
                            {
                                if(!reader.IsDBNull(reader.GetOrdinal("ProductId")))
                                {
                                    Product product = new Product
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                        Title = reader.GetString(reader.GetOrdinal("Title")),
                                        Description = reader.GetString(reader.GetOrdinal("Description")),
                                        Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                        Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                        CustomerId = reader.GetInt32(reader.GetOrdinal("Id")),
                                        ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId"))
                                    };

                                    customers[reader.GetInt32(reader.GetOrdinal("Id")) - 1].Products.Add(product);
                                }
                            }
                        }
                        reader.Close();

                        return Ok(customers);
                    }
                    else if (_include == "payments")
                    {
                        cmd.CommandText = @"SELECT c.Id, c.FirstName, c.LastName, pt.Id PaymentTypeId, pt.AcctNumber, pt.Name
                            FROM Customer c
                            LEFT JOIN PaymentType pt
                            ON c.Id = pt.CustomerId";
                        SqlDataReader reader = await cmd.ExecuteReaderAsync();

                        List<Customer> customers = new List<Customer>();
                        while (reader.Read())
                        {
                            if (customers.Count < reader.GetInt32(reader.GetOrdinal("Id")))
                            {
                                Customer customer = new Customer
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName"))
                                };

                                if (!reader.IsDBNull(reader.GetOrdinal("PaymentTypeId")))
                                {
                                    PaymentType paymentType = new PaymentType
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("PaymentTypeId")),
                                        AccountNumber = reader.GetInt32(reader.GetOrdinal("AcctNumber")),
                                        Name = reader.GetString(reader.GetOrdinal("Name")),
                                        CustomerId = reader.GetInt32(reader.GetOrdinal("Id")),
                                    };

                                    customer.Payments.Add(paymentType);
                                }

                                customers.Add(customer);
                            }
                            else
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal("PaymentTypeId")))
                                {
                                    PaymentType paymentType = new PaymentType
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("PaymentTypeId")),
                                        AccountNumber = reader.GetInt32(reader.GetOrdinal("AcctNumber")),
                                        Name = reader.GetString(reader.GetOrdinal("Name")),
                                        CustomerId = reader.GetInt32(reader.GetOrdinal("Id")),
                                    };

                                    customers[reader.GetInt32(reader.GetOrdinal("Id")) - 1].Payments.Add(paymentType);
                                }
                            }
                        }
                        reader.Close();

                        return Ok(customers);
                    }
                    else
                    {
                        cmd.CommandText = "SELECT Id, FirstName, LastName " +
                            "FROM Customer";
                        SqlDataReader reader = await cmd.ExecuteReaderAsync();

                        List<Customer> customers = new List<Customer>();
                        while (reader.Read())
                        {
                            Customer customer = new Customer
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName"))
                            };

                            customers.Add(customer);
                        }

                        reader.Close();

                        return Ok(customers);
                    }
                }
            }
        }

        // Purpose: get one specficic customer in the database using its ID
        [HttpGet("{id}", Name = "GetCustomer")]
        public async Task<IActionResult> Get([FromRoute] int id, string _include)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (_include == "products")
                    {
                        cmd.CommandText = @"SELECT c.Id, c.FirstName, c.LastName, p.Id ProductId, p.ProductTypeId, p.Price, p.Title, p.Description, p.Quantity
                            FROM Customer c 
                            LEFT JOIN Product p 
                            ON c.Id = p.CustomerId
                            WHERE c.Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        SqlDataReader reader = await cmd.ExecuteReaderAsync();

                        List<Customer> customers = new List<Customer>();
                        while (reader.Read())
                        {
                            if (customers.Count < reader.GetInt32(reader.GetOrdinal("Id")))
                            {
                                Customer customer = new Customer
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName"))
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
                                        CustomerId = reader.GetInt32(reader.GetOrdinal("Id")),
                                        ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId"))
                                    };

                                    customer.Products.Add(product);
                                }

                                customers.Add(customer);
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
                                        CustomerId = reader.GetInt32(reader.GetOrdinal("Id")),
                                        ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId"))
                                    };

                                    customers[reader.GetInt32(reader.GetOrdinal("Id")) - 1].Products.Add(product);
                                }
                            }
                        }
                        reader.Close();

                        return Ok(customers);
                    }
                    else if (_include == "payments")
                    {
                        cmd.CommandText = @"SELECT c.Id, c.FirstName, c.LastName, pt.Id PaymentTypeId, pt.AcctNumber, pt.Name
                            FROM Customer c
                            LEFT JOIN PaymentType pt
                            ON c.Id = pt.CustomerId
                            WHERE c.Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        SqlDataReader reader = await cmd.ExecuteReaderAsync();

                        List<Customer> customers = new List<Customer>();
                        while (reader.Read())
                        {
                            if (customers.Count < reader.GetInt32(reader.GetOrdinal("Id")))
                            {
                                Customer customer = new Customer
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName"))
                                };

                                if (!reader.IsDBNull(reader.GetOrdinal("PaymentTypeId")))
                                {
                                    PaymentType paymentType = new PaymentType
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("PaymentTypeId")),
                                        AccountNumber = reader.GetInt32(reader.GetOrdinal("AcctNumber")),
                                        Name = reader.GetString(reader.GetOrdinal("Name")),
                                        CustomerId = reader.GetInt32(reader.GetOrdinal("Id")),
                                    };

                                    customer.Payments.Add(paymentType);
                                }

                                customers.Add(customer);
                            }
                            else
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal("PaymentTypeId")))
                                {
                                    PaymentType paymentType = new PaymentType
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("PaymentTypeId")),
                                        AccountNumber = reader.GetInt32(reader.GetOrdinal("AcctNumber")),
                                        Name = reader.GetString(reader.GetOrdinal("Name")),
                                        CustomerId = reader.GetInt32(reader.GetOrdinal("Id")),
                                    };

                                    customers[reader.GetInt32(reader.GetOrdinal("Id")) - 1].Payments.Add(paymentType);
                                }
                            }
                        }
                        reader.Close();

                        return Ok(customers);
                    }
                    else if (CustomerExists(id))
                    {
                        cmd.CommandText = @"SELECT Id, FirstName, LastName 
                            FROM Customer
                            WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        SqlDataReader reader = await cmd.ExecuteReaderAsync();

                        List<Customer> customers = new List<Customer>();
                        while (reader.Read())
                        {
                            Customer customer = new Customer
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName"))
                            };

                            customers.Add(customer);
                        }

                        reader.Close();

                        return Ok(customers);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
        }


        // Purpose: add a new customer to the database
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Customer customer)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // More string interpolation
                    cmd.CommandText = @"
                        INSERT INTO Customer (FirstName, LastName)
                        OUTPUT INSERTED.Id
                        VALUES (@firstName, @lastName)
                    ";
                    cmd.Parameters.Add(new SqlParameter("@firstName", customer.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@lastName", customer.LastName));

                    customer.Id = (int)await cmd.ExecuteScalarAsync();

                    return CreatedAtRoute("GetCustomer", new { id = customer.Id }, customer);
                }
            }
        }

        // Purpose: edit a customer in the database using its ID to ensure the proper customer is changed
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Customer customer)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                            UPDATE Customer
                            SET FirstName = @firstName,
                                LastName = @lastName
                            WHERE Id = @id
                        ";
                        cmd.Parameters.Add(new SqlParameter("@firstName", customer.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@lastName", customer.LastName));
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
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // Purpose: delete a customer from the database using its ID to ensure the proper customer is removed
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
                        cmd.CommandText = @"DELETE FROM Customer " +
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
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // Purpose: check if a customer exists in the database, using its ID
        private bool CustomerExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id " +
                        "FROM Customer " +
                        "WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();

                    return reader.Read();
                }
            }
        }
    }
}
