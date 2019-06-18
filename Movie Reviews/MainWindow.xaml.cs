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
            // Default method to initialize the GUI
            InitializeComponent();

            // Custom methos to initialize components in the GUI
            InitializeCustomComponents();

            // Connect to your database
            database = new Database();
        }

        /// <summary>
        /// Method is called when the user double-clicks the API datagrid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatagridAPI_DoubleClicked(object sender, MouseButtonEventArgs e)
        {
            // Get the row that was clicked
            DataGridRow row = sender as DataGridRow;
            // Search API for the exact movie title in the double-clicked row
            Movie movie = APIHandler.SearchExactTitle(FromDynamicToString(row.Item));
            // Copy API-response to the add/edit section of the GUI.
            CopyContentFromDatabase(movie);
        }

        /// <summary>
        /// Method is called when the user double-clicks the database datagrid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatagridDatabase_DoubleClicked(object sender, MouseButtonEventArgs e)
        {
            // Get the row that was clicked
            DataGridRow dgr = sender as DataGridRow;
            // Copy the selected movie to the add/edit section of the GUI.
            CopyContentFromDatabase(dgr.Item as Movie);
        }

        /// <summary>
        /// Helper function to populate the add/edit section of the GUI
        /// </summary>
        /// <param name="movie">The movies information will be used to populate components</param>
        private void CopyContentFromDatabase(Movie movie)
        {
            txt_edit_movieTitle.Text = movie.Title;
            txt_edit_movieYear.Text = movie.Year;
            txt_edit_moviePlot.Text = movie.Plot;
            txt_edit_movieReview.Text = movie.Review;
            combo_score.SelectedItem = movie.Score;
        }

        /// <summary>
        /// Helper function to convert a dynamic object to a string object.
        /// </summary>
        /// <param name="jsonObject">The dynamic object to convert</param>
        /// <returns></returns>
        private string FromDynamicToString(dynamic jsonObject)
        {
            return (string)jsonObject.Title;
        }

        /// <summary>
        /// Method is called when the "Search" button is clicked in the "Search title" section of the GUI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_search_movie_Click(object sender, RoutedEventArgs e)
        {
            string searchQuery = txt_search_movie.Text;

            // If the user has selected to only search API...
            if ((string)combo_search_movie.SelectedValue == "Search API")
            {
                dataGridAPI.ItemsSource = APIHandler.SearchWithQuery(searchQuery);
                return;
            }

            // If the user has selected to only search database...
            else if ((string)combo_search_movie.SelectedValue == "Search database")
            {
                dataGridDatabase.ItemsSource = database.SearchMovieTitle(searchQuery);
                return;
            }

            // Default setting, search both API and database.
            dataGridAPI.ItemsSource = APIHandler.SearchWithQuery(searchQuery);
            dataGridDatabase.ItemsSource = database.SearchMovieTitle(searchQuery);
        }

        /// <summary>
        /// Method is called when the user click the "Add Review" button in the add/edit section of the GUI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_add_movieReview_Click(object sender, RoutedEventArgs e)
        {
            // Create a new movie object.
            Movie movieToAdd = new Movie(
                txt_edit_movieTitle.Text,
                txt_edit_movieYear.Text,
                txt_edit_moviePlot.Text,
                txt_edit_movieReview.Text,
                (string)combo_score.SelectedItem);

            // Add the movie object to the database.
            database.AddMovieReview(movieToAdd);

            // Clear the GUI.
            ClearGUI();
        }

        /// <summary>
        /// Clears the GUI.
        /// </summary>
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

        /// <summary>
        /// Helper function. Initializes custom components.
        /// </summary>
        private void InitializeCustomComponents()
        {
            string[] comboBoxContent = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
            combo_score.ItemsSource = comboBoxContent;

            comboBoxContent = new string[] { "Search both", "Search API", "Search database" };
            combo_search_movie.ItemsSource = comboBoxContent;
            combo_search_movie.SelectedIndex = 0;
        }

        /// <summary>
        /// Method is called when the user clicks the "Clear" button in the add/edit section of the GUI
        /// Clears the add/edit section of the GUI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_clear_edit_Click(object sender, RoutedEventArgs e)
        {
            txt_edit_movieTitle.Text = "";
            txt_edit_movieYear.Text = "";
            txt_edit_moviePlot.Text = "";
            txt_edit_movieReview.Text = "";
            combo_score.SelectedIndex = -1;
        }

        /// <summary>
        /// Method is called when the user clicks on the "Copy to edit" button either below the API datagrid or below the database datagrid.
        /// Copies the movie information from the datagrid to the add/edit section of the GUI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyToEdit_Clicked(object sender, RoutedEventArgs e)
        {

            Button btn = (Button)sender;

            // If button belongs to API datagrid...
            if (btn.Name == "btn_copy_API" && dataGridAPI.SelectedItem != null)
            {
                dynamic dynObj = dataGridAPI.SelectedItem;
                Movie movie = APIHandler.SearchExactTitle(dynObj.Title)[0];
                CopyContentFromDatabase(movie);
            }

            // If button belongs to database datagrid...
            else if (btn.Name == "btn_copy_database" && dataGridDatabase.SelectedItem != null)
            {
                Movie movie = dataGridDatabase.SelectedItem as Movie;
                CopyContentFromDatabase(movie);
            }
        }

        /// <summary>
        /// Method is called when the user clicks on the "Delete" button below the database datagrid.
        /// Deletes the selected movie from the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_delete_database_Click(object sender, RoutedEventArgs e)
        {
            Movie movie = dataGridDatabase.SelectedItem as Movie;
            MessageBoxResult mbr = MessageBox.Show("Are you sure that you want to delete " + movie.Title + " from the database?", "Attention", MessageBoxButton.YesNo);
            if (mbr == MessageBoxResult.Yes)
                database.DeleteMovie(movie);
        }

        /// <summary>
        /// Method is called when the user clicks the "Show all" button below the database datagrid.
        /// Displays all the movies in the database to the datagrid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_showAll_database_Click(object sender, RoutedEventArgs e)
        {
            dataGridDatabase.ItemsSource = database.GetAllMovieReview();
        }
    }
}
