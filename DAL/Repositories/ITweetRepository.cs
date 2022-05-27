
using TweetService.Models;

namespace TweetService.DAL.Repositories
{
    public interface ITweetRepository
    {
        List<Tweet> GetTweets(string userId);
        Tweet? FindTweet(int id);
        Tweet CreateTweet(Tweet tweet);
        Tweet UpdateTweet(Tweet tweet);

        Tweet LoadLikes(Tweet tweet);
    }
}
