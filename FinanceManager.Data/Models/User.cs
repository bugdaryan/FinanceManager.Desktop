using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.Data.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
    }
}
