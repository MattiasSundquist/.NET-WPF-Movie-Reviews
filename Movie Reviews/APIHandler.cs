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

        public static string MakeTitleRequest(string title)
        {
            string url = URL + "?t=" + title + APIKEY;
            var client = new RestClient(url);
            var response = client.Execute(new RestRequest());
            return response.Content;
        }

        public static string MakeTitleSearchRequest(string title)
        {
            string url = URL + "?s=" + title + APIKEY;
            var client = new RestClient(url);
            var response = client.Execute(new RestRequest());
            return response.Content;
        }

        public static List<Movie> SearchExactTitle(string title)
        {
            string parsed = APIHandler.MakeTitleRequest(title);
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
            List<Movie> movies = new List<Movie> { movie };
            return new List<Movie> { movie };
        }

        public static IList SearchWithQuery(string query)
        {
            string parsed = APIHandler.MakeTitleSearchRequest(query);
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
