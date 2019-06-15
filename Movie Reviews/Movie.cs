using MongoDB.Bson;

namespace Movie_Reviews
{
    class Movie
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string Plot { get; set; }
        public string Review { get; set; }
        public string Score { get; set; }

        public Movie(string title, string year, string plot, string review, string score)
        {
            Title = title;
            Year = year;
            Plot = plot;
            Review = review;
            Score = score;
        }

        public Movie(string title)
        {
            Title = title;
        }

    }
}