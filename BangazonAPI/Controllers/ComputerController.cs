﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BangazonAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BangazonAPI.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class ComputersController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ComputersController(IConfiguration config)
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
        // GET /values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())

                {
                    cmd.CommandText = "SELECT Id, PurchaseDate, DecomissionDate, Make, Manufacturer FROM Computer";
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();


                    List<Computer> computers = new List<Computer>();

                    while (reader.Read())
                    {

                    bool IsDecomDateNull = reader.IsDBNull(reader.GetOrdinal("DecomissionDate"));

                    if (IsDecomDateNull == false)
                        {
                            Computer computer = new Computer
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate")),
                                Make = reader.GetString(reader.GetOrdinal("Make")),
                                Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer")),
                                DecomissionDate = reader.GetDateTime(reader.GetOrdinal("DecomissionDate"))
                            };
                            computers.Add(computer);
                        }
                     else if (IsDecomDateNull == true)         
                        {
                            Computer computer = new Computer
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate")),
                                Make = reader.GetString(reader.GetOrdinal("Make")),
                                Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer")),
                            };
                            computers.Add(computer);
                        }
                    }
                    reader.Close();

                    return Ok(computers);
                }
            }
        }

        // GET/values/5
        [HttpGet("{id}", Name = "GetComputer")]
        public async Task<IActionResult> Get(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT PurchaseDate, DecomissionDate, Make, Manufacturer
                    FROM Computer
                    WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    Computer computer = null;
                    if (reader.Read())
                    {
                        computer = new Computer
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate")),
                            DecomissionDate = reader.GetDateTime(reader.GetOrdinal("DecomissionDate")),
                            Make = reader.GetString(reader.GetOrdinal("Make")),
                            Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer")),
                        };
                    }

                    reader.Close();

                    return Ok(computer);
                }
            }
        }

        //PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Computer computer)
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
                            SET FirstName = @firstName
                            -- Set the remaining columns here
                            WHERE Id = @id
                        ";
                        cmd.Parameters.Add(new SqlParameter("@id", computer.Id));
                        cmd.Parameters.Add(new SqlParameter("@firstName", computer.PurchaseDate));
                        cmd.Parameters.Add(new SqlParameter("@firstName", computer.DecomissionDate));

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
                if (!ComputerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        private bool ComputerExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, PurchaseDate, DecomissionDate, Make, Manufacturer
                        FROM Computer
                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();
                    return reader.Read();
                }
            }
        }

        // POST /values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Computer computer)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // More string interpolation
                    cmd.CommandText = @"
                        INSERT INTO Computer (PurchaseDate, DecomissionDate, Make, Manufacturer)
                        OUTPUT INSERTED.Id
                        VALUES (@PurchaseDate, @DecomissionDate, @Make, @Manufacturer)";
                    cmd.Parameters.Add(new SqlParameter("@PurchaseDate", computer.PurchaseDate));
                    cmd.Parameters.Add(new SqlParameter("@DecomissionDate", computer.DecomissionDate));
                    cmd.Parameters.Add(new SqlParameter("@Make", computer.Make));
                    cmd.Parameters.Add(new SqlParameter("@Manufacturer", computer.Manufacturer));

                    int newId = (int)await cmd.ExecuteScalarAsync();
                    computer.Id = newId;

                    return CreatedAtRoute("GetComputer", new { id = newId }, computer);
                }
            }
        }
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
                        cmd.CommandText = @"DELETE FROM Computer WHERE Id = @id";
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
                if (!ComputerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
    }
}




