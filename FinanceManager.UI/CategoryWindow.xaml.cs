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
    public partial class CategoryWindow : Window
    {
        Guid categoryId;
        bool isModify = false;
        public CategoryWindow(Category category = null)
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

            HeaderLabel.Content = "New category";
            ActivityTypeComboBox.SelectedIndex = 0;

            if (category != null)
            {
                isModify = true;
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
                ActivityTypeComboBox.SelectedIndex = index;
                NameTextBox.Text = category.Name;
                HeaderLabel.Content = "Modify category";
            }


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
                    Id = isModify ? categoryId : Guid.Empty,
                    Name = NameTextBox.Text,
                    ActivityType = (((ComboBoxItem)ActivityTypeComboBox.SelectedItem).Content.ToString() == "Income" ? Data.Enums.ActivityType.Income : Data.Enums.ActivityType.Outcome)
                };
                if (isModify)
                {
                    Helper.ModifyCategory(category);
                }
                else
                {
                    Helper.AddCategory(category);
                }
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
