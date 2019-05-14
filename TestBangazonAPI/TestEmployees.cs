using Newtonsoft.Json;
using BangazonAPI.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq;

namespace TestBangazonAPI
{
    public class TestEmployees
    {
        [Fact]
        public async Task Test_Get_All_Employees()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/employees");

                string responseBody = await response.Content.ReadAsStringAsync();
                var employeeList = JsonConvert.DeserializeObject<List<Employee>>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(employeeList.Count > 0);
            }
        }

        [Fact]
        public async Task Test_Get_Single_Employee()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/employees/1");

                string responseBody = await response.Content.ReadAsStringAsync();
                var employee = JsonConvert.DeserializeObject<Employee>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(1, employee.Id);
                Assert.Equal("Warner", employee.FirstName);
                Assert.Equal("Carpenter", employee.LastName);
                Assert.NotNull(employee);
            }
        }

        [Fact]
        public async Task Test_Get_NonExistant_Employee_Fails()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/employees/999999999");
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }


        [Fact]
        public async Task Test_Create_And_Delete_Employee()
        {
            using (var client = new APIClientProvider().Client)
            {
                Employee newEmployee = new Employee
                {
                    FirstName = "Justina",
                    LastName = "Vickers",
                    DepartmentId = 1,
                    IsSuperVisor = true
                };
                var employeeAsJSON = JsonConvert.SerializeObject(newEmployee);


                var response = await client.PostAsync(
                    "/employees",
                    new StringContent(employeeAsJSON, Encoding.UTF8, "application/json")
                );

                string responseBody = await response.Content.ReadAsStringAsync();
                Employee newerEmployee = JsonConvert.DeserializeObject<Employee>(responseBody);

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("Justina", newerEmployee.FirstName);
                Assert.Equal("Vickers", newerEmployee.LastName);
                Assert.Equal(1, newerEmployee.DepartmentId);

                var deleteResponse = await client.DeleteAsync($"/employees/{newerEmployee.Id}");
                deleteResponse.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Delete_NonExistant_Employee_Fails()
        {
            using (var client = new APIClientProvider().Client)
            {
                var deleteResponse = await client.DeleteAsync("/employees/999999");

                Assert.False(deleteResponse.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Modify_DepartmentId()
        {
            // New DepaertmentId
            int departmentId = 2;

            using (var client = new APIClientProvider().Client)
            {
                /*
                    PUT section
                 */
                Employee modifiedEmployee = new Employee
                {
                    FirstName = "Warner",
                    LastName = "Carpenter",
                    DepartmentId = departmentId,
                    IsSuperVisor = true
                };
                var modifiedEmployeeAsJSON = JsonConvert.SerializeObject(modifiedEmployee);

                var response = await client.PutAsync(
                    "/employees/1",
                    new StringContent(modifiedEmployeeAsJSON, Encoding.UTF8, "application/json")
                );
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                /*
                    GET section
                 */
                var getEmployee = await client.GetAsync("/employees/1");
                getEmployee.EnsureSuccessStatusCode();

                string getEmployeeBody = await getEmployee.Content.ReadAsStringAsync();
                Employee newEmployee = JsonConvert.DeserializeObject<Employee>(getEmployeeBody);

                Assert.Equal(HttpStatusCode.OK, getEmployee.StatusCode);
                Assert.Equal(departmentId, newEmployee.DepartmentId);
            }
        }
    }
}