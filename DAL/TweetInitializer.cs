using TweetService.Models;


namespace TweetService.DAL
{
    public static class TweetInitializer
    {
        public static void Initialize(TweetContext context)
        {
            //if (context.Users.Any())
            //{
                //return; //DB has been seeded already
            //}

            // Drops and creates new database with filler code. Useful for now but should be done differently later.
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            //Add some seeded Tweets

            List<Tweet> tweets = new List<Tweet>
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

            context.Tweets.AddRange(tweets);
            context.SaveChanges();

        }
    }
}
