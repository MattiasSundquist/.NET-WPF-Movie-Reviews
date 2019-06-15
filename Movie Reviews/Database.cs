using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie_Reviews
{
    class Database
    {
        private IMongoDatabase database;
        private IMongoCollection<Movie> colMovieReviews;

        public Database()
        {
            // Connect to mongodb server running on 'localhost:27017' and get/create database 'MovieReviews'
            database = new MongoClient().GetDatabase("MovieReviews");
            colMovieReviews = database.GetCollection<Movie>("MovieReview");
        }

        public List<Movie> SearchMovieTitle(string title)
        {
            var filter = new BsonDocument { { "Title", new BsonDocument { { "$regex", title }, { "$options", "i" } } } };

            var result = colMovieReviews.Find(filter).ToList();

            return result;
        }

        public List<Movie> GetMovieByTitle(string title)
        {
            return colMovieReviews.Find(x => x.Title==title).ToList();
        }

        public void DeleteMovie(Movie movie)
        {
            colMovieReviews.DeleteOne(x => x.Id == movie.Id);
        }

        public void UpdateMovie(Movie movie)
        {
            DeleteMovie(movie);
            colMovieReviews.InsertOne(movie);
        }

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

        public List<Movie> GetAllMovieReview()
        {
            return colMovieReviews.Find(x => true).ToList();
        }
    }
}
