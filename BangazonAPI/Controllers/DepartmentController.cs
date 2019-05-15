// Author: Warner Carpenter
// Purpose: This class contains methods for CRUD functionality for the Department resource

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
    [EnableCors("BangazonOnly")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IConfiguration _config;

        public DepartmentsController(IConfiguration config)
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

        // Purpose: get all departments in the database. User can specify that they want departments returned to include employee data. They can also specify that they want to see only departments with a budget greater than a number of their choice.
        [HttpGet]
        public async Task<IActionResult> Get(string _include, int? _gt)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (_include == "employees")
                    {
                        cmd.CommandText = $@"select d.Id, d.Name, d.Budget, e.Id AS EmployeeId, e.FirstName, e.LastName, e.DepartmentId, e.IsSuperVisor from Department d LEFT JOIN Employee e ON e.DepartmentId = d.id";
                        SqlDataReader reader = cmd.ExecuteReader();

                        List<Department> departments = new List<Department>();

                        while (reader.Read())
                        {
                            if (departments.Count < reader.GetInt32(reader.GetOrdinal("Id")))
                            {
                                Department department = new Department
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Budget = reader.GetInt32(reader.GetOrdinal("Budget")),
                                };

                                if (!reader.IsDBNull(reader.GetOrdinal("EmployeeId")))
                                {
                                    Employee employee = new Employee
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                        DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                                        IsSuperVisor = reader.GetBoolean(reader.GetOrdinal("IsSuperVisor"))
                                    };

                                    department.Employees.Add(employee);
                                }
                                departments.Add(department);
                            }
                            else
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal("EmployeeId")))
                                {
                                    Employee employee = new Employee
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                        DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                                        IsSuperVisor = reader.GetBoolean(reader.GetOrdinal("IsSuperVisor"))
                                    };
                                    departments[reader.GetInt32(reader.GetOrdinal("Id")) - 1].Employees.Add(employee);
                                }
                            }
                        }
                        reader.Close();

                        if (_gt != null)
                        {
                            List<Department> greaterDepartments = new List<Department>();

                            foreach (Department department in departments)
                            {
                                if (department.Budget > _gt)
                                {
                                    greaterDepartments.Add(department);
                                };
                            }

                            return Ok(greaterDepartments);
                        }
                        else
                        {
                            return Ok(departments);
                        }
                    }
                    else
                    {
                        cmd.CommandText = $@"SELECT * FROM Department";
                        SqlDataReader reader = await cmd.ExecuteReaderAsync();

                        List<Department> departments = new List<Department>();
                        while (reader.Read())
                        {
                            Department department = new Department
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Budget = reader.GetInt32(reader.GetOrdinal("Budget"))
                            };
                            departments.Add(department);
                        }
                        reader.Close();

                        if (_gt != null)
                        {
                            List<Department> greaterDepartments = new List<Department>();

                            foreach (Department department in departments)
                            {
                                if (department.Budget > _gt)
                                {
                                    greaterDepartments.Add(department);
                                };
                            }

                            return Ok(greaterDepartments);
                        }
                        else
                        {
                            return Ok(departments);
                        }
                    }
                }
            }
        }

        // Purpose: get one specficic department in the database using its ID
        [HttpGet("{id}", Name = "GetDepartment")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $@"SELECT * FROM Department d WHERE d.Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Department department = null;

                    if (reader.Read())
                    {
                        department = new Department
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Budget = reader.GetInt32(reader.GetOrdinal("Budget"))
                        };
                    }
                    reader.Close();
                    return Ok(department);
                }
            }
        }

        // Purpose: add a new department to the database
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Department department)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Department (Name, Budget) OUTPUT INSERTED.Id VALUES (@name, @budget)
                    ";
                    cmd.Parameters.Add(new SqlParameter("@name", department.Name));
                    cmd.Parameters.Add(new SqlParameter("@budget", department.Budget));

                    int newId = (int)cmd.ExecuteScalar();
                    department.Id = newId;
                    return CreatedAtRoute("GetDepartment", new { id = newId }, department);
                }
            }
        }

        // Purpose: edit a department in the database using its ID to ensure the proper department is changed
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Department department)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Department
                                            SET Name = @name,
                                                Budget = @budget
                                            WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@name", department.Name));
                        cmd.Parameters.Add(new SqlParameter("@budget", department.Budget));
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
                if (!DepartmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // Purpose: delete a department from the database using its ID to ensure the proper department is removed
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
                        cmd.CommandText = @"DELETE FROM Department " +
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
                if (!DepartmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // Purpose: check if a department exists in the database, using its ID
        private bool DepartmentExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // More string interpolation
                    cmd.CommandText = "SELECT Id FROM Department WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();

                    return reader.Read();
                }
            }
        }
    }
}
