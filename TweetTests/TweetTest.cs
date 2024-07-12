using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
        public List<TweetViewModel> TweetList = new List<TweetViewModel>();

        public Tweet ExistingTweet = new Tweet("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque interdum rutrum sodales. Nullam mattis fermentum libero, non volutpat.", "7cc35fc6-0eaf-4df8-aaef-773077b4f3c9") { Id = 0, Date = new DateTime(2008, 5, 1, 8, 30, 52) };
        //Used when no modifications are needed.
        public TweetServiceClass ExistingService;
        public ILogger<TweetServiceClass> logger;

        public TweetTest()
        {
            var mock = new Mock<ILogger<TweetServiceClass>>();
            logger = mock.Object;
            ExistingService = new TweetServiceClass(new MockTweetRepository(), logger);
        }

        public TweetServiceClass CreateNewService()
        {
            TweetServiceClass newSerivce = new TweetServiceClass(new MockTweetRepository(), logger);
            return newSerivce;
        }

    //Testing the GetTweet method

    [TestMethod]
        public void GetTweetsFromExistingUserRetunsListOfTweets()
        {
            TweetList = ExistingService.GetTweetsFromUser(MainUserId);
            //ExpectedList, ActualList
            Assert.AreEqual(4, TweetList.Count, "List count is not equal, when it should be");
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
            TweetServiceClass service = CreateNewService();
            TweetViewModel tweet = service.LikeTweet(0, MainUserId);
            Assert.AreEqual(1, tweet.Likes.Count, "Likes count is not equal to 1, on existing tweet");
        }


        [TestMethod]
        public void LikeTweetDoesntAddLikeToNonTweet()
        {
            TweetServiceClass service = CreateNewService();
            TweetViewModel tweet = service.LikeTweet(100, MainUserId);
            Assert.AreEqual(null, tweet.Likes, "Likes count is not null on non existing tweet");
        }

        [TestMethod]
        public void LikeTweetWithExistingLikes()
        {
            TweetServiceClass service = CreateNewService();
            service.LikeTweet(0, MainUserId);
            TweetViewModel tweet = service.LikeTweet(0, "userid");
            Assert.AreEqual(2, tweet.Likes.Count, "Likes count is not equal to 2, on existing tweet when liked twice");
        }

        [TestMethod]
        public void LikeTweetMultipleTimesWithSameUser()
        {
            TweetServiceClass service = CreateNewService();
            service.LikeTweet(0, MainUserId);
            TweetViewModel tweet = service.LikeTweet(0, MainUserId);
            Assert.AreEqual(1, tweet.Likes.Count, "Likes count is not equal to 1 when liking 2 times on same account on same tweet");
        }

        //Testing the PostTweet method

        [TestMethod]
        public void PostTweetWithEqualUserIdAndUserTokenId()
        {
            TweetServiceClass service = CreateNewService();
            TweetViewModel tweet = service.PostTweet(ExistingTweet, MainUserId);
            Assert.AreEqual(MainUserId, tweet.User, "Tweet wasn't posted when using correct user ids");
        }

        [TestMethod]
        public void PostTweetWithNonEqualUserIdAndUserTokenId()
        {
            TweetServiceClass service = CreateNewService();
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

        //Testing the DeleteTweet method

        [TestMethod]
        public void DeleteTweetsFromExistingUser()
        {
            TweetServiceClass service = CreateNewService();
            service.DeleteTweets(MainUserId);
            TweetList = service.GetTweetsFromUser(MainUserId);
            //ExpectedList, ActualList
            Assert.AreEqual(0, TweetList.Count, "List count is not equal, when it should be");
        }

        [TestMethod]
        public void DeleteTweetsFromNonExistingUser()
        {
            TweetServiceClass service = CreateNewService();
            service.DeleteTweets("test");
            TweetList = service.GetTweetsFromUser("test");
            //ExpectedList, ActualList
            Assert.AreEqual(0, TweetList.Count, "List count is not equal, when it should be");
        }
    }
}