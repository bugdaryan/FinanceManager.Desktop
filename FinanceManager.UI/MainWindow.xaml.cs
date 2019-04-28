using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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
            ToDatePicker.SelectedDate = DateTime.Now;
            FromDatePicker.SelectedDate = DateTime.Now.AddMonths(-1);

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
            EditActivitiesWindow editCategoriesWindow = new EditActivitiesWindow();
            if (editCategoriesWindow.ShowDialog().Value)
            {
                CalculateBtn.IsEnabled = true;
            }
        }

        private void EditCategoriesBtn_Click(object sender, RoutedEventArgs e)
        {
            EditCategoriesWindow editCategoriesWindow = new EditCategoriesWindow();
            if (editCategoriesWindow.ShowDialog().Value)
            {
                CalculateBtn.IsEnabled = true;
            }
        }

        private void VisualiseType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void FromDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SetDatePickerAndButton(FromDatePicker, ToDatePicker.SelectedDate, null, CalculateBtn);
        }

        private void ToDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SetDatePickerAndButton(ToDatePicker, DateTime.Now, FromDatePicker.SelectedDate, CalculateBtn);
        }

        private void SetDatePickerAndButton(DatePicker datePicker, DateTime? maxDate, DateTime? minDate, Button button)
        {
            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }
            if (!minDate.HasValue)
            {
                minDate = FromDatePicker.SelectedDate ?? DateTime.MinValue;
            }
            if (datePicker.SelectedDate.HasValue)
            {
                if (datePicker.SelectedDate.Value > maxDate)
                {
                    datePicker.SelectedDate = maxDate;
                }
                else if (datePicker.SelectedDate.Value < minDate)
                {
                    datePicker.SelectedDate = minDate;
                }
            }

            button.IsEnabled = true;
        }

    }
}
