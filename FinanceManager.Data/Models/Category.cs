using FinanceManager.Data.Enums;
using System;

namespace FinanceManager.Data.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ActivityType ActivityType { get; set; }
    }
}
