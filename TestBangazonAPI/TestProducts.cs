using System;
using System.Net;
using Newtonsoft.Json;
using Xunit;
using BangazonAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TestBangazonAPI
{
    public class Products
    {
        [Fact]
        public async Task Test_Get_All_Products()
        {
            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/products");

                string responseBody = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<Product>>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(products.Count > 0);
            }
        }

        [Fact]
        public async Task Test_Get_Single_Product()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/products/3");

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<Product>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(3, product.ProductTypeId);
                Assert.Equal(1, product.CustomerId);
                Assert.Equal("Albatross Sweater", product.Title);
                Assert.NotNull(product);
            }
        }

        [Fact]
        public async Task Test_Get_NonExistant_Product_Fails()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/products/999999999");
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

    }
}
