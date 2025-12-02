using MovieCatalogApp.Data;
using RK02.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RK02.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            var context = FakeDatabase.GetContext();
            var user = context.Users.FirstOrDefault(u =>
                u.Username == username && u.Password == password);

            if (user != null)
            {
                OpenMoviesWindow(user);
            }
            else
            {
                ErrorTextBlock.Text = "Неверный логин или пароль";
            }
        }

        private void GuestButton_Click(object sender, RoutedEventArgs e)
        {
            OpenMoviesWindow(null);
        }

        private void OpenMoviesWindow(User user)
        {
            var moviesWindow = new MoviesWindow(user);
            moviesWindow.Show();
            this.Close();
        }
    }
}