using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TweetService.Models;
using TweetService.Services;
using TweetService.ViewModels;
using TweetTests.UnitTests;

namespace TweetTests
{
    [TestClass]
    public class TweetTest
    {
        public string MainUserId = "7cc35fc6-0eaf-4df8-aaef-773077b4f3c9";
        public string FakeUserId = "000";
        public List<TweetViewModel> TweetList;
        public List<TweetViewModel> AssertList = new List<TweetViewModel>
        {
                new TweetViewModel{Id = 0, Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque interdum rutrum sodales. Nullam mattis fermentum libero, non volutpat.", User = "7cc35fc6-0eaf-4df8-aaef-773077b4f3c9", Date = new DateTime(2008, 5, 1, 8, 30, 52) },
                new TweetViewModel{Id = 1, Content = "Lorem", User = "7cc35fc6-0eaf-4df8-aaef-773077b4f3c9", Date = new DateTime(2008, 5, 1, 8, 30, 52) },
                new TweetViewModel{Id = 2, Content = "a", User = "7cc35fc6-0eaf-4df8-aaef-773077b4f3c9", Date = new DateTime(2008, 5, 1, 8, 30, 52) },
                new TweetViewModel{Id = 3, Content = "Tweet4", User = "7cc35fc6-0eaf-4df8-aaef-773077b4f3c9", Date = new DateTime(2008, 5, 1, 8, 30, 52) }
        };
        public Tweet ExistingTweet = new Tweet("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque interdum rutrum sodales. Nullam mattis fermentum libero, non volutpat.", "7cc35fc6-0eaf-4df8-aaef-773077b4f3c9") { Id = 0, Date = new DateTime(2008, 5, 1, 8, 30, 52) };
        //Used when no modifications are needed.
        public TweetServiceClass ExistingService = new TweetServiceClass(new MockTweetRepository());

        //Testing the GetTweet method

        [TestMethod]
        public void GetTweetsFromExistingUserRetunsListOfTweets()
        {
            TweetList = ExistingService.GetTweetsFromUser(MainUserId);
            //ExpectedList, ActualList
            Assert.AreEqual(AssertList.Count, TweetList.Count, "List count is not equal, when it should be");
        }

        [TestMethod]
        public void GetTweetsFromFakeUserRetunsListOfZeroTweets()
        {
            TweetList = ExistingService.GetTweetsFromUser(FakeUserId);
            Assert.AreEqual(0, TweetList.Count, "List count is more than 0 when it shouldn't be");
        }

        //Testing the LikeTweet method

        //We don't have to test with non existing user as the user is provided using JWT Tokens
        [TestMethod]
        public void LikeTweetAddsLikeToExistingTweet()
        {
            TweetServiceClass service = new TweetServiceClass(new MockTweetRepository());
            TweetViewModel tweet = service.LikeTweet(0, MainUserId);
            Assert.AreEqual(1, tweet.Likes.Count, "Likes count is not equal to 1, on existing tweet");
        }


        [TestMethod]
        public void LikeTweetDoesntAddLikeToNonTweet()
        {
            TweetServiceClass service = new TweetServiceClass(new MockTweetRepository());
            TweetViewModel tweet = service.LikeTweet(100, MainUserId);
            Assert.AreEqual(null, tweet.Likes, "Likes count is not null on non existing tweet");
        }

        [TestMethod]
        public void LikeTweetWithExistingLikes()
        {
            TweetServiceClass service = new TweetServiceClass(new MockTweetRepository());
            service.LikeTweet(0, MainUserId);
            TweetViewModel tweet = service.LikeTweet(0, "userid");
            Assert.AreEqual(2, tweet.Likes.Count, "Likes count is not equal to 2, on existing tweet when liked twice");
        }

        [TestMethod]
        public void LikeTweetMultipleTimesWithSameUser()
        {
            TweetServiceClass service = new TweetServiceClass(new MockTweetRepository());
            service.LikeTweet(0, MainUserId);
            TweetViewModel tweet = service.LikeTweet(0, MainUserId);
            Assert.AreEqual(1, tweet.Likes.Count, "Likes count is not equal to 1 when liking 2 times on same account on same tweet");
        }

        //Testing the PostTweet method

        [TestMethod]
        public void PostTweetWithEqualUserIdAndUserTokenId()
        {
            TweetServiceClass service = new TweetServiceClass(new MockTweetRepository());
            TweetViewModel tweet = service.PostTweet(ExistingTweet, MainUserId);
            Assert.AreEqual(MainUserId, tweet.User, "Tweet wasn't posted when using correct user ids");
        }

        [TestMethod]
        public void PostTweetWithNonEqualUserIdAndUserTokenId()
        {
            TweetServiceClass service = new TweetServiceClass(new MockTweetRepository());
            TweetViewModel tweet = service.PostTweet(ExistingTweet, "1000");
            Assert.AreEqual(null, tweet.User, "Tweet wasn posted when using incorrect user ids");
        }

        //Testing transform view model method
        [TestMethod]
        public void TransformToViewModelCorrectlyTransformsATweet()
        {
            TweetViewModel tweetVM = ExistingService.TransformToViewModel(ExistingTweet);
            Assert.AreEqual(ExistingTweet.Content, tweetVM.Content, "Tweet was not correctly transformed");
        }
    }
}