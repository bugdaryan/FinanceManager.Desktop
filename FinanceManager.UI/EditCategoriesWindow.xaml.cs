using System.Windows;
using System.Windows.Controls;

namespace FinanceManager.UI
{
    /// <summary>
    /// Interaction logic for EditCategoriesWindow.xaml
    /// </summary>
    public partial class EditCategoriesWindow : Window
    {
        bool isSearch = false;
        bool result = false;
        public EditCategoriesWindow()
        {
            InitializeComponent();
            Helper.GetCategoriesList();
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
                Helper.GetSearchedCategories(SearchBox.Text);
            }
            RefreshList();
        }

        private void RefreshList()
        {
            var list = isSearch ? Helper.SearchedCategories : Helper.Categories;
            if (list != null)
            {
                CategoriesListBox.Items.Clear();
                foreach (var item in list)
                {
                    CategoriesListBox.Items.Add(Helper.GetCategoryBorder(item, CategoriesListBox.Width));
                }
            }
        }

        private void NewCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            var newCategoryWindow = new CategoryWindow();
            if (newCategoryWindow.ShowDialog().Value)
            {
                Helper.GetCategoriesList();
                RefreshList();
                result = true;
            }
        }

        private void ModifyCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            var border = (Border)CategoriesListBox.SelectedItem;
            if (border != null)
            {
                var category = Helper.GetCategoryByBorder(border);
                var modifyCategoryWindow = new CategoryWindow(category);
                if (modifyCategoryWindow.ShowDialog().Value)
                {
                    Helper.GetCategoriesList();
                    RefreshList();
                    result = true;
                }

            }
            else
            {
                MessageBox.Show("Please select category do modify");
            }
        }

        private void RemoveCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            var border = (Border)CategoriesListBox.SelectedItem;
            if (border != null)
            {
                var categoryName = Helper.GetCategoryByBorder(border).Name;
                var dlgRes = MessageBox.Show($"Are you sure you want to remove {categoryName} category permanently?", "Be careful", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (dlgRes == MessageBoxResult.Yes)
                {

                    Helper.RemoveCategory(border);
                    Helper.GetCategoriesList();
                    RefreshList();
                    result = true;
                }
            }
            else
            {
                MessageBox.Show("Please select category do remove");
            }
        }

        private void CategoriesListBox_GotFocus(object sender, RoutedEventArgs e)
        {
            RemoveCategoryBtn.IsEnabled = true;
            ModifyCategoryBtn.IsEnabled = true;
        }

        private void CategoriesListBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!ModifyCategoryBtn.IsMouseOver && !RemoveCategoryBtn.IsMouseOver)
            {
                ModifyCategoryBtn.IsEnabled = false;
                RemoveCategoryBtn.IsEnabled = false;
            }
        }
    }
}
