using FinanceManager.Data.Models;
using FinanceManager.Service;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FinanceManager.UI
{
    public static class Helper
    {
        public static IEnumerable<Summary> Summaries { get; set; }

        public static IEnumerable<Category> Categories { get; set; }
        public static IEnumerable<Category> SearchedCategories { get; set; }

        public static IEnumerable<Activity> Activities { get; set; }
        public static IEnumerable<Activity> SearchedActivities { get; set; }

        private static readonly DataMapper _service;

        private static Dictionary<Border, Category> _categoryBorderToCategory;
        private static Dictionary<Border, Activity> _activityBorderToActivity;

        static Helper()
        {
            string databaseName = "FinanceManager";
            string schemaName = "dbo";
            string[] tableNames = { "Activities", "Categories" };
            SummaryService summaryService = new SummaryService(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, databaseName, schemaName, tableNames);
            CategoryService categoryService = new CategoryService(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, databaseName, schemaName, tableNames[1]);

            _service = new DataMapper(summaryService, categoryService);
        }


        public static void GetSummaries(DateTime from, DateTime to)
        {
            Summaries = _service.GetSummaries(from, to);
        }

        public static (ChartValues<decimal>, List<string>) GetChartValues()
        {
            var values = new ChartValues<decimal>();
            var labels = new List<string>();
            foreach (var item in Summaries)
            {
                values.Add(item.Total);
                labels.Add(item.Date.ToShortDateString());
            }

            return (values, labels);
        }


        public static void GetSearchedCategories(string searchQuery)
        {
            var queries = PrepareSearchQuery(searchQuery);
            SearchedCategories = Categories.Where(category => queries.Any(query => category.Name.ToLower().Contains(query)));
        }

        public static void GetSearchedActivities(string searchQuery)
        {
            var queries = PrepareSearchQuery(searchQuery);
            SearchedActivities = Activities.Where(activity => queries.Any(query => !string.IsNullOrEmpty(activity.Description) && activity.Description.ToLower().Contains(query)));
        }

        public static void GetCategoriesList()
        {
            Categories = _service.GetCategories();
        }

        public static void GetActivitiesList()
        {
            Activities = _service.GetActivities();
        }

        public static void AddCategory(Category category)
        {
            _service.AddCategory(category);
        }

        public static void ModifyCategory(Category category)
        {
            _service.ModifyCategory(category);
        }

        public static Border GetCategoryBorder(Category category, double width)
        {
            if (_categoryBorderToCategory == null)
            {
                _categoryBorderToCategory = new Dictionary<Border, Category>();
            }

            Border border = new Border
            {
                BorderThickness = new Thickness(4),
                BorderBrush = Brushes.Black
            };
            var stackPanel = new StackPanel
            {
                Width = width * .80,
                Height = 60,
                Background = Brushes.ForestGreen
            };
            Label labelName = new Label
            {
                Content = category.Name,
                FontSize = 20
            };

            Label labelType = new Label
            {
                Content = category.ActivityType.ToString(),
                FontSize = 10
            };

            border.Child = stackPanel;

            stackPanel.Children.Add(labelName);
            stackPanel.Children.Add(labelType);

            _categoryBorderToCategory.Add(border, category);

            return border;
        }
        
        public static object GetActivityBorder(Activity activity, double width)
        {
            if (_activityBorderToActivity == null)
            {
                _activityBorderToActivity = new Dictionary<Border, Activity>();
            }

            Border border = new Border
            {
                BorderThickness = new Thickness(4),
                BorderBrush = Brushes.Black
            };
            var stackPanel = new StackPanel
            {
                Width = width * .80,
                Height = 60,
                Background = Brushes.ForestGreen
            };
            Label labelValue = new Label
            {
                Content = activity.Value,
                FontSize = 20
            };

            Label labelDescription = new Label
            {
                Content = activity.Description,
                FontSize = 10
            };

            Label labelCategory = new Label
            {
                Content = activity.Category.ActivityType.ToString(),
                FontSize = 10
            };

            var grid = new Grid();
            grid.Children.Add(labelDescription);
            grid.Children.Add(labelCategory);


            border.Child = stackPanel;

            stackPanel.Children.Add(labelValue);
            stackPanel.Children.Add(grid);

            _activityBorderToActivity.Add(border, activity);

            return border;
        }

        public static void RemoveCategory(Border border)
        {
            Guid id = _categoryBorderToCategory[border].Id;
            _service.RemoveCategory(id);
        }

        public static void RemoveActivity(Border border)
        {
            Guid id = _activityBorderToActivity[border].Id;
            _service.RemoveActivity(id);
        }

        public static Category GetCategoryByBorder(Border border)
        {
            if (Categories == null)
            {
                GetCategoriesList();
            }

            return _categoryBorderToCategory[border];
        }

        public static Activity GetActivityByBorder(Border border)
        {
            if (Activities == null)
            {
                GetCategoriesList();
            }

            return _activityBorderToActivity[border];
        }
        private static string[] PrepareSearchQuery(string searchQuery)
        {
            searchQuery = Regex.Replace(searchQuery, @"\s+", " ").Trim().ToLower();
           return searchQuery.Split(' ');
        }
    }
}
