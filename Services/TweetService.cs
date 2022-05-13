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

        public List<TweetViewModel> GetTweetsFromUser(string id)
        {
            List<Tweet> tweets = TweetRepository.GetTweets(id);
            return TransformToViewModelList(tweets);
        }

        public TweetViewModel LikeTweet(int tweetId, string userId)
        {
            Tweet? foundTweet = TweetRepository.FindTweet(tweetId);
            if (foundTweet != null)
            {
                foundTweet = TweetRepository.LoadLikes(foundTweet);
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
                            return TransformToViewModel(foundTweet);
                        }
                    }
                    if (count == 0)
                    {
                        foundTweet.Likes.Add(new Likes(userId));
                        return TransformToViewModel(TweetRepository.UpdateTweet(foundTweet));
                    }
                }
            }
            //Returns empty when nothing found
            return new TweetViewModel();
        }

        public TweetViewModel PostTweet(Tweet tweet, string userTokenId)
        {
            if(tweet.User == userTokenId)
            {
                //Should probally strip id,dates and likes from this
                return TransformToViewModel(TweetRepository.CreateTweet(tweet));
            }
            else
            {
                return new TweetViewModel();
            }
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

        public List<TweetViewModel> TransformToViewModelList(List<Tweet> tweets)
        {
            List<TweetViewModel> allTweets = new List<TweetViewModel>();
            foreach (Tweet tweet in tweets)
            {
                //Propally should load it imediatly instead of looping through like here
                TweetRepository.LoadLikes(tweet);
                allTweets.Add(TransformToViewModel(tweet));
            }
            return allTweets;
        }
    }
}
