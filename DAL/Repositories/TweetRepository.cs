using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetService.Models;

namespace TweetService.DAL.Repositories
{
    public class TweetRepository : ITweetRepository
    {
        private readonly TweetContext tweetContext;

        public TweetRepository(TweetContext context)
        {
            this.tweetContext = context;
        }
        public List<Tweet> GetTweets()
        {
            return tweetContext.Tweets.ToList();
        }

        //Should catch null error
        public Tweet GetTweet(int id)
        {
            return tweetContext.Tweets.Find(id);
        }

        //Should just give like + 1 
        public Tweet LikeTweet(int id, Tweet tweet)
        {
            //Should add catches
            tweetContext.Tweets.Update(tweet);
            tweetContext.SaveChanges();
            return tweet;
        }

        public void AddTweet(Tweet tweet)
        {
        }

        public void DeleteTweet(Tweet tweet)
        {
        }


    }
}
