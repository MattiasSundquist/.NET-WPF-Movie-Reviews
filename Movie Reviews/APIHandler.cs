using Newtonsoft.Json;
using RestSharp;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Movie_Reviews
{
    class APIHandler
    {
        private const string URL = "http://www.omdbapi.com/";
        private const string APIKEY = "&apikey=fedd7355";

        /// <summary>
        /// Searches the API for a movie title that matches exactly
        /// </summary>
        /// <param name="title">The movie title to search for</param>
        /// <returns>A JSON string with all the API information about the movie</returns>
        public static string MakeTitleRequest(string title)
        {
            string url = URL + "?t=" + title + APIKEY;
            var client = new RestClient(url);
            var response = client.Execute(new RestRequest());
            return response.Content;
        }

        /// <summary>
        /// Searches the API for movie titles that are like the search query
        /// </summary>
        /// <param name="title">The title to search for</param>
        /// <returns>A JSON string all the API information about all the movies</returns>
        public static string MakeTitleSearchRequest(string title)
        {
            string url = URL + "?s=" + title + APIKEY;
            var client = new RestClient(url);
            var response = client.Execute(new RestRequest());
            return response.Content;
        }

        /// <summary>
        /// Searches the API for a movie title that matches exactly.
        /// Converts the string response to a movie object
        /// </summary>
        /// <param name="title">The title to search for</param>
        /// <returns>Returns a movie object</returns>
        public static Movie SearchExactTitle(string title)
        {
            string parsed = MakeTitleRequest(title);
            JsonTextReader reader = new JsonTextReader(new StringReader(parsed));
            Movie movie = new Movie(title);
            string previousValue = "";
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (previousValue == "Year")
                    {
                        movie.Year = reader.Value.ToString();
                    }
                    if (previousValue == "Plot")
                    {
                        movie.Plot = reader.Value.ToString();
                    }
                    previousValue = reader.Value.ToString();
                }
            }
            return movie;
        }

        /// <summary>
        /// Searches the API for movie titles that are like the search query
        /// Converts the string response to a list of objects with titles and years.
        /// </summary>
        /// <param name="query">The title to search for</param>
        /// <returns>A list of objects with titles and years.</returns>
        public static IList SearchWithQuery(string query)
        {
            string parsed = MakeTitleSearchRequest(query);
            JsonTextReader reader = new JsonTextReader(new StringReader(parsed));
            List<Movie> titles = new List<Movie>();
            string previousValue = "";
            int indexCounter = 0;
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (previousValue == "Title")
                    {
                        titles.Insert(indexCounter, new Movie(reader.Value.ToString()));
                    }
                    if (previousValue == "Year")
                    {
                        titles[indexCounter].Year = reader.Value.ToString();
                        indexCounter++;
                    }
                    previousValue = reader.Value.ToString();
                }
            }

            return titles.Select(x => new { x.Title, x.Year }).ToList();
        }
    }
}
