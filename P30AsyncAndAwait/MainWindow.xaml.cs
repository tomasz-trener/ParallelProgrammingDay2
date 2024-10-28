using System.Net;
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

namespace P30AsyncAndAwait
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //public int CalculateValue()
        //{
        //    Thread.Sleep(5000);
        //    return 100;
        //}

        public Task<int> CalculateValueAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return 100;
            });
        }

        public async Task<int> CalculateValueAsync2()
        {
            await Task.Delay(5000);
            return 100;
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            var n = CalculateValueAsync();

            n.ContinueWith(task =>
            {
                lblResult.Content = task.Result.ToString();
            },TaskScheduler.FromCurrentSynchronizationContext());


            lblResult.Content = "Loading...";
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            int value = await CalculateValueAsync2();
            lblResult.Content = value.ToString();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using(var wc = new WebClient())
            {
                var result = await wc.DownloadStringTaskAsync("https://www.google.com/robots.txt");
                lblResult.Content = result.Split("\n")[0];
            }
        }

        private void btnGetTemperature1_Click(object sender, RoutedEventArgs e)
        {
            WeatherForecastService service = new WeatherForecastService();
            string[] cities = txtCity.Text.Split(Environment.NewLine);

            foreach (var city in cities)
            {
                int temp = service.GetTemperature(city);
                tbTemperature.Text += $"Temperature in {city} is currently {temp} C" + Environment.NewLine;
            }
        }

        // secnariusz 1: jedno miasto po drugim 
        private void btnGetTemperature2_Click(object sender, RoutedEventArgs e)
        {
            tbTemperature.Text = string.Empty;
            lvLogger.Items.Clear();
            WeatherForecastService service = new WeatherForecastService();
            string[] cities = txtCity.Text.Split(Environment.NewLine);
            foreach (var city in cities)
            {
                lvLogger.Items.Add($"Currently Processing {city}");
                
                var t = new Task<int>(() =>
                {
                    int temp = service.GetTemperature(city);
                    return temp;
                });
                t.Start();


               
                tbTemperature.Text += $"Temperature in {city} is currently {t.Result} C" + Environment.NewLine;
            }

        }

        // secnariusz 1: jedno miasto po drugim z uzyciem await 
        private async void btnGetTemperature3_Click(object sender, RoutedEventArgs e)
        {
            tbTemperature.Text = string.Empty;
            lvLogger.Items.Clear();
            WeatherForecastService service = new WeatherForecastService();
            string[] cities = txtCity.Text.Split(Environment.NewLine);
            foreach (var city in cities)
            {
                lvLogger.Items.Add($"Currently Processing {city}");

                var t = await Task.Run<int>(() => 
                {
                    int temp = service.GetTemperature(city);
                    return temp;
                });

                tbTemperature.Text += $"Temperature in {city} is currently {t} C" + Environment.NewLine;
            }
        }

        // secnariusz 1: czekamy na wszystkie zadania 
        private async void btnGetTemperature4_Click(object sender, RoutedEventArgs e)
        {
            tbTemperature.Text = string.Empty;
            lvLogger.Items.Clear();
            WeatherForecastService service = new WeatherForecastService();
            string[] cities = txtCity.Text.Split(Environment.NewLine);
            List<Task<int>> tasks = new List<Task<int>>();
            foreach (var city in cities)
            {
                var t = Task.Run<int>(() =>
                {
                    int temp = service.GetTemperature(city);
                    return temp;
                });
                tasks.Add(t);
            }

            lvLogger.Items.Add($"Stared processing all cities");
            await Task.WhenAll(tasks);
            lvLogger.Items.Add($"Finished processing all cities");

            foreach (var item in tasks)
            {
                var t = item.Result;
                tbTemperature.Text += $"Temperature in ... is currently {t} C" + Environment.NewLine;
            }
          
          
        }

        private async void btnGetTemperature5_Click(object sender, RoutedEventArgs e)
        {
            tbTemperature.Text = string.Empty;
            lvLogger.Items.Clear();
            WeatherForecastService service = new WeatherForecastService();
            string[] cities = txtCity.Text.Split(Environment.NewLine);
            List<Task> tasks = new List<Task>();
            foreach (var city in cities)
            {
                var t = Task.Run(() =>
                {
                    int temp = service.GetTemperature(city);
                    return (temp, city);
                });
                tasks.Add(t);
            }

            lvLogger.Items.Add($"Stared processing all cities");
            await Task.WhenAll(tasks);
            lvLogger.Items.Add($"Finished processing all cities");

            foreach (Task<(int Temperture, string City)> item in tasks)
            {
                var t = item.Result.Temperture;
                string cityName = item.Result.City;
                tbTemperature.Text += $"Temperature in {cityName} is currently {t} C" + Environment.NewLine;
            }

        }

        // scenariusz : nie czekamy na wszystkie zadania ale wypisujemy wnik od razu 
        private void btnGetTemperature6_Click(object sender, RoutedEventArgs e)
        {
            tbTemperature.Text = string.Empty;
            lvLogger.Items.Clear();
            WeatherForecastService service = new WeatherForecastService();
            string[] cities = txtCity.Text.Split(Environment.NewLine);

            foreach (var city in cities)
            {
                var t = Task.Run(() =>
                {
                    int temp = service.GetTemperature(city);
                    return (temp, city);
                });

                lvLogger.Items.Add("Started processing " + city);

                t.GetAwaiter().OnCompleted(() =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        tbTemperature.Text += $"Temperature in {t.Result.city} is currently {t.Result.temp} C" + Environment.NewLine;
                    });
                });
            }
        }

        private async void btnGetTemperature7_Click(object sender, RoutedEventArgs e)
        {
            tbTemperature.Text = string.Empty;
            lvLogger.Items.Clear();
            WeatherForecastService service = new WeatherForecastService();
            string[] cities = txtCity.Text.Split(Environment.NewLine);
            pbProgress.Maximum = cities.Length;
            pbProgress.Value = 0;

            foreach (var city in cities)
            {
                lvLogger.Items.Add($"Currently Processing {city}");

                var t = await Task.Run<int>(() =>
                {
                    int temp = service.GetTemperature(city);
                    return temp;
                });

                tbTemperature.Text += $"Temperature in {city} is currently {t} C" + Environment.NewLine;
                pbProgress.Value++;
            }
        }

        private async void btnGetTemperature8_Click(object sender, RoutedEventArgs e)
        {
            tbTemperature.Text = string.Empty;
            lvLogger.Items.Clear();
            WeatherForecastService service = new WeatherForecastService();
            string[] cities = txtCity.Text.Split(Environment.NewLine);
            pbProgress.Maximum = cities.Length;
            pbProgress.Value = 0;

            foreach (var city in cities)
            {
                lvLogger.Items.Add($"Currently Processing {city}");

                int temp = await service.GetTemperatureAsync(city);
                tbTemperature.Text += $"Temperature in {city} is currently {temp} C" + Environment.NewLine;
                pbProgress.Value++;
            }
        }
    }
}