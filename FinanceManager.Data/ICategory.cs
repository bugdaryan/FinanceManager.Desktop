using FinanceManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.Data
{
    public interface ICategory
    {
        IEnumerable<Category> GetAllCategories();
        Category GetCategory(Guid id);
        void PostCategory(Category category);
        void PutCategory(Category category);
        void DeleteCategory(Guid id);
    }
}
