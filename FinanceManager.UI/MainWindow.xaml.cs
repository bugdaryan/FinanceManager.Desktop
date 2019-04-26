using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
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
            ToDatePicker.SelectedDate = DateTime.Now.AddMonths(1);
            FromDatePicker.SelectedDate = DateTime.Now;

            FromDatePicker.SelectedDateChanged += DatePicker_SelectedDateChanged;
            ToDatePicker.SelectedDateChanged += DatePicker_SelectedDateChanged;
        }

        private void DatePicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CalculateBtn.IsEnabled = true;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            CalculateBtn.IsEnabled = false;
            Helper.GetSummaries(FromDatePicker.SelectedDate.Value, ToDatePicker.SelectedDate.Value);
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
            (lineSeries.Values, Labels) = Helper.GetChartValues();
            SeriesCollection.Add(lineSeries);
            DataContext = this;
        }

        private void EditActivitiesBtn_Click(object sender, RoutedEventArgs e)
        {
            //EditActivitiesWindow editCategoriesWindow = new EditCategoriesWindow();
            //if(editCategoriesWindow.ShowDialog().Value)
            //{
            //    RefreshChart();
            //}
        }

        private void EditCategoriesBtn_Click(object sender, RoutedEventArgs e)
        {
            EditCategoriesWindow editCategoriesWindow = new EditCategoriesWindow();
            if(editCategoriesWindow.ShowDialog().Value)
            {
                CalculateBtn.IsEnabled = true;
            }
        }
    }
}
