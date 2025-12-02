using System.Windows;

namespace RK02
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var context = MovieCatalogApp.Data.FakeDatabase.GetContext();
        }
    }
}