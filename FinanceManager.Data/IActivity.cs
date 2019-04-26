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
        IEnumerable<Activity> GetActivities();

        void Add(Activity activity);

        void Modify(Activity activity);

        void Remove(Guid id);
    }
}
