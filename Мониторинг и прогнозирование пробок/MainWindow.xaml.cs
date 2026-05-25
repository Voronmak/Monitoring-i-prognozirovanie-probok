using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Мониторинг_и_прогнозирование_пробок.Data;

namespace Мониторинг_и_прогнозирование_пробок
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DBContext db;
        public MainWindow()
        {
            InitializeComponent();

            db = new DBContext();
            db.Database.EnsureCreated();

            LoadAllData();
        }

        private void LoadAllData()
        {
            LoadHistorySpeeds();
            LoadPrognozSpeeds();
            LoadTypeDays();
        }

        private void LoadHistorySpeeds()
        {
            lstskrorost.ItemsSource = db.HistorySpeeds.ToList();
            lstskrorost.DisplayMemberPath = "DataVremya";
        }

        private void LoadPrognozSpeeds()
        {
            Prognozskrorost.ItemsSource = db.PrognozSpeeds.ToList();
            Prognozskrorost.DisplayMemberPath = "ID";
        }

        private void LoadTypeDays()
        {
            TypeDay.ItemsSource = db.TypeDays.ToList();
            TypeDay.DisplayMemberPath = "Name";
        }

    }
}