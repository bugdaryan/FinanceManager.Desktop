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
        IEnumerable<Activity> GetAllActivities();
        IEnumerable<Activity> GetActivitiesInRange(DateTime from, DateTime to);
        Activity GetActivity(Guid id);
        void PostActivity(Activity activity);
        void PutActivity(Activity activity);
        void DeleteActivity(Guid id);
    }
}
