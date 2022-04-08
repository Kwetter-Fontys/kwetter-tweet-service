using TweetService.Models;
using Microsoft.EntityFrameworkCore;

namespace TweetService.DAL
{
    public class TweetInitializer
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
                   new Tweet("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque interdum rutrum sodales. Nullam mattis fermentum libero, non volutpat.", 1),
                   new Tweet("Lorem", 1),
                   new Tweet("a", 1),
                   new Tweet("Tweet4", 1),
                   new Tweet("Tweet", 2),
                   new Tweet("Tweet", 3),
                   new Tweet("Tweet", 4),
                   new Tweet("Tweet", 5),
                   new Tweet("Tweet", 6),
                   new Tweet("Tweet", 7)
            };

            context.Tweets.AddRange(tweets);
            context.SaveChanges();

        }
    }
}
