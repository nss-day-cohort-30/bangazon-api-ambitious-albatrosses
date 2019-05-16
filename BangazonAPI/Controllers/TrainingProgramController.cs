/// <summary>
/// Author: Stephen Clark
/// Purpose: This class contains methods for CRUD functionality for the Trainng Program resource
/// </summary>


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
    [Route("/[controller]")]
    [ApiController]
    public class TrainingProgramsController : ControllerBase
    {

        private readonly IConfiguration _config;

        public TrainingProgramsController(IConfiguration config)
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

        [HttpGet]
        public async Task<IActionResult> Get(bool? completed)
        {
            string sql
            = @"SELECT tp.id AS TPId, Name, StartDate, EndDate, MaxAttendees, DepartmentId, IsSupervisor, e.id AS EmployeeId, FirstName, LastName FROM TrainingProgram tp
              LEFT JOIN EmployeeTraining et on et.TrainingProgramId = tp.id
              LEFT JOIN Employee e on et.EmployeeId = e.Id";

            DateTime currentDateTime = DateTime.UtcNow;

            if (completed == false)
            {
                sql = $"{ sql} WHERE tp.StartDate >= @currentDateTime";
            }
            else if (completed == true)
            {
                sql = $"{sql} WHERE tp.StartDate < @currentDateTime";
            }

            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;

                    if (completed != null)
                    {
                        cmd.Parameters.Add(new SqlParameter("@currentDateTime", currentDateTime));
                    }

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    Dictionary<int, TrainingProgram> programHash = new Dictionary<int, TrainingProgram>();

                    while (reader.Read())
                    {
                        int programId = reader.GetInt32(reader.GetOrdinal("TPId"));
                        bool IsEmployeeIdNull = reader.IsDBNull(reader.GetOrdinal("EmployeeId"));

                        if (!programHash.ContainsKey(programId))
                        {
                            programHash[programId] = new TrainingProgram

                            {
                                Id = reader.GetInt32(reader.GetOrdinal("TPId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                                EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                                MaxAttendees = reader.GetInt32(reader.GetOrdinal("MaxAttendees")),
                            };
                        }

                        if (IsEmployeeIdNull == false)
                        {

                            programHash[programId].EmployeesAssigned.Add(new Employee
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                IsSuperVisor = reader.GetBoolean(reader.GetOrdinal("IsSuperVisor")),
                                DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId"))
                            });
                        }

                    }

                    List<TrainingProgram> trainingPrograms = programHash.Values.ToList();
                    reader.Close();

                    return Ok(trainingPrograms);
                }
            }
        }

        // GET/TrainingPrograms/4
        [HttpGet("{id}", Name = "GetTrainingProgram")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
            SELECT tp.Id AS TPId, Name, StartDate, EndDate, MaxAttendees, DepartmentId, IsSupervisor, e.id AS EmployeeId, FirstName, LastName FROM TrainingProgram tp
              LEFT JOIN EmployeeTraining et on et.TrainingProgramId = tp.id
              LEFT JOIN Employee e on et.EmployeeId = e.Id
                    WHERE tp.Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    TrainingProgram trainingProgram = new TrainingProgram();

                    if (reader.Read())
                    {
                        int programId = reader.GetInt32(reader.GetOrdinal("TPId"));
                        bool IsEmployeeIdNull = reader.IsDBNull(reader.GetOrdinal("EmployeeId"));

                        if (trainingProgram.Id != programId)
                        {
                            {
                                trainingProgram.Id = reader.GetInt32(reader.GetOrdinal("TPId"));
                                trainingProgram.Name = reader.GetString(reader.GetOrdinal("Name"));
                                trainingProgram.StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate"));
                                trainingProgram.EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate"));
                                trainingProgram.MaxAttendees = reader.GetInt32(reader.GetOrdinal("MaxAttendees"));
                            };
                        }
                        if (IsEmployeeIdNull == false)
                        {
                            trainingProgram.EmployeesAssigned.Add(new Employee
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                IsSuperVisor = reader.GetBoolean(reader.GetOrdinal("IsSuperVisor")),
                                DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId"))
                            });

                        }

                    }

                    reader.Close();
                    if (trainingProgram.Id == id)
                    {
                        return Ok(trainingProgram);
                    } else
                    {
                        return new StatusCodeResult(StatusCodes.Status204NoContent);
                    }
                }
            }
        }

        /// <summary>
        /// Purpose: edit a training program in the database using its ID 
        /// </summary>
        /// /// <param name="id">
        /// Id of the specific program to be updated
        /// </param>        

        //PUT api/TrainngPrograms/4
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] TrainingProgram trainingProgram)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                            UPDATE TrainingProgram
                            SET [Name] = @Name,
                            StartDate = @StartDate,
                            EndDate = @EndDate,
                            MaxAttendees = @MaxAttendees
                            WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        cmd.Parameters.Add(new SqlParameter("@Name", trainingProgram.Name));
                        cmd.Parameters.Add(new SqlParameter("@StartDate", trainingProgram.StartDate));
                        cmd.Parameters.Add(new SqlParameter("@EndDate", trainingProgram.EndDate));
                        cmd.Parameters.Add(new SqlParameter("@MaxAttendees", trainingProgram.MaxAttendees));

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
                if (!TrainingProgramExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        private bool TrainingProgramExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, Name, StartDate, EndDate, MaxAttendees
                        FROM TrainingProgram
                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();
                    return reader.Read();
                }
            }
        }

        /// <summary>
        /// Purpose: add a new product type to the database
        /// </summary>
        /// <returns>
        /// Newly created product type
        /// </returns>

        // POST /values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TrainingProgram trainingProgram)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO TrainingProgram (Name, StartDate, EndDate, MaxAttendees)
                        OUTPUT INSERTED.Id
                        VALUES (@Name, @StartDate, @EndDate, @MaxAttendees)";
                    cmd.Parameters.Add(new SqlParameter("@Name", trainingProgram.Name));
                    cmd.Parameters.Add(new SqlParameter("@StartDate", trainingProgram.StartDate));
                    cmd.Parameters.Add(new SqlParameter("@EndDate", trainingProgram.EndDate));
                    cmd.Parameters.Add(new SqlParameter("@MaxAttendees", trainingProgram.MaxAttendees));

                    int newId = (int)await cmd.ExecuteScalarAsync();
                    trainingProgram.Id = newId;

                    return CreatedAtRoute("GetTrainingProgram", new { id = newId }, trainingProgram);
                }
            }
        }

        /// <summary>
        /// Purpose: delete a training program from the database using its ID 
        /// </summary>
        /// /// <param name="id">
        /// Id of the specific program to be deleted
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
                        cmd.CommandText = @"  DELETE FROM EmployeeTraining WHERE TrainingProgramId = @id
                                              DELETE FROM TrainingProgram WHERE Id = @id";
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
                if (!TrainingProgramExists(id))
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




