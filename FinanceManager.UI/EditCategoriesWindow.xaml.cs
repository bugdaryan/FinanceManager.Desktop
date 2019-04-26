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
            var newCategoryWindow = new NewCategoryWindow();
            if (newCategoryWindow.ShowDialog().Value)
            {
                Helper.GetCategoriesList();
                RefreshList();
                result = true;
            }
        }

        private void ModifyCategoryBtn_Click(object sender, RoutedEventArgs e)
        {

            result = true;
        }

        private void RemoveCategoryBtn_Click(object sender, RoutedEventArgs e)
        {

            result = true;
        }
    }
}
