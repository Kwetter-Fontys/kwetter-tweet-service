using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TweetService.Models
{
    public class Tweet
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; }
        public int User { get; set; }
        public int Likes { get; set; }

        public Tweet(string content, int user)
        {
            Content = content;
            Date = DateTime.Now;
            User = user;
            Likes = 0;
        }
    }
}
