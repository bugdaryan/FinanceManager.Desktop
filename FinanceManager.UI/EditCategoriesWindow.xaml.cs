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
        public EditCategoriesWindow()
        {
            InitializeComponent();
            Helper.GetCategoriesList();
            RefreshList();
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
                    CategoriesListBox.Items.Add(Helper.GetNewToDoItemBorder(item, CategoriesListBox.Width));
                }
            }
        }

        private void NewCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            
            Helper.GetCategoriesList();
            RefreshList();
            DialogResult = true;
        }

        private void ModifyCategoryBtn_Click(object sender, RoutedEventArgs e)
        {

            DialogResult = true;
        }

        private void RemoveCategoryBtn_Click(object sender, RoutedEventArgs e)
        {

            DialogResult = true;
        }
    }
}
