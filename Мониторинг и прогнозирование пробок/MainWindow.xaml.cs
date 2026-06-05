using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Мониторинг_и_прогнозирование_пробок.Data;
using Мониторинг_и_прогнозирование_пробок.Models;

namespace Мониторинг_и_прогнозирование_пробок
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DBContext db;
        public MainWindow(string Login)
        {
            InitializeComponent();

            db = new DBContext();
            db.Database.EnsureCreated();

            if(Login == "user")
            {
                TabControlMain.SelectedItem = TabItemMonitoring;

                TabItemHist.Visibility = Visibility.Collapsed;
                TabItemPrognoz.Visibility = Visibility.Collapsed;
                TabItemType.Visibility = Visibility.Collapsed;
            }
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
        private void Lstskrorost_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstskrorost.SelectedItem is Историческая_скорость selected)
            {
                txtid.Text = selected.ID.ToString();
                txtdate.Text = selected.DataVremya.ToString("yyyy-MM-dd HH:mm:ss");
                txtspeed.Text = selected.SrScorost.ToString();
                txtspeedmedlp.Text = selected.ScorostMedlPotoka.ToString();
                txtspeedbystrp.Text = selected.ScorostBystrPotoka.ToString();
                txtIDPrognoza.Text = selected.IDPrognoza.ToString();
            }
        }

        private void Prognozskrorost_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Prognozskrorost.SelectedItem is Прогноз_скорости selected)
            {
                txtIDPr.Text = selected.ID.ToString();
                txtTime.Text = selected.PrognozaDataVremya.ToString("yyyy-MM-dd HH:mm:ss");
                txtDataSozd.Text = selected.DataSozdaniaPrognoza.ToString("yyyy-MM-dd HH:mm:ss");
                txtspeedpriprognoz.Text = selected.PrognozSpeed.ToString();
            }
        }

        private void TypeDay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TypeDay.SelectedItem is Тип_дня selected)
            {
                txtIDType.Text = selected.ID.ToString();
                txtDay.Text = selected.Name;
                txtPrazdnik.IsChecked = selected.prazdnik;
                txtNagruzka.Text = selected.Nagruzki.ToString();
                txtIDprognoza.Text = selected.IDPrognoz.ToString();
            }
        }

        #region ist
        private void IstSkorostAdd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string error = "";
            if (string.IsNullOrEmpty(txtid.Text))
            {
                error += "Введите ID исторической скорости\n";
            }
            if (string.IsNullOrEmpty(txtdate.Text))
            {
                error += "Введите дату исторической скорости\n";
            }
            if (string.IsNullOrEmpty(txtspeed.Text))
            {
                error += "Введите среднюю скорость\n";
            }
            if (string.IsNullOrEmpty(txtspeedmedlp.Text))
            {
                error += "Введите скорость медленного потока\n";
            }
            if (string.IsNullOrEmpty(txtspeedbystrp.Text))
            {
                error += "Введите скорость быстрого потока\n";
            }
            if (string.IsNullOrEmpty(txtIDPrognoza.Text))
            {
                error += "Введите ID прогноза\n";
            }
            if (error != "")
            {
                MessageBox.Show(error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var lstskrorost = new Историческая_скорость
                {
                    ID = Convert.ToInt32(txtid.Text),
                    DataVremya = Convert.ToDateTime(txtdate.Text),
                    SrScorost = Convert.ToDecimal(txtspeed.Text),
                    ScorostMedlPotoka = Convert.ToDecimal(txtspeedmedlp.Text),
                    ScorostBystrPotoka = Convert.ToDecimal(txtspeedbystrp.Text),
                    IDPrognoza = Convert.ToInt32(txtIDPrognoza.Text),
                };

                db.HistorySpeeds.Add(lstskrorost);
                db.SaveChanges();
                LoadAllData();
                ClearIstSkorost();
            }
            catch (Exception ex) { MessageBox.Show("Ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
        private void IstSkorostDel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (lstskrorost.SelectedItem is not Историческая_скорость selected)
            {
                MessageBox.Show("Выберите историческую скорость для удаления", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Удалить историческую скорость '{selected.ID}'", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    db.HistorySpeeds.Remove(selected);
                    db.SaveChanges();
                    LoadAllData();
                    ClearIstSkorost();

                    MessageBox.Show("Историческая скорость удалена!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex) { MessageBox.Show("Ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
            }
        }
        private void IstSkorostEdit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (lstskrorost.SelectedItem is not Историческая_скорость selected)
            {
                MessageBox.Show("Выберите историческую скорость для редактирования", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string error = "";
            if (string.IsNullOrEmpty(txtid.Text))
            {
                error += "Введите ID исторической скорости\n";
            }
            if (string.IsNullOrEmpty(txtdate.Text))
            {
                error += "Введите дату исторической скорости\n";
            }
            if (string.IsNullOrEmpty(txtspeed.Text))
            {
                error += "Введите среднюю скорость\n";
            }
            if (string.IsNullOrEmpty(txtspeedmedlp.Text))
            {
                error += "Введите скорость медленного потока\n";
            }
            if (string.IsNullOrEmpty(txtspeedbystrp.Text))
            {
                error += "Введите скорость быстрого потока\n";
            }
            if (string.IsNullOrEmpty(txtIDPrognoza.Text))
            {
                error += "Введите ID прогноза\n";
            }
            if (error != "")
            {
                MessageBox.Show(error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                selected.ID = Convert.ToInt32(txtid.Text);
                selected.DataVremya = Convert.ToDateTime(txtdate.Text);
                selected.SrScorost = Convert.ToDecimal(txtspeed.Text);
                selected.ScorostMedlPotoka = Convert.ToDecimal(txtspeedmedlp.Text);
                selected.ScorostBystrPotoka = Convert.ToDecimal(txtspeedbystrp.Text);
                selected.IDPrognoza = Convert.ToInt32(txtIDPrognoza.Text);

                db.SaveChanges();
                LoadAllData();
                ClearIstSkorost();
            }
            catch (Exception ex) { MessageBox.Show("Ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
        #endregion
        #region prognoz
        private void PrognozSkorostAdd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string error = "";
            if (string.IsNullOrEmpty(txtIDPr.Text))
            {
                error += "Введите ID прогноза скорости\n";
            }
            if (string.IsNullOrEmpty(txtTime.Text))
            {
                error += "Введите время прогноза скорости\n";
            }
            if (string.IsNullOrEmpty(txtDataSozd.Text))
            {
                error += "Введите дату оздания прогноза\n";
            }
            if (string.IsNullOrEmpty(txtspeedpriprognoz.Text))
            {
                error += "Введите скорость при прогнозе\n";
            }
            if (error != "")
            {
                MessageBox.Show(error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var PrognozSkorost = new Прогноз_скорости
                {
                    ID = Convert.ToInt32(txtIDPr.Text),
                    PrognozaDataVremya = Convert.ToDateTime(txtTime.Text),
                    DataSozdaniaPrognoza = Convert.ToDateTime(txtDataSozd.Text),
                    PrognozSpeed = Convert.ToInt32(txtspeedpriprognoz.Text),
                };

                db.PrognozSpeeds.Add(PrognozSkorost);
                db.SaveChanges();
                LoadAllData();
                ClearPrognozSkorost();

            }
            catch (Exception ex) { MessageBox.Show("Ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }


        }
        private void PrognozSkorostDel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Prognozskrorost.SelectedItem is not Прогноз_скорости selected)
            {
                MessageBox.Show("Выберите прогноз скорости для удаления", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Удалить прогноз скорости '{selected.ID}'", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    db.PrognozSpeeds.Remove(selected);
                    db.SaveChanges();
                    LoadAllData();
                    ClearPrognozSkorost();

                    MessageBox.Show("Прогноз скорости удален!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex) { MessageBox.Show("Ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
            }
        }
        private void PrognozSkorostEdit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Prognozskrorost.SelectedItem is not Прогноз_скорости selected)
            {
                MessageBox.Show("Выберите прогноз скорости для редактирования", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string error = "";
            if (string.IsNullOrEmpty(txtIDPr.Text))
            {
                error += "Введите ID прогноза скорости\n";
            }
            if (string.IsNullOrEmpty(txtTime.Text))
            {
                error += "Введите время прогноза скорости\n";
            }
            if (string.IsNullOrEmpty(txtDataSozd.Text))
            {
                error += "Введите дату оздания прогноза\n";
            }
            if (string.IsNullOrEmpty(txtspeedpriprognoz.Text))
            {
                error += "Введите скорость при прогнозе\n";
            }
            if (error != "")
            {
                MessageBox.Show(error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {

                selected.ID = Convert.ToInt32(txtIDPr.Text);
                selected.PrognozaDataVremya = Convert.ToDateTime(txtTime.Text);
                selected.DataSozdaniaPrognoza = Convert.ToDateTime(txtDataSozd.Text);
                selected.PrognozSpeed = Convert.ToInt32(txtspeedpriprognoz.Text);

                db.SaveChanges();
                LoadAllData();
                ClearPrognozSkorost();
            }
            catch (Exception ex) { MessageBox.Show("Ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
        #endregion
        #region TypeDay
        private void TypeDayAdd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string error = "";
            if (string.IsNullOrEmpty(txtIDType.Text))
            {
                error += "Введите ID типа дня\n";
            }
            if (string.IsNullOrEmpty(txtDay.Text))
            {
                error += "Введите день недели\n";
            }
            if (string.IsNullOrEmpty(txtNagruzka.Text))
            {
                error += "Введите нагрузку\n";
            }
            if (string.IsNullOrEmpty(txtIDprognoza.Text))
            {
                error += "Введите ID прогноза\n";
            }
            if (error != "")
            {
                MessageBox.Show(error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var TypeDay = new Тип_дня
                {
                    ID = Convert.ToInt32(txtIDType.Text),
                    Name = txtDay.Text,
                    prazdnik = (bool)txtPrazdnik.IsChecked,
                    Nagruzki = Convert.ToInt32(txtNagruzka.Text),
                    IDPrognoz = Convert.ToInt32(txtIDprognoza.Text),
                };

                db.TypeDays.Add(TypeDay);
                db.SaveChanges();
                LoadAllData();
                ClearTypeDay();
            }
            catch (Exception ex) { MessageBox.Show("Ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
        private void TypeDayDel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (TypeDay.SelectedItem is not Тип_дня selected)
            {
                MessageBox.Show("Выберите тип дня для удаления", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Удалить тип дня '{selected.ID}'", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    db.TypeDays.Remove(selected);
                    db.SaveChanges();
                    LoadAllData();
                    ClearTypeDay();

                    MessageBox.Show("Тип дня удален!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex) { MessageBox.Show("Ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
            }
        }
        private void TypeDayEdit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (TypeDay.SelectedItem is not Тип_дня selected)
            {
                MessageBox.Show("Выберите тип дня для редактирования", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string error = "";
            if (string.IsNullOrEmpty(txtIDType.Text))
            {
                error += "Введите ID типа дня\n";
            }
            if (string.IsNullOrEmpty(txtDay.Text))
            {
                error += "Введите день недели\n";
            }
            if (string.IsNullOrEmpty(txtNagruzka.Text))
            {
                error += "Введите нагрузку\n";
            }
            if (string.IsNullOrEmpty(txtIDprognoza.Text))
            {
                error += "Введите ID прогноза\n";
            }
            if (error != "")
            {
                MessageBox.Show(error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {

                selected.ID = Convert.ToInt32(txtIDType.Text);
                selected.Name = txtDay.Text;
                selected.prazdnik = (bool)txtPrazdnik.IsChecked;
                selected.Nagruzki = Convert.ToInt32(txtNagruzka.Text);
                selected.IDPrognoz = Convert.ToInt32(txtIDprognoza.Text);

                db.SaveChanges();
                LoadAllData();
                ClearTypeDay();
            }
            catch (Exception ex) { MessageBox.Show("Ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
        #endregion

        private void btnObnovi_Click(object sender, RoutedEventArgs e)
        {
            db.SaveChanges();
            LoadAllData();
        }

        private void ClearIstSkorost()
        {
            txtid.Text = "";
            txtdate.Text = "";
            txtspeed.Text = "";
            txtspeedmedlp.Text = "";
            txtspeedbystrp.Text = "";
            txtIDPrognoza.Text = "";

        }

        private void ClearPrognozSkorost()
        {
            txtIDPr.Text = "";
            txtTime.Text = "";
            txtDataSozd.Text = "";
            txtspeedpriprognoz.Text = "";
        }

        private void ClearTypeDay()
        {
            txtIDType.Text = "";
            txtDay.Text = "";
            txtPrazdnik.IsChecked = false;
            txtNagruzka.Text = "";
            txtIDprognoza.Text = "";
        }

        #region Мониторинг пробок

        private void StatistikButton_Click(object sender, RoutedEventArgs e)
        {
            ShowAverageSpeedByDay();
        }

        private void ShowAverageSpeedByDay()
        {
            try
            {
                var historyData = db.HistorySpeeds.ToList();

                if (!historyData.Any())
                {
                    MessageBox.Show("Нет данных в таблице исторической скорости",
                                  "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var result = new List<object>();

                string[] days = { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" };

                foreach (string day in days)
                {
                    int dayNumber = Array.IndexOf(days, day) + 1;
                    if (dayNumber == 7) dayNumber = 0;

                    var dayData = historyData.Where(h => (int)h.DataVremya.DayOfWeek == dayNumber).ToList();

                    if (dayData.Any())
                    {
                        double avgSpeed = (double)dayData.Average(h => h.SrScorost);

                        result.Add(new
                        {
                            День_недели = day,
                            Средняя_скорость = Math.Round(avgSpeed, 1),
                            Количество_записей = dayData.Count,
                            Минимальная_скорость = Math.Round((double)dayData.Min(h => h.SrScorost), 1),
                            Максимальная_скорость = Math.Round((double)dayData.Max(h => h.SrScorost), 1)
                        });
                    }
                }

                Type.ItemsSource = result;

                if (result.Any())
                {
                    var bestDay = result.OrderByDescending(x => x.GetType().GetProperty("Средняя_скорость").GetValue(x)).First();
                    var worstDay = result.OrderBy(x => x.GetType().GetProperty("Средняя_скорость").GetValue(x)).First();

                    MessageBox.Show($"Анализ по дням недели\n\n" +
                                   $"Самый быстрый день: {bestDay.GetType().GetProperty("День_недели").GetValue(bestDay)} " +
                                   $"({bestDay.GetType().GetProperty("Средняя_скорость").GetValue(bestDay)} км/ч)\n\n" +
                                   $"Самый медленный день: {worstDay.GetType().GetProperty("День_недели").GetValue(worstDay)} " +
                                   $"({worstDay.GetType().GetProperty("Средняя_скорость").GetValue(worstDay)} км/ч)\n\n",
                                   "Результат анализа", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при анализе: {ex.Message}", "Ошибка",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowWorstTrafficDays()
        {
            try
            {
                var historyData = db.HistorySpeeds.ToList();

                if (!historyData.Any())
                {
                    MessageBox.Show("Нет данных в таблице исторической скорости",
                                  "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var worstDays = historyData
                    .OrderBy(h => h.SrScorost)
                    .Take(10)
                    .Select(h => new
                    {
                        Дата = h.DataVremya.ToString("dd.MM.yyyy"),
                        День_недели = h.DataVremya.DayOfWeek.ToString(),
                        Средняя_скорость = Math.Round((double)h.SrScorost, 1),
                        Скорость_медленного = Math.Round((double)h.ScorostMedlPotoka, 1),
                        Скорость_быстрого = Math.Round((double)h.ScorostBystrPotoka, 1),
                        Оценка_пробок = GetTrafficRating((double)h.SrScorost)
                    })
                    .ToList();

                Type.ItemsSource = worstDays;

                double avgSpeedAll = (double)historyData.Average(h => h.SrScorost);
                var worstDay = worstDays.First();

                MessageBox.Show($"Топ самых пробочных дней\n\n" +
                               $"Всего дней: {historyData.Count}\n" +
                               $"Средняя скорость: {Math.Round(avgSpeedAll, 1)} км/ч\n\n" +
                               $"Самый пробочный день:\n" +
                               $" - Дата: {worstDay.GetType().GetProperty("Дата").GetValue(worstDay)}\n" +
                               $" - День: {worstDay.GetType().GetProperty("День_недели").GetValue(worstDay)}\n" +
                               $" - Скорость: {worstDay.GetType().GetProperty("Средняя_скорость").GetValue(worstDay)} км/ч\n" +
                               $" - Оценка: {worstDay.GetType().GetProperty("Оценка_пробок").GetValue(worstDay)}\n\n",
                               "Анализ пробок", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при анализе: {ex.Message}", "Ошибка",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetTrafficRating(double speed)
        {
            if (speed < 20) return "Критическая пробка";
            if (speed < 40) return "Сильная пробка";
            if (speed < 60) return "Средняя пробка";
            return "Свободная дорога";
        }

        private void BtnByDay_Click(object sender, RoutedEventArgs e)
        {
            ShowAverageSpeedByDay();
        }

        private void BtnWorst_Click(object sender, RoutedEventArgs e)
        {
            ShowWorstTrafficDays();
        }

        private void BtnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = db.HistorySpeeds.ToList();

                if (!result.Any())
                {
                    MessageBox.Show("Нет данных", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var displayResult = result.Select(h => new
                {
                    ID = h.ID,
                    Дата = h.DataVremya.ToString("dd.MM.yyyy HH:mm"),
                    Средняя_скорость = h.SrScorost,
                    ID_прогноза = h.IDPrognoza
                }).ToList();

                Type.ItemsSource = displayResult;

                MessageBox.Show($"SELECT * FROM HistorySpeeds\n\nНайдено записей: {result.Count}",
                               "LINQ запрос", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnWhereSpeed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = db.HistorySpeeds.Where(h => h.SrScorost > 50).ToList();

                var displayResult = result.Select(h => new
                {
                    ID = h.ID,
                    Дата = h.DataVremya.ToString("dd.MM.yyyy HH:mm"),
                    Средняя_скорость = h.SrScorost
                }).ToList();

                Type.ItemsSource = displayResult;

                MessageBox.Show($"WHERE SrScorost > 50\n\nНайдено записей: {result.Count}",
                               "LINQ запрос", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnOrderBySpeed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = db.HistorySpeeds.OrderBy(h => h.SrScorost).ToList();

                var displayResult = result.Select(h => new
                {
                    ID = h.ID,
                    Дата = h.DataVremya.ToString("dd.MM.yyyy HH:mm"),
                    Средняя_скорость = h.SrScorost
                }).ToList();

                Type.ItemsSource = displayResult;

                MessageBox.Show($"ORDER BY SrScorost ASC\n\nОтсортировано от медленных к быстрым",
                               "LINQ запрос", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnOrderBySpeedDesc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = db.HistorySpeeds.OrderByDescending(h => h.SrScorost).ToList();

                var displayResult = result.Select(h => new
                {
                    ID = h.ID,
                    Дата = h.DataVremya.ToString("dd.MM.yyyy HH:mm"),
                    Средняя_скорость = h.SrScorost
                }).ToList();

                Type.ItemsSource = displayResult;

                MessageBox.Show($"ORDER BY SrScorost DESC\n\nОтсортировано от быстрых к медленным",
                               "LINQ запрос", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

#endregion

