using System;
using System.Net;
using Newtonsoft.Json;
using Xunit;
using BangazonAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

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

        [Fact]
        public async Task Test_Create_And_Delete_Product()
        {
            using (var client = new APIClientProvider().Client)
            {
                Product testProduct = new Product
                {
                    Title = "Test Product",
                    Description = "This is a test product",
                    Quantity = 20,
                    Price = 12.99m,
                    ProductTypeId = 1,
                    CustomerId = 1
                };
                var testProductAsJSON = JsonConvert.SerializeObject(testProduct);

                var response = await client.PostAsync(
                    "/products",
                    new StringContent(testProductAsJSON, Encoding.UTF8, "application/json")
                );

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var newTestProduct = JsonConvert.DeserializeObject<Product>(responseBody);

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("Test Product", newTestProduct.Title);
                Assert.Equal(20, newTestProduct.Quantity);
                Assert.Equal(1, newTestProduct.ProductTypeId);

                // Delete the newly posted object

                var deleteResponse = await client.DeleteAsync($"/products/{newTestProduct.Id}");
                deleteResponse.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Delete_NonExistent_Product_Fails()
        {
            using (var client = new APIClientProvider().Client)
            {
                var deleteResponse = await client.DeleteAsync("/prodcuts/9999999999");

                Assert.False(deleteResponse.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Modify_Product()
        {
            // New quantity to change to and test
            int newQuantity = 36;

            using (var client = new APIClientProvider().Client)
            {
                /*
                    PUT section
                 */
                Product modifiedProduct = new Product
                {
                    Title = "Fake Product",
                    Description = "This is for testing, not real",
                    Quantity = newQuantity,
                    Price = 37m,
                    ProductTypeId = 3,
                    CustomerId = 1
                };
                var modifiedProductAsJSON = JsonConvert.SerializeObject(modifiedProduct);

                var response = await client.PutAsync(
                    "/products/1003",
                    new StringContent(modifiedProductAsJSON, Encoding.UTF8, "application/json")
                );
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                /*
                    GET section
                 */
                var getTestProduct = await client.GetAsync("/products/1003");
                getTestProduct.EnsureSuccessStatusCode();

                string getTestProductBody = await getTestProduct.Content.ReadAsStringAsync();
                Product newTestProduct = JsonConvert.DeserializeObject<Product>(getTestProductBody);

                Assert.Equal(HttpStatusCode.OK, getTestProduct.StatusCode);
                Assert.Equal(newQuantity, newTestProduct.Quantity);
            }
        }
    }
}
