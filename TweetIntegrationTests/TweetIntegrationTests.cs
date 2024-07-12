using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Xunit;
using TweetService;
using System;
using System.Net.Http;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using TweetService.ViewModels;
using System.Net.Http.Json;
using TweetService.Models;

namespace TweetIntegrationTests
{
    public class TweetIntegrationTests 
        : IClassFixture<CustomWebApplicationFactory<Program>>
    {

        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;
        public string AccessToken;
        public string CorrectId = "bf40cabc-3cc7-49bb-aeba-cd1c6ab23dcc";
        public TweetIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
               _factory = factory;
                _client = factory.CreateClient(new WebApplicationFactoryClientOptions
                {
                    
                    AllowAutoRedirect = false
                });
                _factory = factory;
            AccessToken = GetAccessToken();
        }

        public string GetAccessToken()
        {
            var client = new RestClient("https://keycloak.sebananasprod.nl/auth/realms/kwetter/protocol/openid-connect/token");
            var request = new RestRequest("https://keycloak.sebananasprod.nl/auth/realms/kwetter/protocol/openid-connect/token", Method.Post);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "password");
            request.AddParameter("client_id", "Kwetter-frontend");
            request.AddParameter("username", "sebas");
            request.AddParameter("password", "test");
            RestResponse response = client.ExecuteAsync(request).Result;

            var myJObject = JObject.Parse(response.Content);
            return (string)myJObject["access_token"];
        }


        [Fact]
        public async Task AccessControllerWithoutToken()
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/api/tweetcontroller/bf40cabc-3cc7-49bb-aeba-cd1c6ab23dcc"))
            {
                HttpResponseMessage response = await _client.SendAsync(requestMessage);
                Console.WriteLine(response);
                Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            }
        }

        [Fact]
        public async Task GetAllTweetsFromUserWithToken()
        { 
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/api/tweetcontroller/bf40cabc-3cc7-49bb-aeba-cd1c6ab23dcc"))
            {
                requestMessage.Headers.Authorization =  new AuthenticationHeaderValue("Bearer", AccessToken);
                HttpResponseMessage response = await _client.SendAsync(requestMessage);

                var rep =   response.Content.ReadAsStringAsync().Result;
                List<TweetViewModel> myJObject = JsonConvert.DeserializeObject<List<TweetViewModel>>(rep);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(4, myJObject.Count);
            }
        }

        [Fact]
        public async Task LikeTweetWithToken()
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Put, "/api/tweetcontroller/1"))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
                HttpResponseMessage response = await _client.SendAsync(requestMessage);

                var rep = response.Content.ReadAsStringAsync().Result;
                TweetViewModel myJObject = JsonConvert.DeserializeObject<TweetViewModel>(rep);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(1, myJObject.Likes.Count);
            }
        }


        [Fact]
        public async Task PostTweetWithToken()
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/tweetcontroller"))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
                //Tweet tweet = new Tweet("Tweet", "bf40cabc-3cc7-49bb-aeba-cd1c6ab23dc") { Id = 1, Date = "2022-05-28T13:58:05" }
                requestMessage.Content = JsonContent.Create(new { id = 1, content = "Tweet", user = "bf40cabc-3cc7-49bb-aeba-cd1c6ab23dcc", date = "2022-05-28T13:58:05" });
                HttpResponseMessage response = await _client.SendAsync(requestMessage);

                var rep = response.Content.ReadAsStringAsync().Result;
                TweetViewModel myJObject = JsonConvert.DeserializeObject<TweetViewModel>(rep);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(11, myJObject.Id);
            }
        }
    }
}