using FinanceManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.Data
{
    public interface IActivity
    {
        IEnumerable<Activity> Get();
        Activity Get(Guid id);
        void Post(Activity activity);
        void Put(Activity activity);
        void Delete(Guid id);
    }
}
