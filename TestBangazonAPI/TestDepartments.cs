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
    public class TestDepartments
    {
        [Fact]
        public async Task Test_Get_All_Departments()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/departments");

                string responseBody = await response.Content.ReadAsStringAsync();
                var departmentList = JsonConvert.DeserializeObject<List<Department>>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(departmentList.Count > 0);
            }
        }

        [Fact]
        public async Task Test_Get_Single_Department()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/departments/1");

                string responseBody = await response.Content.ReadAsStringAsync();
                var department = JsonConvert.DeserializeObject<Department>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(1, department.Id);
                Assert.Equal("Department1", department.Name);
                Assert.NotNull(department);
            }
        }

        [Fact]
        public async Task Test_Get_NonExistant_Department_Fails()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/departments/999999999");
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }


        [Fact]
        public async Task Test_Create_And_Delete_Department()
        {
            using (var client = new APIClientProvider().Client)
            {
                Department newDepartment = new Department
                {
                    Name = "TestDepartment",
                    Budget = 9000
                };
                var departmentAsJSON = JsonConvert.SerializeObject(newDepartment);


                var response = await client.PostAsync(
                    "/departments",
                    new StringContent(departmentAsJSON, Encoding.UTF8, "application/json")
                );

                string responseBody = await response.Content.ReadAsStringAsync();
                Department newerDepartment = JsonConvert.DeserializeObject<Department>(responseBody);

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("TestDepartment", newerDepartment.Name);
                Assert.Equal(9000, newerDepartment.Budget);

                var deleteResponse = await client.DeleteAsync($"/departments/{newerDepartment.Id}");
                deleteResponse.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Delete_NonExistant_Department_Fails()
        {
            using (var client = new APIClientProvider().Client)
            {
                var deleteResponse = await client.DeleteAsync("/departments/999999");

                Assert.False(deleteResponse.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Modify_Budget()
        {
            // New Budget
            int budget = 8000;

            using (var client = new APIClientProvider().Client)
            {
                /*
                    PUT section
                 */
                Department modifiedDepartment = new Department
                {
                    Name = "Department1",
                    Budget = budget
                };
                var modifiedDepartmentAsJSON = JsonConvert.SerializeObject(modifiedDepartment);

                var response = await client.PutAsync(
                    "/departments/1",
                    new StringContent(modifiedDepartmentAsJSON, Encoding.UTF8, "application/json")
                );
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                /*
                    GET section
                 */
                var getDepartment = await client.GetAsync("/departments/1");
                getDepartment.EnsureSuccessStatusCode();

                string getDepartmentBody = await getDepartment.Content.ReadAsStringAsync();
                Department newDepartment = JsonConvert.DeserializeObject<Department>(getDepartmentBody);

                Assert.Equal(HttpStatusCode.OK, getDepartment.StatusCode);
                Assert.Equal(budget, newDepartment.Budget);
            }
        }
    }
}