using Newtonsoft.Json;
using BangazonAPI.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System;

namespace TestBangazonAPI
{
    public class TestComputers
    {
        [Fact]
        public async Task Test_Get_All_Computers()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/Computers");

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var computerList = JsonConvert.DeserializeObject<List<Computer>>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(computerList.Count > 0);
            }
        }

        [Fact]
        public async Task Test_Get_Single_Computer()
        {
            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/Computers/1");

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var computer = JsonConvert.DeserializeObject<Computer>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(1, computer.Id);
                Assert.Equal("Inspiron", computer.Make);
                Assert.Equal("Dell", computer.Manufacturer);
            }
        }

        [Fact]
        public async Task Test_Get_NonExistent_Computer_Fails()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/Computers/999999999");
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Create_And_Delete_Computer()
        {
            using (var client = new APIClientProvider().Client)
            {
                Computer computer = new Computer
                {
                   PurchaseDate = System.DateTime.Today,
                   DecomissionDate = null,
                   Make = "Inspiron",
                   Manufacturer = "Dell"
            };
                var computerAsJSON = JsonConvert.SerializeObject(computer);


                var response = await client.PostAsync(
                    "/Computers",
                    new StringContent(computerAsJSON, Encoding.UTF8, "application/json")
                );

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                Computer newComputer = JsonConvert.DeserializeObject<Computer>(responseBody);

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("Inspiron", newComputer.Make);
                Assert.Equal("Dell", newComputer.Manufacturer);

                var deleteResponse = await client.DeleteAsync($"/Computers/{newComputer.Id}");
                deleteResponse.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Delete_NonExistent_Computer_Fails()
        {
            using (var client = new APIClientProvider().Client)
            {
                var deleteResponse = await client.DeleteAsync("/Customer/999999");

                Assert.False(deleteResponse.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Modify_Computer()
        {
            // New last name to change to and test
            string updatedMake = "PinkLady";
            string updatedManufacturer = "Apple";

            using (var client = new APIClientProvider().Client)
            {
                /*
                    PUT section
                 */
                Computer modifiedComputer = new Computer
                {
                    PurchaseDate = DateTime.Parse("2015-06-18T07:34:09"),
                    DecomissionDate = null,
                    Make = updatedMake,
                    Manufacturer = updatedManufacturer
                };
                var modifiedComputerAsJSON = JsonConvert.SerializeObject(modifiedComputer);

                var response = await client.PutAsync(
                    "/Computers/4",
                    new StringContent(modifiedComputerAsJSON, Encoding.UTF8, "application/json")
                );
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                /*
                    GET section
                 */
                var getComputer = await client.GetAsync("/Computers/4");
                getComputer.EnsureSuccessStatusCode();

                string getComputerBody = await getComputer.Content.ReadAsStringAsync();
                Computer newComputer = JsonConvert.DeserializeObject<Computer>(getComputerBody);

                Assert.Equal(HttpStatusCode.OK, getComputer.StatusCode);
                Assert.Equal(updatedMake, newComputer.Make);
                Assert.Equal(updatedManufacturer, newComputer.Manufacturer);
            }
        }
    }
}