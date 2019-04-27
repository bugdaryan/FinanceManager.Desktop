using FinanceManager.Data;
using FinanceManager.Data.Models;
using System;
using System.Collections.Generic;

namespace FinanceManager.UI
{
    public class DataMapper
    {
        private readonly ISummary _summaryService;
        private readonly ICategory _categoryService;
        private readonly IActivity _activityService;

        public DataMapper(ISummary summaryService, ICategory categoryService, IActivity activityService)
        {
            _summaryService = summaryService;
            _categoryService = categoryService;
            _activityService = activityService;
        }

        public IEnumerable<Summary> GetSummaries(DateTime from, DateTime to)
        {
            return _summaryService.GetSummaries(from, to);
        }

        public IEnumerable<Category> GetCategories()
        {
            return _categoryService.GetCategories();
        }

        public void AddCategory(Category category)
        {
            _categoryService.Add(category);
        }

        public void ModifyCategory(Category category)
        {
            _categoryService.Modify(category);
        }

        public void RemoveCategory(Guid id)
        {
            _categoryService.Remove(id);
        }

        public IEnumerable<Activity> GetActivities()
        {
            return _activityService.GetActivities();
        }

        public void RemoveActivity(Guid id)
        {
            _activityService.Remove(id);
        }
    }
}
