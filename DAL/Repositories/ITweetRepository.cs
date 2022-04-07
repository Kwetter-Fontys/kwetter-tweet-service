using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetService.Models;

namespace TweetService.DAL.Repositories
{
    public interface ITweetRepository
    {
        List<Tweet> GetTweets();
        Tweet GetTweet(int id);

        Tweet LikeTweet(int id, Tweet tweet);
    }
}
