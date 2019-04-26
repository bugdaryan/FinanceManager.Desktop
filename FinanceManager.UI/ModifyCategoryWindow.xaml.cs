using FinanceManager.Data.Enums;
using FinanceManager.Data.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace FinanceManager.UI
{
    /// <summary>
    /// Interaction logic for ModifyCategoryWindow.xaml
    /// </summary>
    public partial class ModifyCategoryWindow : Window
    {
        Guid categoryId;
        public ModifyCategoryWindow(Category category)
        {
            InitializeComponent();
            var activityTypes = typeof(ActivityType).GetEnumValues();
            int index = 0;

            categoryId = category.Id;

            foreach (var activityType in activityTypes)
            {
                if ((int)activityType == (int)category.ActivityType)
                {
                    break;
                }
                index++;
            }

            foreach (var activityType in activityTypes)
            {
                ActivityTypeComboBox.Items.Add(new ComboBoxItem
                {
                    Content = activityType.ToString()
                });
            }

            ActivityTypeComboBox.SelectedIndex = index;

            NameTextBox.Text = category.Name;
        }

        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            InvalidNameLabel.Visibility = Visibility.Collapsed;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CheckForValidName())
            {
                var category = new Category
                {
                    Id = categoryId,
                    Name = NameTextBox.Text,
                    ActivityType = (ActivityTypeComboBox.SelectedValue.ToString() == "Income" ? Data.Enums.ActivityType.Income : Data.Enums.ActivityType.Outcome)
                };

                Helper.ModifyCategory(category);
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
            Close();
        }
    }
}
