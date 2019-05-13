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
    public class TestOrders
    {
        [Fact]
        public async Task Test_Get_All_Orders()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/Order");

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var orderList = JsonConvert.DeserializeObject<List<Order>>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(orderList.Count > 0);
            }
        }

        [Fact]
        public async Task Test_Get_Single_Order()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/Order/1");

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<Order>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(1, order.Id);
                Assert.Equal(1, order.CustomerId);
                Assert.Equal(1, order.PaymentTypeId);
                Assert.NotNull(order);
            }
        }

        [Fact]
        public async Task Test_Get_NonExistant_Order_Fails()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/[Order]/999999999");
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }


        [Fact]
        public async Task Test_Create_And_Delete_Order()
        {
            using (var client = new APIClientProvider().Client)
            {
                Order newOrder = new Order
                {
                    PaymentTypeId = 3,
                    CustomerId = 1
                };
                var orderAsJSON = JsonConvert.SerializeObject(newOrder);


                var response = await client.PostAsync(
                    "/Order",
                    new StringContent(orderAsJSON, Encoding.UTF8, "application/json")
                );

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                Order newerOrder = JsonConvert.DeserializeObject<Order>(responseBody);

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal(1, newerOrder.CustomerId);
                Assert.Equal(3, newerOrder.PaymentTypeId);

                var deleteResponse = await client.DeleteAsync($"/Order/{newerOrder.Id}");
                deleteResponse.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Delete_NonExistant_Order_Fails()
        {
            using (var client = new APIClientProvider().Client)
            {
                var deleteResponse = await client.DeleteAsync("/Order/999999");

                Assert.False(deleteResponse.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Modify_PaymentType()
        {
            // New PaymentId
            int paymentId = 3;

            using (var client = new APIClientProvider().Client)
            {
                /*
                    PUT section
                 */
                Order modifiedOrder = new Order
                {
                    PaymentTypeId = paymentId,
                    CustomerId = 1
                };
                var modifiedOrderAsJSON = JsonConvert.SerializeObject(modifiedOrder);

                var response = await client.PutAsync(
                    "/Order/1",
                    new StringContent(modifiedOrderAsJSON, Encoding.UTF8, "application/json")
                );
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                /*
                    GET section
                 */
                var getOrder = await client.GetAsync("/Order/1");
                getOrder.EnsureSuccessStatusCode();

                string getOrderBody = await getOrder.Content.ReadAsStringAsync();
                Order newOrder = JsonConvert.DeserializeObject<Order>(getOrderBody);

                Assert.Equal(HttpStatusCode.OK, getOrder.StatusCode);
                Assert.Equal(1, newOrder.CustomerId);
                Assert.Equal(paymentId, newOrder.PaymentTypeId);
            }
        }
    }
}