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
    public class TestTrainingPrograms
    {
        [Fact]
        public async Task Test_Get_All_TrainingPrograms()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/TrainingPrograms");

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var trainingProgramList = JsonConvert.DeserializeObject<List<TrainingProgram>>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(trainingProgramList.Count > 0);
            }
        }

        [Fact]
        public async Task Test_Get_Single_TrainingProgram()
        {
            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/TrainingPrograms/3");

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var trainingProgram = JsonConvert.DeserializeObject<TrainingProgram>(responseBody);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(3, trainingProgram.Id);          
            }
        }

        [Fact]
        public async Task Test_Get_NonExistent_Program_Fails()
        {

            using (var client = new APIClientProvider().Client)
            {
                var response = await client.GetAsync("/TrainingPrograms/999999999");
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Create_And_Delete_Program()
        {
            using (var client = new APIClientProvider().Client)
            {
                TrainingProgram trainingProgram = new TrainingProgram
                {
                   Name = "AWS",
                   StartDate = DateTime.Today,
                   EndDate = DateTime.Now,
                   MaxAttendees = 70
                   
            };
                var programAsJSON = JsonConvert.SerializeObject(trainingProgram);


                var response = await client.PostAsync(
                    "/TrainingPrograms",
                    new StringContent(programAsJSON, Encoding.UTF8, "application/json")
                );

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                TrainingProgram newProgram = JsonConvert.DeserializeObject<TrainingProgram>(responseBody);

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal(70, newProgram.MaxAttendees);
                Assert.Equal("AWS", newProgram.Name);

                var deleteResponse = await client.DeleteAsync($"/TrainingPrograms/{newProgram.Id}");
                deleteResponse.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Delete_NonExistent_Program_Fails()
        {
            using (var client = new APIClientProvider().Client)
            {
                var deleteResponse = await client.DeleteAsync("/TrainingPrograms/999999");

                Assert.False(deleteResponse.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Modify_Program()
        {
            // New last name to change to and test
            int updatedMaxAttendees = 100;
            string updatedName = "Python";

            using (var client = new APIClientProvider().Client)
            {
                /*
                    PUT section
                 */
                TrainingProgram modifiedProgram = new TrainingProgram
                {
                    StartDate = DateTime.Parse("2015-06-12T10:26:05"),
                    EndDate = DateTime.Parse("2015-06-13T09:26:06"),
                    Name = updatedName,
                    MaxAttendees = updatedMaxAttendees
                };
                var modifiedProgramAsJSON = JsonConvert.SerializeObject(modifiedProgram);

                var response = await client.PutAsync(
                    "/TrainingPrograms/3",
                    new StringContent(modifiedProgramAsJSON, Encoding.UTF8, "application/json")
                );
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                /*
                    GET section
                 */
                var getProgram = await client.GetAsync("/TrainingPrograms/3");
                getProgram.EnsureSuccessStatusCode();

                string getProgramBody = await getProgram.Content.ReadAsStringAsync();
                TrainingProgram newProgram = JsonConvert.DeserializeObject<TrainingProgram>(getProgramBody);

                Assert.Equal(HttpStatusCode.OK, getProgram.StatusCode);
                Assert.Equal(updatedName, newProgram.Name);
                Assert.Equal(updatedMaxAttendees, newProgram.MaxAttendees);
            }
        }
    }
}