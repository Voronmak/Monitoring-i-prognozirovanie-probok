using System.Configuration;
using System.Data;
using System.Windows;

namespace Мониторинг_и_прогнозирование_пробок
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            loginWindow login = new loginWindow();
            login.Show();
        }

    }

}
