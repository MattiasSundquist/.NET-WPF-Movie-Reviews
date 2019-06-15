using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.IO;
using RestSharp;
using Newtonsoft.Json;
using System.Windows.Controls;
using System.Collections;
using System.Web.UI.WebControls;

namespace Movie_Reviews
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Database database;
        private Movie movieToAdd;

        public MainWindow()
        {
            InitializeComponent();

            database = new Database();

            string[] comboBoxContent = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
            combo_score.ItemsSource = comboBoxContent;
        }

        private void DatagridAPI_DoubleClicked(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            Movie response = ParsingHandler.SearchExactTitle(FromDynamicToString(row.Item))[0];
            txt_edit_movieTitle.Text = response.Title;
            txt_edit_movieYear.Text = response.Year;
            txt_edit_moviePlot.Text = response.Plot;
            movieToAdd = response;
        }

        private void DatagridDatabase_DoubleClicked(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            if (row.Item is Movie)
            {
                var response = row.Item as Movie;
                txt_edit_movieTitle.Text = response.Title;
                txt_edit_movieYear.Text = response.Year;
                txt_edit_moviePlot.Text = response.Plot;
                txt_edit_movieReview.Text = response.Review;
                combo_score.SelectedItem = response.Score;
                movieToAdd = response;
            }
        }

        private string FromDynamicToString(dynamic jsonObject)
        {
            if (jsonObject.Title is string)
            {
                return (string)jsonObject.Title;
            } else
            {
                return null;
            }
        }

        private void btn_search_movie_Click(object sender, RoutedEventArgs e)
        {
            string searchQuery = txt_search_movie.Text;
            dataGridAPI.ItemsSource = ParsingHandler.SearchWithQuery(searchQuery);
            dataGridDatabase.ItemsSource = database.SearchMovieTitle(searchQuery);
        }

        private void btn_add_movieReview_Click(object sender, RoutedEventArgs e)
        {
            
            movieToAdd.Title = txt_edit_movieTitle.Text;
            movieToAdd.Year = txt_edit_movieYear.Text;
            movieToAdd.Plot = txt_edit_moviePlot.Text;
            movieToAdd.Review = txt_edit_movieReview.Text;
            movieToAdd.Score = (string) combo_score.SelectedItem;
            database.AddMovieReview(movieToAdd);
            ClearGUI();
        }

        private void ClearGUI()
        {
            txt_search_movie.Text = "";
            txt_edit_movieTitle.Text = "";
            txt_edit_movieYear.Text = "";
            txt_edit_moviePlot.Text = "";
            txt_edit_movieReview.Text = "";
            combo_score.SelectedIndex = -1;
            dataGridAPI.ItemsSource = null;
            dataGridDatabase.ItemsSource = null;
            movieToAdd = null;
        }
    }

    class ParsingHandler
    {
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
            List<Movie> movies = new List<Movie>{movie};
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

        public static string MakeRequestGetResponse(string url)
        {
            var client = new RestClient(url);
            var response = client.Execute(new RestRequest());
            return response.Content;
        }
    }
}
