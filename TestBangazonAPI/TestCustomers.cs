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
    public class TestCustomers
    {
        [Fact]
        public async Task Test_Get_All_Customers()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/Customer");

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var customerList = JsonConvert.DeserializeObject<List<Customer>>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(customerList.Count > 0);
            }
        }

        [Fact]
        public async Task Test_Get_All_Customers_Without_Orders()
        {
            string KirrenFirstName = "Kirren";
            string KirrenLastName = "Covey";
            int KirrenId = 2;

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/Customer?refine=noOrders");

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var customerList = JsonConvert.DeserializeObject<List<Customer>>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(customerList.Count > 0);
                Assert.Equal(customerList[0].FirstName, KirrenFirstName);
                Assert.Equal(customerList[0].LastName, KirrenLastName);
                Assert.Equal(customerList[0].Id, KirrenId);
            }
        }

        [Fact]
        public async Task Test_Get_Single_Customer()
        {
            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/Customer/3");

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var customer = JsonConvert.DeserializeObject<Customer>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(3, customer.Id);
                Assert.Equal("Warner", customer.FirstName);
                Assert.Equal("Carpenter", customer.LastName);
                Assert.NotNull(customer);
            }
        }

        [Fact]
        public async Task Test_Get_NonExistant_Customer_Fails()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/Customer/999999999");
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }


        [Fact]
        public async Task Test_Create_And_Delete_Customer()
        {
            using (var client = new APIClientProvider().Client)
            {
                Customer customer = new Customer
                {
                    FirstName = "Steve",
                    LastName = "Brownlee",
                };
                var customerAsJSON = JsonConvert.SerializeObject(customer);


                var response = await client.PostAsync(
                    "/Customer",
                    new StringContent(customerAsJSON, Encoding.UTF8, "application/json")
                );

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                Customer newCustomer = JsonConvert.DeserializeObject<Customer>(responseBody);

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("Steve", newCustomer.FirstName);
                Assert.Equal("Brownlee", newCustomer.LastName);

                var deleteResponse = await client.DeleteAsync($"/Customer/{newCustomer.Id}");
                deleteResponse.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Delete_NonExistant_Customer_Fails()
        {
            using (var client = new APIClientProvider().Client)
            {
                var deleteResponse = await client.DeleteAsync("/Customer/999999");

                Assert.False(deleteResponse.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Modify_Customer()
        {
            // New last name to change to and test
            string updatedCustomerFirstName = "Kimmy";
            string updatedCustomerLastName = "Bird";

            using (var client = new APIClientProvider().Client)
            {
                /*
                    PUT section
                 */
                Customer modifiedCustomer = new Customer
                {
                    FirstName = updatedCustomerFirstName,
                    LastName = updatedCustomerLastName
                };
                var modifiedCustomerAsJSON = JsonConvert.SerializeObject(modifiedCustomer);

                var response = await client.PutAsync(
                    "/Customer/5",
                    new StringContent(modifiedCustomerAsJSON, Encoding.UTF8, "application/json")
                );
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                /*
                    GET section
                 */
                var getCustomer = await client.GetAsync("/Customer/5");
                getCustomer.EnsureSuccessStatusCode();

                string getCustomerBody = await getCustomer.Content.ReadAsStringAsync();
                Customer newCustomer = JsonConvert.DeserializeObject<Customer>(getCustomerBody);

                Assert.Equal(HttpStatusCode.OK, getCustomer.StatusCode);
                Assert.Equal(updatedCustomerFirstName, newCustomer.FirstName);
                Assert.Equal(updatedCustomerLastName, newCustomer.LastName);
            }
        }
    }
}