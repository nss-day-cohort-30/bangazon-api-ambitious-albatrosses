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

namespace BangazonAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IConfiguration _config;

        public EmployeesController(IConfiguration config)
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

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $@"SELECT * FROM Employee e 
                                            LEFT JOIN Department d ON e.DepartmentId = d.id 
                                            LEFT JOIN ComputerEmployee ce ON e.id = ce.EmployeeId 
                                            LEFT JOIN Computer c ON ce.ComputerId = c.Id";
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    List<Employee> employees = new List<Employee>();
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(reader.GetOrdinal("EmployeeId")))
                        {
                            Employee employee = new Employee
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                                IsSuperVisor = reader.GetBoolean(reader.GetOrdinal("IsSuperVisor")),
                                Department = new Department
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Budget = reader.GetInt32(reader.GetOrdinal("Budget"))
                                }
                            };
                            employees.Add(employee);
                        }
                        else
                        {
                            if (reader.IsDBNull(reader.GetOrdinal("DecomissionDate")))
                            {
                                Employee employee = new Employee
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                                    IsSuperVisor = reader.GetBoolean(reader.GetOrdinal("IsSuperVisor")),
                                    Department = new Department
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                                        Name = reader.GetString(reader.GetOrdinal("Name")),
                                        Budget = reader.GetInt32(reader.GetOrdinal("Budget"))
                                    },
                                    Computer = new Computer
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("ComputerId")),
                                        PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate")),
                                        Make = reader.GetString(reader.GetOrdinal("Make")),
                                        Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer"))
                                    }
                                };
                                employees.Add(employee);
                            }
                            else
                            {
                                Employee employee = new Employee
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                                    IsSuperVisor = reader.GetBoolean(reader.GetOrdinal("IsSuperVisor")),
                                    Department = new Department
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                                        Name = reader.GetString(reader.GetOrdinal("Name")),
                                        Budget = reader.GetInt32(reader.GetOrdinal("Budget"))
                                    },
                                    Computer = new Computer
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("ComputerId")),
                                        PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate")),
                                        DecomissionDate = reader.GetDateTime(reader.GetOrdinal("DecomissionDate")),
                                        Make = reader.GetString(reader.GetOrdinal("Make")),
                                        Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer"))
                                    }
                                };
                                employees.Add(employee);
                            }
                        }
                    }
                    reader.Close();
                    return Ok(employees);
                }
            }
        }

        [HttpGet("{id}", Name = "GetEmployee")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $@"SELECT * FROM Employee e 
                                            LEFT JOIN Department d ON e.DepartmentId = d.id 
                                            LEFT JOIN ComputerEmployee ce ON e.id = ce.EmployeeId 
                                            LEFT JOIN Computer c ON ce.ComputerId = c.Id
                                            WHERE e.id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Employee employee = null;

                    if (reader.Read())
                    {
                        if (reader.IsDBNull(reader.GetOrdinal("EmployeeId")))
                        {
                            employee = new Employee
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                                IsSuperVisor = reader.GetBoolean(reader.GetOrdinal("IsSuperVisor")),
                                Department = new Department
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Budget = reader.GetInt32(reader.GetOrdinal("Budget"))
                                }
                            };
                        }
                        else
                        {
                            if (reader.IsDBNull(reader.GetOrdinal("DecomissionDate")))
                            {
                                employee = new Employee
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                                    IsSuperVisor = reader.GetBoolean(reader.GetOrdinal("IsSuperVisor")),
                                    Department = new Department
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                                        Name = reader.GetString(reader.GetOrdinal("Name")),
                                        Budget = reader.GetInt32(reader.GetOrdinal("Budget"))
                                    },
                                    Computer = new Computer
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("ComputerId")),
                                        PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate")),
                                        Make = reader.GetString(reader.GetOrdinal("Make")),
                                        Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer"))
                                    }
                                };
                            }
                            else
                            {
                                employee = new Employee
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                                    IsSuperVisor = reader.GetBoolean(reader.GetOrdinal("IsSuperVisor")),
                                    Department = new Department
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                                        Name = reader.GetString(reader.GetOrdinal("Name")),
                                        Budget = reader.GetInt32(reader.GetOrdinal("Budget"))
                                    },
                                    Computer = new Computer
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("ComputerId")),
                                        PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate")),
                                        DecomissionDate = reader.GetDateTime(reader.GetOrdinal("DecomissionDate")),
                                        Make = reader.GetString(reader.GetOrdinal("Make")),
                                        Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer"))
                                    }
                                };
                            }
                        }
                    }
                    reader.Close();

                    return Ok(employee);
                }
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Employee employee)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // More string interpolation
                    cmd.CommandText = @"
                        INSERT INTO Employee (FirstName, LastName, DepartmentId, IsSuperVisor)
                        OUTPUT INSERTED.Id
                        VALUES (@firstName, @lastName, @departmentId, @isSuperVisor)
                    ";
                    cmd.Parameters.Add(new SqlParameter("@firstName", employee.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@lastName", employee.LastName));
                    cmd.Parameters.Add(new SqlParameter("@departmentId", employee.DepartmentId));
                    cmd.Parameters.Add(new SqlParameter("@isSuperVisor", employee.IsSuperVisor));

                    employee.Id = (int)await cmd.ExecuteScalarAsync();

                    return CreatedAtRoute("GetEmployee", new { id = employee.Id }, employee);
                }
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Employee employee)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Employee
                                            SET FirstName = @firstName,
                                                LastName = @lastName,
                                                DepartmentId = @departmentId,
                                                IsSuperVisor = @isSuperVisor
                                            WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@firstName", employee.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@lastName", employee.LastName));
                        cmd.Parameters.Add(new SqlParameter("@departmentId", employee.DepartmentId));
                        cmd.Parameters.Add(new SqlParameter("@isSuperVisor", employee.IsSuperVisor));
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return new StatusCodeResult(StatusCodes.Status200OK);
                        }
                        throw new Exception("No rows affected");
                    }
                }
            }
            catch (Exception)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool EmployeeExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // More string interpolation
                    cmd.CommandText = "SELECT Id FROM Employee WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();

                    return reader.Read();
                }
            }
        }
    }
}
