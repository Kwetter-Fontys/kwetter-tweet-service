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
        public List<Tweet> GetTweets(int userId)
        {
            List<Tweet> tweets = tweetContext.Tweets.Where(t => t.User == userId).OrderByDescending(t => t.Date).ToList();
            return tweets;
        }

        public Tweet FindTweet(int id)
        {
            return tweetContext.Tweets.Find(id);
        }

        public Tweet LoadLikes(Tweet tweet)
        {
            tweetContext.Entry(tweet).Collection(t => t.Likes).Load();
            return tweet;
        }


        public Tweet CreateTweet(Tweet tweet)
        {
            tweetContext.Tweets.Add(tweet);
            tweetContext.SaveChanges();
            return tweet;
        }

        public Tweet UpdateTweet(Tweet tweet)
        {
            tweetContext.Tweets.Update(tweet);
            tweetContext.SaveChanges();
            return tweet;
        }

    }
}
