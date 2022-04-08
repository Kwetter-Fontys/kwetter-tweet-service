
using TweetService.Models;

namespace TweetService.DAL.Repositories
{
    public interface ITweetRepository
    {
        List<Tweet> GetTweets(int id);
        Tweet FindTweet(int id);
        Tweet CreateTweet(Tweet tweet);
        Tweet UpdateTweet(Tweet tweet);

        Tweet LoadLikes(Tweet tweet);
    }
}
