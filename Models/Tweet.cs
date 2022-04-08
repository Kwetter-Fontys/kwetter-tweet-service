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
        public int User { get; set; }
        public int Likes { get; set; }

        public DateTime Date { get; set; }
        public Tweet(string content, int user)
        {
            Content = content;
            User = user;
            Likes = 0;
            Date = DateTime.Now;
        }
    }
}
