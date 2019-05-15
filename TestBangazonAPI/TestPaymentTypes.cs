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
    public class TestPaymentTypes
    {
        [Fact]
        public async Task Test_Get_All_Payment_Types()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/PaymentType");

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var paymentTypeList = JsonConvert.DeserializeObject<List<PaymentType>>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(paymentTypeList.Count > 0);
            }
        }

        [Fact]
        public async Task Test_Get_Single_Payment_Type()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/PaymentType/3");

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var paymentType = JsonConvert.DeserializeObject<PaymentType>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(2468, paymentType.AccountNumber);
                Assert.Equal("ConnorAccount3", paymentType.Name);
                Assert.NotNull(paymentType);
            }
        }

        [Fact]
        public async Task Test_Get_NonExistant_Payment_Type_Fails()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/PaymentType/999999999");
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }


        [Fact]
        public async Task Test_Create_And_Delete_Payment_Type()
        {
            using (var client = new APIClientProvider().Client)
            {
                PaymentType newPaymentType = new PaymentType
                {
                    Name = "Test Payment Account 1",
                    AccountNumber = 13579,
                    CustomerId = 1
                };
                var paymentTypeAsJSON = JsonConvert.SerializeObject(newPaymentType);


                var response = await client.PostAsync(
                    "/PaymentType",
                    new StringContent(paymentTypeAsJSON, Encoding.UTF8, "application/json")
                );

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                PaymentType newerPaymentType = JsonConvert.DeserializeObject<PaymentType>(responseBody);

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("Test Payment Account 1", newerPaymentType.Name);
                Assert.Equal(13579, newerPaymentType.AccountNumber);
                Assert.Equal(1, newerPaymentType.CustomerId);


                var deleteResponse = await client.DeleteAsync($"/PaymentType/{newerPaymentType.Id}");
                deleteResponse.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Delete_NonExistant_PaymentType_Fails()
        {
            using (var client = new APIClientProvider().Client)
            {
                var deleteResponse = await client.DeleteAsync("/PaymentType/999999");

                Assert.False(deleteResponse.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Modify_PaymentType()
        {
            string newAcctName = "New Acct Name For Test";
            int newAcctNum = 12345;

            using (var client = new APIClientProvider().Client)
            {
                /*
                    PUT section
                 */
                PaymentType modifiedPaymentType = new PaymentType
                {
                    Name = newAcctName,
                    AccountNumber = newAcctNum,
                    CustomerId = 1
                };
                var modifiedPaymentTypeAsJSON = JsonConvert.SerializeObject(modifiedPaymentType);

                var response = await client.PutAsync(
                    "/PaymentType/4",
                    new StringContent(modifiedPaymentTypeAsJSON, Encoding.UTF8, "application/json")
                );
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                /*
                    GET section
                 */
                var getPaymentType = await client.GetAsync("/PaymentType/4");
                getPaymentType.EnsureSuccessStatusCode();

                string getPaymentTypeBody = await getPaymentType.Content.ReadAsStringAsync();
                PaymentType newPaymentType = JsonConvert.DeserializeObject<PaymentType>(getPaymentTypeBody);

                Assert.Equal(HttpStatusCode.OK, getPaymentType.StatusCode);
                Assert.Equal(newAcctName, newPaymentType.Name);
                Assert.Equal(newAcctNum, newPaymentType.AccountNumber);
            }
        }
    }
}