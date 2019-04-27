using System.Windows;
using System.Windows.Controls;

namespace FinanceManager.UI
{
    /// <summary>
    /// Interaction logic for EditActivitiesWindow.xaml
    /// </summary>
    public partial class EditActivitiesWindow : Window
    {
        bool isSearch = false;
        bool result = false;
        public EditActivitiesWindow()
        {
            InitializeComponent();
            Helper.GetActivitiesList();
            RefreshList();

            Closing += (sender, e) =>
            {
                DialogResult = result;
            };
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isSearch = !(string.IsNullOrEmpty(SearchBox.Text) || string.IsNullOrWhiteSpace(SearchBox.Text));
            if (isSearch)
            {
                Helper.GetSearchedActivities(SearchBox.Text);
            }
            RefreshList();
        }

        private void RefreshList()
        {
            var list = isSearch ? Helper.SearchedActivities : Helper.Activities;
            if (list != null)
            {
                ActivitiesListBox.Items.Clear();
                foreach (var item in list)
                {
                    ActivitiesListBox.Items.Add(Helper.GetActivityBorder(item, ActivitiesListBox.Width));
                }
            }
        }

        private void NewActivityBtn_Click(object sender, RoutedEventArgs e)
        {
            var newActivityWindow = new CategoryWindow();
            if (newActivityWindow.ShowDialog().Value)
            {
                Helper.GetActivitiesList();
                RefreshList();
                result = true;
            }
        }

        private void ModifyActivityBtn_Click(object sender, RoutedEventArgs e)
        {
            var border = (Border)ActivitiesListBox.SelectedItem;
            if (border != null)
            {
                var activity = Helper.GetActivityByBorder(border);
                var modifyActivityWindow = new ActivityWindow(activity);
                if (modifyActivityWindow.ShowDialog().Value)
                {
                    Helper.GetActivitiesList();
                    RefreshList();
                    result = true;
                }

            }
            else
            {
                MessageBox.Show("Please select category do modify");
            }
        }

        private void RemoveActivityBtn_Click(object sender, RoutedEventArgs e)
        {
            var border = (Border)ActivitiesListBox.SelectedItem;
            if (border != null)
            {
                var categoryName = Helper.GetActivityByBorder(border).Name;
                var dlgRes = MessageBox.Show($"Are you sure you want to remove {categoryName} category permanently?", "Be careful", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (dlgRes == MessageBoxResult.Yes)
                {

                    Helper.RemoveActivity(border);
                    Helper.GetActivitiesList();
                    RefreshList();
                    result = true;
                }
            }
            else
            {
                MessageBox.Show("Please select category do remove");
            }
        }

        private void ActivitiesListBox_GotFocus(object sender, RoutedEventArgs e)
        {
            RemoveActivityBtn.IsEnabled = true;
            ModifyActivityBtn.IsEnabled = true;
        }

        private void ActivitiesListBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!ModifyActivityBtn.IsMouseOver && !RemoveActivityBtn.IsMouseOver)
            {
                ModifyActivityBtn.IsEnabled = false;
                RemoveActivityBtn.IsEnabled = false;
            }
        }
    }
}
