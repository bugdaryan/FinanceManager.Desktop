
using FinanceManager.Data.Enums;
using FinanceManager.Data.Models;
using System.Windows;
using System.Windows.Controls;

namespace FinanceManager.UI
{
    /// <summary>
    /// Interaction logic for NewCategoryWindow.xaml
    /// </summary>
    public partial class NewCategoryWindow : Window
    {
        public NewCategoryWindow()
        {
            InitializeComponent();
            var activityTypes = typeof(ActivityType).GetEnumValues();
            foreach (var activityType in activityTypes)
            {
                ActivityTypeComboBox.Items.Add(new ComboBoxItem
                {
                    Content = activityType.ToString()
                });
            }

            ActivityTypeComboBox.SelectedIndex = 0;
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CheckForValidName())
            {
                var category = new Category
                {
                    Name = NameTextBox.Text,
                    ActivityType = (ActivityTypeComboBox.SelectedValue.ToString() == "Income" ? Data.Enums.ActivityType.Income : Data.Enums.ActivityType.Outcome)
                };
                Helper.AddCategory(category);
                DialogResult = true;
            }
            else
            {
                InvalidNameLabel.Visibility = Visibility.Visible;
            }
        }

        private bool CheckForValidName()
        {
            return !(string.IsNullOrEmpty(NameTextBox.Text) || string.IsNullOrWhiteSpace(NameTextBox.Text));
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            InvalidNameLabel.Visibility = Visibility.Collapsed;
        }
    }
}
