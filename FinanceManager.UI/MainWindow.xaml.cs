using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace FinanceManager.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //InitChart();
        }

        private void InitChart()
        {
            if (SeriesCollection != null)
            {
                SeriesCollection.Clear();
            }

            var values = new ChartValues<decimal>();
            Labels = new List<string>();

            foreach (var item in Helper.Activities)
            {
                values.Add(item.Total);
                Labels.Add(item.Date.ToString("yyyy-mm-dd"));
            }
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = values
                }
            };



            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CalculateBtn.IsEnabled = false;
            RefreshChart();
        }
        private void RefreshChart()
        {
            if (SeriesCollection == null)
            {
                SeriesCollection = new SeriesCollection();
            }

            if (Labels == null)
            {
                Labels = new List<string>();
            }

            SeriesCollection.Clear();
            Labels.Clear();
            var lineSeries = new LineSeries();
            lineSeries.Values = new ChartValues<decimal>();
            foreach (var item in Helper.Activities)
            {
                lineSeries.Values.Add(item.Total);
                Labels.Add(item.Date.ToString("yyyy-mm-dd"));
            }
            SeriesCollection.Add(lineSeries);

            Labels = Labels.Distinct().ToList();
            DataContext = this;
        }
    }
}
