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

        // GET api/values
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

        // POST api/values
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

        // PUT api/values/5
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
