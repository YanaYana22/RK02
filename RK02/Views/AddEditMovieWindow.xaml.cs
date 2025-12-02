using System;
using System.Linq;
using System.Windows;
using MovieCatalogApp.Data;
using RK02.Models;

namespace RK02.Views
{
    public partial class AddEditMovieWindow : Window
    {
        private readonly FakeDatabase _context;
        private readonly Movie _movie;

        public AddEditMovieWindow(FakeDatabase context, Movie movie)
        {
            InitializeComponent();
            _context = context;
            _movie = movie;

            if (_movie != null)
            {
                Title = "Редактировать фильм";
                TitleTextBox.Text = _movie.Title;
                YearTextBox.Text = _movie.Year.ToString();
                RatingTextBox.Text = _movie.Rating.ToString("F1");
            }

            GenreComboBox.ItemsSource = _context.Genres;

            if (_movie != null)
            {
                GenreComboBox.SelectedValue = _movie.GenreId;
            }
            else if (GenreComboBox.Items.Count > 0)
            {
                GenreComboBox.SelectedIndex = 0;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                var selectedGenre = GenreComboBox.SelectedItem as Genre;

                if (selectedGenre == null)
                {
                    ErrorTextBlock.Text = "Выберите жанр";
                    return;
                }

                if (_movie == null)
                {
                    var newMovie = new Movie
                    {
                        Title = TitleTextBox.Text.Trim(),
                        Year = int.Parse(YearTextBox.Text),
                        GenreId = selectedGenre.Id,
                        Rating = double.Parse(RatingTextBox.Text)
                    };

                    _context.Add(newMovie);
                }
                else
                {
                    _movie.Title = TitleTextBox.Text.Trim();
                    _movie.Year = int.Parse(YearTextBox.Text);
                    _movie.GenreId = selectedGenre.Id;
                    _movie.Rating = double.Parse(RatingTextBox.Text);
                    _movie.Genre = selectedGenre;

                    _context.Update(_movie);
                }

                _context.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                ErrorTextBlock.Text = $"Ошибка при сохранении: {ex.Message}";
            }
        }

        private bool ValidateInput()
        {
            ErrorTextBlock.Text = "";

            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                ErrorTextBlock.Text = "Введите название фильма";
                return false;
            }

            if (!int.TryParse(YearTextBox.Text, out int year))
            {
                ErrorTextBlock.Text = "Введите корректный год";
                return false;
            }

            if (year < 1888 || year > DateTime.Now.Year + 5)
            {
                ErrorTextBlock.Text = $"Введите год от 1888 до {DateTime.Now.Year + 5}";
                return false;
            }

            if (!double.TryParse(RatingTextBox.Text, out double rating))
            {
                ErrorTextBlock.Text = "Введите корректный рейтинг";
                return false;
            }

            if (rating < 0 || rating > 10)
            {
                ErrorTextBlock.Text = "Введите рейтинг от 0 до 10";
                return false;
            }

            if (GenreComboBox.SelectedItem == null)
            {
                ErrorTextBlock.Text = "Выберите жанр";
                return false;
            }

            return true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}