using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Мониторинг_и_прогнозирование_пробок.Models;
using Мониторинг_и_прогнозирование_пробок.Data;

namespace Мониторинг_и_прогнозирование_пробок
{
    /// <summary>
    /// Логика взаимодействия для loginWindow.xaml
    /// </summary>
    public partial class loginWindow : Window
    {
        private DBContext db = new DBContext();
        public loginWindow()
        {
            InitializeComponent();

            if (!db.Users.Any())
            {
                db.Users.Add(new Models.User
                {
                    Login = "admin",
                    Password = "adminsuperpro"
                });
                db.SaveChanges();

                db.Users.Add(new Models.User
                {
                    Login = "user",
                    Password = "usernotpro"
                });
                db.SaveChanges();
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var user = db.Users.FirstOrDefault(u =>
                u.Login == txtLogin.Text && u.Password == txtPassword.Password);

            if (user != null)
            {

                MainWindow mainWindow = new MainWindow(user.Login);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                txtError.Text = "Неверный логин или пароль!";
            }
        }
    }
}
