using Microsoft.AspNetCore.Mvc;
using TweetService.DAL.Repositories;
using TweetService.Models;


namespace TweetService.Controllers
{
    [Route("api/tweetcontroller")]
    [ApiController]
    public class TweetController
    {

        ITweetRepository TweetRepository = null;
        public TweetController(ITweetRepository tweetRepo)
        {
            TweetRepository = tweetRepo;
        }

        [HttpGet] // GET /api/tweetcontroller
        public List<Tweet> GetAllTweets()
        {
            return TweetRepository.GetTweets();
        }


        [HttpGet("{id}")]   // GET /api/tweetcontroller/xyz
        public Tweet GetSingleTweet(int id)
        {
            return TweetRepository.GetTweet(id);
        }

        [HttpPut("{id}")]   // PUT /api/tweetcontroller/xyz
        public Tweet LikeTweet(int id, Tweet tweet)
        {
            return TweetRepository.LikeTweet(id, tweet);
        }
    }
}
