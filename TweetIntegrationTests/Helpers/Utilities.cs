using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TweetService.DAL;
using TweetService.Models;

namespace TweetIntegrationTests.Helpers
{
    public static class Utilities
    {
        #region snippet1
        public static void InitializeDbForTests(TweetContext db)
        {
            db.Tweets.AddRange(GetSeedingMessages());
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(TweetContext db)
        {
            db.Tweets.RemoveRange(db.Tweets);
            InitializeDbForTests(db);
        }

        public static List<Tweet> GetSeedingMessages()
        {
            return new List<Tweet>
            {
                //140 characters exactly for testing
                //If put in more you get an error
                   new Tweet("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque interdum rutrum sodales. Nullam mattis fermentum libero, non volutpat.", "bf40cabc-3cc7-49bb-aeba-cd1c6ab23dcc"),
                   new Tweet("Lorem", "bf40cabc-3cc7-49bb-aeba-cd1c6ab23dcc"),
                   new Tweet("a", "bf40cabc-3cc7-49bb-aeba-cd1c6ab23dcc"),
                   new Tweet("Tweet4", "bf40cabc-3cc7-49bb-aeba-cd1c6ab23dcc"),
                   new Tweet("Tweet", "c888f6c2-d4ce-442f-b630-52a91150f22a"),
                   new Tweet("Tweet", "1"),
                   new Tweet("Tweet", "2"),
                   new Tweet("Tweet", "3"),
                   new Tweet("Tweet", "4"),
                   new Tweet("Tweet", "5")
            };

        }
        #endregion
    }
}
