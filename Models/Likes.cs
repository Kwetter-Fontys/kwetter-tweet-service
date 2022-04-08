namespace TweetService.Models
{
    public class Likes
    {
        public int LikesId { get; set; }

        public int User { get; set; }

        public int TweetId { get; set; }

        public Tweet Tweet { get; set; }

        public Likes(int user)
        {
            User = user;
        }
    }
}
