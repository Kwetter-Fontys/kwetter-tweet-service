﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using TweetService.DAL.Repositories;
using TweetService.Models;
using TweetService.ViewModels;

namespace TweetService.Services
{
    public class TweetServiceClass : ITweetService
    {
        private readonly ITweetRepository TweetRepository;
        private readonly ILogger _logger;

        public TweetServiceClass(ITweetRepository tweetRepo, ILogger<TweetServiceClass> logger)
        {
            _logger = logger;
            TweetRepository = tweetRepo;
        }

        public List<TweetViewModel> GetTweetsFromUser(string id)
        {
            List<Tweet> tweets = TweetRepository.GetTweets(id);
            //Check if empty
            if(!tweets.Any())
            {
                _logger.LogWarning("List of empty tweets was gotten from user: {id}", id);
            }
            else
            {
                _logger.LogInformation("List of {tweets.Count} tweets was gotten from user: {id}", tweets.Count, id);
            }
            return TransformToViewModelList(tweets);
        }

        public TweetViewModel LikeTweet(int tweetId, string userId)
        {
            Tweet? foundTweet = TweetRepository.FindTweet(tweetId);
            if (foundTweet != null)
            {
                if (!foundTweet.Likes.Any())
                {
                    foundTweet.Likes.Add(new Likes(userId));
                    _logger.LogInformation("Tweet: {tweetId} was liked by user: {userId}", tweetId, userId);
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
                            _logger.LogWarning("Tweet: {tweetId} was already liked by user: {userId}", tweetId, userId);
                            return TransformToViewModel(foundTweet);
                        }
                    }
                    if (count == 0)
                    {
                        foundTweet.Likes.Add(new Likes(userId));
                        _logger.LogInformation("Tweet: {tweetId} was liked by user: {userId}", tweetId, userId);
                        return TransformToViewModel(TweetRepository.UpdateTweet(foundTweet));
                    }
                }
            }
            _logger.LogWarning("No tweet with id: {tweetId} found", tweetId);
            //Returns empty when nothing found
            return new TweetViewModel();
        }

        public TweetViewModel PostTweet(Tweet tweet, string userTokenId)
        {
            //This prevents adding tweet with existing id;
            tweet.Id = 0;
            if(tweet.User == userTokenId)
            {
                _logger.LogInformation("User: {userTokenId} succesfully posted tweet with: {tweet.content}", userTokenId, tweet.Content);
                //Should probally strip id,dates and likes from this
                return TransformToViewModel(TweetRepository.CreateTweet(tweet));
            }
            else
            {
                _logger.LogWarning("User: {userTokenId} tried to tweet from and impersonate user: {tweet.User}", userTokenId, tweet.User);
                return new TweetViewModel();
            }
        }


        public TweetViewModel TransformToViewModel(Tweet tweet)
        {
            List<LikesViewModel> likesVM = new List<LikesViewModel>();

            likesVM = tweet.Likes.Select(x => new LikesViewModel()
            {
                LikesId = x.LikesId,
                TweetId = x.TweetId,
                User = x.User,
            }).ToList();
            return new TweetViewModel { Id = tweet.Id, Content = tweet.Content, Date = tweet.Date, User = tweet.User, Likes = likesVM };
        }

        public List<TweetViewModel> TransformToViewModelList(List<Tweet> tweets)
        {
            List<TweetViewModel> allTweets = new List<TweetViewModel>();
            allTweets = tweets.Select(x => new TweetViewModel()
            {
                Id = x.Id,
                Content = x.Content,
                User = x.User,
                Date = x.Date,
                Likes = x.Likes.Select(x => new LikesViewModel()
                {
                    LikesId = x.LikesId,
                    TweetId = x.TweetId,
                    User = x.User,
                }).ToList()
            }).ToList();
            return allTweets;
        }

        public void DeleteTweets(string userId)
        {
            List<Tweet> tweets = TweetRepository.GetTweets(userId);
            //Check if empty
            if (!tweets.Any())
            {
                _logger.LogWarning("DeleteTweets(): List of empty tweets was gotten from user: {id}", userId);
            }
            else
            {
                _logger.LogInformation("DeleteTweets(): List of {tweets.Count} tweets was gotten from user: {id}", tweets.Count, userId);
                TweetRepository.DeleteAllTweetsFromUser(tweets);
            }
        }
    }
}
