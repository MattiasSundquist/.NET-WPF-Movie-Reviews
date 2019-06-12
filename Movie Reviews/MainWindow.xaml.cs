using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Net;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Movie_Reviews
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Database database;

        public MainWindow()
        {
            InitializeComponent();
            //database = new Database();
            //FillDataGridWithAllReviews();

            string response = APIHandler.MakeTitleRequest("Lord");
            var values = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(response);
            JArray stuff = values["Search"];
            string content = "";
            foreach (JToken token in stuff)
            {
                foreach (JToken anotherToken in token)
                {
                    content += anotherToken.First + "\t FIRST!!! \n";
                }
            }

            txtBlock.Text = content;

            //foreach (KeyValuePair<string, dynamic> entry in values)
            //{
            //    content += "KEY: " + entry.Key + "\tVALUE: " + entry.Value + "\n\n\n";
            //}
            //txtBlock.Text = content;

        }

        private void FillDataGridWithAllReviews()
        {
            List<MovieReview> movieReviews = database.GetAllMovieReview();
            var listWithoutCol = movieReviews.Select(x => new { x.Title, x.Description, x.Score }).ToList();
            dataGrid.ItemsSource = listWithoutCol;
        }

        private void Datagrid_DoubleClicked(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Datagrid was double-clicked!");
        }
    }

    class APIHandler
    {
        
        private const string URL = "http://www.omdbapi.com/";
        private const string APIKEY = "&apikey=fedd7355";
        
        public static string MakeTitleRequest(string title)
        {
            string url = URL + "?s=" + title + APIKEY;

            var client = new RestClient(url);

            var response = client.Execute(new RestRequest());

            return response.Content;
        }

        public static string MakeRequestGetResponse(string url)
        {
            var client = new RestClient(url);

            var response = client.Execute(new RestRequest());

            return response.Content;
        }
    }
}
