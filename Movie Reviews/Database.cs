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
        private IMongoCollection<MovieReview> colMovieReviews;

        public Database()
        {
            // Connect to mongodb server running on 'localhost:27017' and get/create database 'MovieReviews'
            database = new MongoClient().GetDatabase("MovieReviews");

            colMovieReviews = database.GetCollection<MovieReview>("MovieReview");

            //populateDatabase();
        }

        private void populateDatabase()
        {
            string title = "Lord Of The Rings: The Fellowship Of The Ring";
            string description = "An evil dude wants a magical ring that will allow him to rule the world. Some good dudes find the ring and decides to destroy it by throwing it into a volcano.";
            string review = "Cool movie, but why didn't the good guys just fly to volcano with those griffins at the beginning?";
            int score = 9;
            MovieReview mr1 = new MovieReview(title, description, review, score);
            colMovieReviews.InsertOne(mr1);

            Console.WriteLine("Database populated");
        }
    }
}
