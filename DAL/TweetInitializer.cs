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
                   new Tweet("Tweet1", 1),
                   new Tweet("Tweet2", 1),
                   new Tweet("Tweet3", 1),
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
