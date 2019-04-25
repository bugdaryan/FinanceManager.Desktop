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

        public DataMapper(ISummary summaryService)
        {
            _summaryService = summaryService;
        }

        public IEnumerable<Summary> GetSummaries(DateTime from, DateTime to)
        {
            return _summaryService.GetSummaries(from, to);
        }
    }
}
