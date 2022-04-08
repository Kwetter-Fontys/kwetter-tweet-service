using TweetService.DAL.Repositories;
using TweetService.Models;
namespace TweetService.Services
{
    public class TweetServiceClass
    {
        ITweetRepository TweetRepository;

        public TweetServiceClass(ITweetRepository tweetRepo)
        {
            TweetRepository = tweetRepo;
        }

        public List<Tweet> GetTweetsFromUser(int id)
        {
            return TweetRepository.GetTweets(id);
        }

        public Tweet LikeTweet(Tweet tweet)
        {
            Tweet foundTweet = TweetRepository.FindTweet(tweet.Id);
            //We do this because date messes up in json
            if (foundTweet.Id == tweet.Id && foundTweet.Likes == tweet.Likes && foundTweet.User == tweet.User)
            {
                foundTweet.Likes += 1;
                return TweetRepository.UpdateTweet(foundTweet);
            }
            return tweet;
        }

        public Tweet PostTweet(string content, int uId)
        {
            Tweet tweet = new Tweet(content, uId);
            return TweetRepository.CreateTweet(tweet);
        }
    }
}
