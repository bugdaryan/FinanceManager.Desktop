using FinanceManager.Data.Models;
using FinanceManager.Service;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinanceManager.UI
{
    public static class Helper
    {
        public static IEnumerable<Summary> Summaries;
        private static readonly DataMapper _service;

        static Helper()
        {
            string connectionString = @"Server=(localdb)\mssqllocaldb;Trusted_Connection=True;MultipleActiveResultSets=true;";
            string databaseName = "FinanceManager";
            string schemaName = "dbo";

            SummaryService summaryService = new SummaryService(connectionString, "Activities", "Categories", schemaName, databaseName);
            _service = new DataMapper(summaryService);
        }

        public static void GetSummaries(DateTime from, DateTime to)
        {
            Summaries = _service.GetSummaries(from, to);
        }

        public static (ChartValues<decimal>, List<string>) GetChartValues()
        {
            var values = new ChartValues<decimal>();
            var labels = new List<string>();
            foreach (var item in Summaries)
            {
                values.Add(item.Total);
                labels.Add(item.Date.ToShortDateString());
            }

            return (values, labels);
        }
    }
}
