using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TweetService.Models
{
    public class Tweet
    {
        public int Id { get; set; }

        [StringLength(140, MinimumLength = 2)]
        [MaxLength]
        public string Content { get; set; }
        public string User { get; set; }
        public List<Likes> Likes { get; set; }

        public DateTime Date { get; set; }
        public Tweet(string content, string user)
        {
            Content = content;
            User = user;
            Date = DateTime.Now;
            Likes = new List<Likes>();
        }
    }
}
