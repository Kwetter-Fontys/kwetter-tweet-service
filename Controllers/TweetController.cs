using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using TweetService.DAL.Repositories;
using TweetService.Models;
using TweetService.Services;
using TweetService.ViewModels;
using UserService.Controllers;

namespace TweetService.Controllers
{
    [Authorize]
    [Route("api/tweetcontroller")]
    [ApiController]
    public class TweetController : ControllerBase
    {
        JwtTokenHelper jwtTokenHelper;
        TweetServiceClass tweetService;
        public TweetController(ITweetRepository tweetRepo)
        {
            jwtTokenHelper = new JwtTokenHelper();
            tweetService = new TweetServiceClass(tweetRepo);
        }

        [HttpGet("{id}")]// GET /api/tweetcontroller/xyz
        public List<TweetViewModel> GetAllTweets(string id)
        {
            return tweetService.GetTweetsFromUser(id);
        }

        [HttpPut("{id}")]   // PUT /api/tweetcontroller/xyz
        public TweetViewModel LikeTweet(int id)
        {
            string userTokenId = jwtTokenHelper.GetId(Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", ""));
            return tweetService.LikeTweet(id, userTokenId);
        }

        [HttpPost]// post /api/tweetcontroller
        public TweetViewModel PostTweet(Tweet tweet)
        {
            string userTokenId = jwtTokenHelper.GetId(Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", ""));
            return tweetService.PostTweet(tweet, userTokenId);
        }
    }
}
