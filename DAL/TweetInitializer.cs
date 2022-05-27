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
                   new Tweet("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque interdum rutrum sodales. Nullam mattis fermentum libero, non volutpat.", "7cc35fc6-0eaf-4df8-aaef-773077b4f3c9"),
                   new Tweet("Lorem", "7cc35fc6-0eaf-4df8-aaef-773077b4f3c9"),
                   new Tweet("a", "7cc35fc6-0eaf-4df8-aaef-773077b4f3c9"),
                   new Tweet("Tweet4", "7cc35fc6-0eaf-4df8-aaef-773077b4f3c9"),
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
