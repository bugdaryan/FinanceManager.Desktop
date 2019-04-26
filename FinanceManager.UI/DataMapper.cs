using FinanceManager.Data;
using FinanceManager.Data.Models;
using FinanceManager.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.UI
{
    public class DataMapper
    {
        private readonly ISummary _summaryService;
        private readonly ICategory _categoryService;

        public DataMapper(ISummary summaryService, ICategory categoryService)
        {
            _summaryService = summaryService;
            _categoryService = categoryService;
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
    }
}
