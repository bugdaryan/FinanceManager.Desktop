using FinanceManager.Data.Models;
using System;
using System.Collections.Generic;

namespace FinanceManager.UI
{
    public static class Helper
    {
        public static IEnumerable<Summary> Activities;
        private static readonly DataMapper _service;

        static Helper()
        {
            _service = new DataMapper();
            Activities = new List<Summary>
            {
               new Summary { Date = DateTime.ParseExact("2019-12-27","yyyy-mm-dd",null), Total = -1930},
               new Summary { Date = DateTime.ParseExact("2019-12-26","yyyy-mm-dd",null), Total = 417},
               new Summary { Date = DateTime.ParseExact("2019-12-25","yyyy-mm-dd",null), Total = -548},
               new Summary { Date = DateTime.ParseExact("2019-12-24","yyyy-mm-dd",null), Total = -1104},
               new Summary { Date = DateTime.ParseExact("2019-12-23","yyyy-mm-dd",null), Total = -1810},
               new Summary { Date = DateTime.ParseExact("2019-12-22","yyyy-mm-dd",null), Total = -457},
               new Summary { Date = DateTime.ParseExact("2019-12-21","yyyy-mm-dd",null), Total = -1498},
               new Summary { Date = DateTime.ParseExact("2019-12-20","yyyy-mm-dd",null), Total = -641},
               new Summary { Date = DateTime.ParseExact("2019-12-19","yyyy-mm-dd",null), Total = -56},
               new Summary { Date = DateTime.ParseExact("2019-12-18","yyyy-mm-dd",null), Total = 283},
               new Summary { Date = DateTime.ParseExact("2019-12-17","yyyy-mm-dd",null), Total = -217},
               new Summary { Date = DateTime.ParseExact("2019-12-16","yyyy-mm-dd",null), Total = 31},
               new Summary { Date = DateTime.ParseExact("2019-12-15","yyyy-mm-dd",null), Total = 185},
               new Summary { Date = DateTime.ParseExact("2019-12-14","yyyy-mm-dd",null), Total = -153},
               new Summary { Date = DateTime.ParseExact("2019-12-13","yyyy-mm-dd",null), Total = 119}
            };

        }

        public static void GetActivities(DateTime from, DateTime to)
        {
            //Activities = _service.GetActivities(from, to);
        }
    }
}
