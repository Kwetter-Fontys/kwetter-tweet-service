using TweetService.Models;
using TweetService.ViewModels;

namespace TweetService.Services
{
    public interface ITweetService
    {
        void DeleteTweets(string userId);
        List<TweetViewModel> GetTweetsFromUser(string id);
        TweetViewModel LikeTweet(int tweetId, string userId);

        TweetViewModel PostTweet(Tweet tweet, string userTokenId);

        TweetViewModel TransformToViewModel(Tweet tweet);

        List<TweetViewModel> TransformToViewModelList(List<Tweet> tweets);



    }
}
