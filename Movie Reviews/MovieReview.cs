namespace Movie_Reviews
{
    class MovieReview
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public string Review { get; set; }
        public int Score { get; set; }

        public MovieReview(string title, string description, string review, int score)
        {
            Title = title;
            Description = description;
            Review = review;
            Score = score;
        }

    }
}