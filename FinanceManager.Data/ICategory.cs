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
        IEnumerable<Category> Get();
        ICategory Get(Guid id);
        void Post(Category category);
        void Put(Category category);
        void Delete(Guid id);
    }
}
