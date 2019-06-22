using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;

namespace Movie_Reviews
{
    class Database
    {
        private readonly IMongoCollection<Movie> colMovieReviews;

        public Database()
        {
            //var database = CreateCloudDatabase();

            // Connect to mongodb server running on 'localhost:27017' and get/create database 'MovieReviews'
            var database = new MongoClient().GetDatabase("MovieReviews");

            // Create a collection called "MovieReview"
            colMovieReviews = database.GetCollection<Movie>("MovieReview");
        }

        private IMongoDatabase CreateCloudDatabase()
        {
            string connectionString = @"INSERT YOUR OWN CONNECTION STRING HERE";
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            var database = new MongoClient(settings).GetDatabase("MovieReviews");
            return database;
        }

        /// <summary>
        /// Searches for a movie title that contains the search query
        /// </summary>
        /// <param name="title">The title to search for</param>
        /// <returns></returns>
        public List<Movie> SearchMovieTitle(string title)
        {
            var filter = new BsonDocument { { "Title", new BsonDocument { { "$regex", title }, { "$options", "i" } } } };

            var result = colMovieReviews.Find(filter).ToList();

            return result;
        }

        /// <summary>
        /// Searches for a movie title that matches exactly with the query.
        /// </summary>
        /// <param name="title">The title to search for</param>
        /// <returns></returns>
        public List<Movie> GetMovieByTitle(string title)
        {
            return colMovieReviews.Find(x => x.Title==title).ToList();
        }

        /// <summary>
        /// Deletes a movie from the collection
        /// </summary>
        /// <param name="movie">The movie to delete from the collection</param>
        public void DeleteMovie(Movie movie)
        {
            colMovieReviews.DeleteOne(x => x.Id == movie.Id);
        }

        /// <summary>
        /// Updates a movie in the collection
        /// </summary>
        /// <param name="movie">The movie to update</param>
        public void UpdateMovie(Movie movie)
        {
            DeleteMovie(movie);
            colMovieReviews.InsertOne(movie);
        }

        /// <summary>
        /// Adds/updates a movie to the collection
        /// </summary>
        /// <param name="review">The movie to add</param>
        public void AddMovieReview(Movie review)
        {
            if (GetMovieByTitle(review.Title) != null)
            {
                UpdateMovie(review);
            }
            else
            {
                colMovieReviews.InsertOne(review);
            }
        }

        /// <summary>
        /// Gets all the movies from the collection
        /// </summary>
        /// <returns>A list of movies</returns>
        public List<Movie> GetAllMovieReview()
        {
            return colMovieReviews.Find(x => true).ToList();
        }
    }
}
