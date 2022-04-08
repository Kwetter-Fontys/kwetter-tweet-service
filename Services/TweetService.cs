using TweetService.DAL.Repositories;
using TweetService.Models;
using TweetService.ViewModels;

namespace TweetService.Services
{
    public class TweetServiceClass
    {
        ITweetRepository TweetRepository;

        public TweetServiceClass(ITweetRepository tweetRepo)
        {
            TweetRepository = tweetRepo;
        }

        public List<TweetViewModel> GetTweetsFromUser(int id)
        {
            List<Tweet> tweets = TweetRepository.GetTweets(id);
            List<TweetViewModel> allTweets = new List<TweetViewModel>();
            foreach (Tweet tweet in tweets)
            {
                allTweets.Add(TransformToViewModel(tweet));
            }
            return allTweets;
        }

        public TweetViewModel LikeTweet(int tweetId, int userId)
        {
            Tweet foundTweet = TweetRepository.FindTweet(tweetId);
            foundTweet = TweetRepository.LoadLikes(foundTweet);
            if (foundTweet != null)
            {
                if (foundTweet.Likes.Count() == 0)
                {
                    foundTweet.Likes.Add(new Likes(userId));
                    return TransformToViewModel(TweetRepository.UpdateTweet(foundTweet));
                }
                else
                {
                    int count = 0;
                    foreach (Likes like in foundTweet.Likes)
                    {
                        if (like.User == userId)
                        {
                            count += 1;
                            break;
                        }
                    }
                    if (count == 0)
                    {
                        foundTweet.Likes.Add(new Likes(userId));
                        return TransformToViewModel(TweetRepository.UpdateTweet(foundTweet));
                    }
                }
            }
            return TransformToViewModel(foundTweet);
        }

        public TweetViewModel PostTweet(string content, int uId)
        {
            Tweet tweet = new Tweet(content, uId);
            return TransformToViewModel(TweetRepository.CreateTweet(tweet));
        }


        public TweetViewModel TransformToViewModel(Tweet tweet)
        {
            List<LikesViewModel> likesVM = new List<LikesViewModel>();

            foreach(Likes like in tweet.Likes)
            {
                likesVM.Add(new LikesViewModel { LikesId = like.LikesId, TweetId = like.TweetId, User = like.User });
            }
            return new TweetViewModel { Id = tweet.Id, Content = tweet.Content, Date = tweet.Date, User = tweet.User, Likes = likesVM };
        }
    }
}
