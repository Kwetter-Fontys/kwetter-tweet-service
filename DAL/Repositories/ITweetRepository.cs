
using TweetService.Models;

namespace TweetService.DAL.Repositories
{
    public interface ITweetRepository
    {
        List<Tweet> GetTweets(string id);
        Tweet FindTweet(int id);
        Tweet CreateTweet(Tweet tweet);
        Tweet UpdateTweet(Tweet tweet);

        Tweet LoadLikes(Tweet tweet);
    }
}
