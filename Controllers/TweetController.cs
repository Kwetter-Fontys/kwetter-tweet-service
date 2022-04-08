using Microsoft.AspNetCore.Mvc;
using TweetService.DAL.Repositories;
using TweetService.Models;
using TweetService.Services;
using TweetService.ViewModels;

namespace TweetService.Controllers
{
    [Route("api/tweetcontroller")]
    [ApiController]
    public class TweetController
    {

        TweetServiceClass tweetService;
        public TweetController(ITweetRepository tweetRepo)
        {
            tweetService = new TweetServiceClass(tweetRepo);
        }

        [HttpGet("{id}")]// GET /api/tweetcontroller/xyz
        public List<TweetViewModel> GetAllTweets(int id)
        {
            return tweetService.GetTweetsFromUser(id);
        }

        [HttpPut("{id}")]   // PUT /api/tweetcontroller/xyz
        public TweetViewModel LikeTweet(int id, int userId)
        {
            return tweetService.LikeTweet(id,userId);
        }

        [HttpPost]// post /api/tweetcontroller
        public TweetViewModel PostTweet(Tweet tweet)
        {
           return tweetService.PostTweet(tweet);
        }
    }
}
