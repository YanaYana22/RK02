using MovieCatalogApp.Data;
using RK02.Models;
using RK02.Views;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RK02.Views
{
    public partial class MoviesWindow : Window
    {
        private readonly FakeDatabase _context;
        private readonly User _currentUser;
        private List<Movie> _allMovies;
        private List<Movie> _filteredMovies;

        public MoviesWindow(User user)
        {
            InitializeComponent();
            _context = FakeDatabase.GetContext();
            _currentUser = user;
            _allMovies = _context.Movies;
            _filteredMovies = new List<Movie>(_allMovies);

            InitializeUI();
            LoadMovies();
        }

        private void InitializeUI()
        {
            if (_currentUser == null)
            {
                UserInfoLabel.Content = "Гость";
                ControlPanel.Visibility = Visibility.Collapsed;
                AdminPanel.Visibility = Visibility.Collapsed;
            }
            else if (_currentUser.RoleId == 1)
            {
                UserInfoLabel.Content = $"Администратор: {_currentUser.Username}";
                ControlPanel.Visibility = Visibility.Visible;
                AdminPanel.Visibility = Visibility.Visible;
            }
            else
            {
                UserInfoLabel.Content = $"Пользователь: {_currentUser.Username}";
                ControlPanel.Visibility = Visibility.Visible;
                AdminPanel.Visibility = Visibility.Collapsed;
            }

            GenreComboBox.Items.Add("Все жанры");
            foreach (var genre in _context.Genres)
            {
                GenreComboBox.Items.Add(genre.Name);
            }
            GenreComboBox.SelectedIndex = 0;
        }

        private void LoadMovies()
        {
            MoviesDataGrid.ItemsSource = null;
            MoviesDataGrid.ItemsSource = _filteredMovies;
            StatusTextBlock.Text = $"Всего фильмов: {_filteredMovies.Count}";
        }

        private void MoviesDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (e.Row.Item is Movie movie)
            {
                if (movie.Rating > 8.9)
                {
                    e.Row.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E8B57"));
                    e.Row.Foreground = Brushes.White;
                }
                else
                {
                    e.Row.ClearValue(DataGridRow.BackgroundProperty);
                    e.Row.ClearValue(DataGridRow.ForegroundProperty);
                }
            }
        }

        private void ApplyFilters()
        {
            _filteredMovies = new List<Movie>(_allMovies);

            if (_currentUser != null && !string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                string searchText = SearchTextBox.Text.ToLower();
                _filteredMovies = _filteredMovies
                    .Where(m => m.Title.ToLower().Contains(searchText))
                    .ToList();
            }

            if (_currentUser != null && GenreComboBox.SelectedIndex > 0)
            {
                string selectedGenre = GenreComboBox.SelectedItem.ToString();
                _filteredMovies = _filteredMovies
                    .Where(m => m.Genre?.Name == selectedGenre)
                    .ToList();
            }

            if (_currentUser != null)
            {
                switch ((SortComboBox.SelectedItem as ComboBoxItem)?.Content.ToString())
                {
                    case "По рейтингу ↑":
                        _filteredMovies = _filteredMovies
                            .OrderBy(m => m.Rating)
                            .ToList();
                        break;
                    case "По рейтингу ↓":
                        _filteredMovies = _filteredMovies
                            .OrderByDescending(m => m.Rating)
                            .ToList();
                        break;
                }
            }

            LoadMovies();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_currentUser != null)
                ApplyFilters();
        }

        private void GenreComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_currentUser != null)
                ApplyFilters();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_currentUser != null)
                ApplyFilters();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddEditMovieWindow(_context, null);
            if (dialog.ShowDialog() == true)
            {
                _allMovies = _context.Movies;
                ApplyFilters();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MoviesDataGrid.SelectedItem is Movie selectedMovie)
            {
                var result = MessageBox.Show($"Удалить фильм '{selectedMovie.Title}'?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _context.Remove(selectedMovie);
                    _context.SaveChanges();
                    _allMovies = _context.Movies;
                    ApplyFilters();
                }
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}