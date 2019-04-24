using System;

namespace FinanceManager.Data.Models
{
    public class Activity
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public Guid WalletId { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public DateTime Created { get; set; }
        public DateTime Date { get; set; }
    }
}
