﻿namespace TweetService.ViewModels
{
    public class TweetViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int User { get; set; }
        public List<LikesViewModel> Likes { get; set; }
        public DateTime Date { get; set; }
    }
}
