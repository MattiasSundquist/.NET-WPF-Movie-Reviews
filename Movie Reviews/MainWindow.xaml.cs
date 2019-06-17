using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System;

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
            InitializeCustomComponents();

            database = new Database();
        }

        private void DatagridAPI_DoubleClicked(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            Movie movie = APIHandler.SearchExactTitle(FromDynamicToString(row.Item))[0];
            CopyContentFromDatabase(movie);
        }

        private void DatagridDatabase_DoubleClicked(object sender, MouseButtonEventArgs e)
        {
            DataGridRow dgr = sender as DataGridRow;
            CopyContentFromDatabase(dgr.Item as Movie);
        }

        private void CopyContentFromDatabase(Movie movie)
        {
            txt_edit_movieTitle.Text = movie.Title;
            txt_edit_movieYear.Text = movie.Year;
            txt_edit_moviePlot.Text = movie.Plot;
            txt_edit_movieReview.Text = movie.Review;
            combo_score.SelectedItem = movie.Score;
        }

        private string FromDynamicToString(dynamic jsonObject)
        {
            return (string)jsonObject.Title;
        }

        private void btn_search_movie_Click(object sender, RoutedEventArgs e)
        {
            string searchQuery = txt_search_movie.Text;
            if ((string) combo_search_movie.SelectedValue == "Search both")
            {
                dataGridAPI.ItemsSource = APIHandler.SearchWithQuery(searchQuery);
                dataGridDatabase.ItemsSource = database.SearchMovieTitle(searchQuery);
            } else if ((string)combo_search_movie.SelectedValue == "Search API")
                dataGridAPI.ItemsSource = APIHandler.SearchWithQuery(searchQuery);
            else
                dataGridDatabase.ItemsSource = database.SearchMovieTitle(searchQuery);
        }

        private void btn_add_movieReview_Click(object sender, RoutedEventArgs e)
        {
            Movie movieToAdd = new Movie(
                txt_edit_movieTitle.Text,
                txt_edit_movieYear.Text,
                txt_edit_moviePlot.Text,
                txt_edit_movieReview.Text,
                (string)combo_score.SelectedItem);

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
        }

        private void InitializeCustomComponents()
        {
            string[] comboBoxContent = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
            combo_score.ItemsSource = comboBoxContent;

            comboBoxContent = new string[] { "Search both", "Search API", "Search database" };
            combo_search_movie.ItemsSource = comboBoxContent;
            combo_search_movie.SelectedIndex = 0;
        }

        private void btn_clear_edit_Click(object sender, RoutedEventArgs e)
        {
            ClearEditSection();
        }

        private void ClearEditSection()
        {
            txt_edit_movieTitle.Text = "";
            txt_edit_movieYear.Text = "";
            txt_edit_moviePlot.Text = "";
            txt_edit_movieReview.Text = "";
            combo_score.SelectedIndex = -1;
        }

        private void CopyToEdit_Clicked(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Name == "btn_copy_API" && dataGridAPI.SelectedItem != null)
            {
                dynamic dynObj = dataGridAPI.SelectedItem;
                Movie movie = APIHandler.SearchExactTitle(dynObj.Title)[0];
                CopyContentFromDatabase(movie);
            }
            else if (btn.Name == "btn_copy_database" && dataGridDatabase.SelectedItem != null)
            {
                Movie movie = dataGridDatabase.SelectedItem as Movie;
                CopyContentFromDatabase(movie);
            }
        }
    }
}
