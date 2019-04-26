using FinanceManager.Data;
using FinanceManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace FinanceManager.Service
{
    public class SummaryService : ISummary
    {
        readonly SqlConnectionStringBuilder _builder;
        private readonly string _activitiesTableName;
        private readonly string _categoriesTableName;
        private readonly string _schemaName;
        private readonly string _databaseName;

        public SummaryService(string connection, string databaseName, string schemaName,string[] tableNames)
        {
            _builder = new SqlConnectionStringBuilder(connection);
            _activitiesTableName = tableNames[0];
            _categoriesTableName = tableNames[1];
            _schemaName = schemaName;
            _databaseName = databaseName;
        }


        public IEnumerable<Summary> GetSummaries(DateTime from, DateTime to)
        {
            string query = $@"SELECT  a.Date, SUM(CASE WHEN c.ActivityType = 1 THEN a.[Value] ELSE -a.[Value] END) AS [Total]
                            FROM {_databaseName}.{_schemaName}.{_activitiesTableName} a
                            INNER JOIN {_databaseName}.{_schemaName}.{_categoriesTableName} c ON a.CategoryId = c.Id
                            WHERE a.Date BETWEEN '{from}' AND '{to}'
                            GROUP BY a.Date
                            ORDER BY a.Date";
            using (var connection = new SqlConnection(_builder.ConnectionString))
            {
                var command = new SqlCommand(query, connection);
                List<Summary> summaries = new List<Summary>();
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var summary = new Summary
                        {
                            Date = (DateTime)reader["Date"],
                            Total = (decimal)reader["Total"]
                        };
                        summaries.Add(summary);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                return summaries;
            }

        }
    }
}
