using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetService.DAL.Repositories;
using TweetService.Models;

namespace TweetTests.UnitTests
{
    internal class MockTweetRepository : ITweetRepository
    {
        List<Tweet> tweets;
        public MockTweetRepository()
        {
            tweets = new List<Tweet>
            {
                new Tweet("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque interdum rutrum sodales. Nullam mattis fermentum libero, non volutpat.", "7cc35fc6-0eaf-4df8-aaef-773077b4f3c9"){Id = 0 , Date = new DateTime(2008, 5, 1, 8, 30, 52)},
                new Tweet("Lorem", "7cc35fc6-0eaf-4df8-aaef-773077b4f3c9"){Id = 1, Date = new DateTime(2008, 5, 1, 8, 30, 52)},
                new Tweet("a", "7cc35fc6-0eaf-4df8-aaef-773077b4f3c9"){Id = 2, Date = new DateTime(2008, 5, 1, 8, 30, 52)},
                new Tweet("Tweet4", "7cc35fc6-0eaf-4df8-aaef-773077b4f3c9"){Id = 3, Date = new DateTime(2008, 5, 1, 8, 30, 52)},
                new Tweet("Tweet", "c888f6c2-d4ce-442f-b630-52a91150f22a"){Id = 4, Date = new DateTime(2008, 5, 1, 8, 30, 52)},
                new Tweet("Tweet", "1"){Id = 5 , Date = new DateTime(2008, 5, 1, 8, 30, 52)},
                new Tweet("Tweet", "2"){Id = 6 , Date = new DateTime(2008, 5, 1, 8, 30, 52)},
                new Tweet("Tweet", "3"){Id = 7 , Date = new DateTime(2008, 5, 1, 8, 30, 52)},
                new Tweet("Tweet", "4"){Id = 8 , Date = new DateTime(2008, 5, 1, 8, 30, 52)},
                new Tweet("Tweet", "5"){Id = 9 , Date = new DateTime(2008, 5, 1, 8, 30, 52)}
            };
        }
        public Tweet CreateTweet(Tweet tweet)
        {
            tweets.Add(tweet);
            return tweet;
        }

        public Tweet? FindTweet(int tweetId)
        {
            return tweets.Find(x => x.Id == tweetId);
        }

        public List<Tweet> GetTweets(string userId)
        {
            return tweets.Where(x => x.User == userId).OrderByDescending(x => x.Date).ToList();
        }

        public Tweet LoadLikes(Tweet tweet)
        {
            return tweet;
        }

        public Tweet UpdateTweet(Tweet tweet)
        {
            int index = tweets.FindIndex(tweets => tweets.Id == tweet.Id);
            tweets[index] = tweet;
            return tweets[index];
        }
    }
}
