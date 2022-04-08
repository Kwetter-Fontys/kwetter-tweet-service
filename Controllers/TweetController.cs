using Microsoft.AspNetCore.Mvc;
using TweetService.DAL.Repositories;
using TweetService.Models;
using TweetService.Services;

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
        public List<Tweet> GetAllTweets(int id)
        {
            return tweetService.GetTweetsFromUser(id);
        }

        [HttpPut]   // PUT /api/tweetcontroller
        public Tweet LikeTweet(Tweet tweet)
        {
            return tweetService.LikeTweet(tweet);
        }

        [HttpPost]// post /api/tweetcontroller
        public Tweet PostTweet(string content, int uId)
        {
            return tweetService.PostTweet(content, uId);
        }
    }
}
