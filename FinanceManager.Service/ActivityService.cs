using FinanceManager.Data;
using FinanceManager.Data.Enums;
using FinanceManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace FinanceManager.Service
{
    public class ActivityService : IActivity 
    {
        readonly SqlConnectionStringBuilder _builder;
        private readonly string _activitiesTableName;
        private readonly string _categoriesTableName;
        private readonly string _schemaName;
        private readonly string _databaseName;

        public ActivityService(string connection, string databaseName, string schemaName,string[] tableNames)
        {
            _builder = new SqlConnectionStringBuilder(connection);
            _activitiesTableName = tableNames[0];
            _categoriesTableName = tableNames[1];
            _schemaName = schemaName;
            _databaseName = databaseName;
        }

        public IEnumerable<Activity> GetActivities()
        {
            List<Activity> activities = new List<Activity>();
            string query = $"SELECT c.id as [CategoryId],c.ActivityType, c.Name as [CategoryName], a.Id as [ActivityId],a.Value,a.Description ,a.Date,a.Created FROM {_databaseName}.{_schemaName}.{_categoriesTableName} c INNER JOIN {_databaseName}.{_schemaName}.{_activitiesTableName} a ON a.CategoryId = c.Id";
            using (var connection = new SqlConnection(_builder.ConnectionString))
            {
                var command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var category = new Category
                        {
                            ActivityType = ((bool)reader["ActivityType"] ? ActivityType.Income : ActivityType.Outcome),
                            Id = (Guid)reader["CategoryId"],
                            Name = reader["CategoryName"].ToString()
                        };
                        var activity = new Activity
                        {
                            Category = category,
                            Created = (DateTime)reader["Created"],
                            Date = (DateTime)reader["Date"],
                            Description = reader["Description"].ToString(),
                            Id = (Guid)reader["ActivityId"],
                            Value = (decimal)reader["Value"]
                        };
                        activities.Add(activity);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                return activities;
            }
        }

        public void Add(Activity activity)
        {
            string query = $@"INSERT INTO {_databaseName}.{_schemaName}.{_activitiesTableName} (CategoryId, Date, Desctiption,  Value) VALUES 
                                ('{activity.Category.Id}','{activity.Date.ToString("yyyy-mm-dd")}', '{activity.Description}', {activity.Value});";
            ExecuteNonQuery(query);
        }
        
        public void Modify(Activity activity)
        {
            string query = $@"UPDATE {_databaseName}.{_schemaName}.{_activitiesTableName} SET Date = '{activity.Date}', Desctiption = '{activity.Description}', Value={activity.Value}";
            ExecuteNonQuery(query);
        }

        public void Remove(Guid id)
        {
            string query = $"DELETE FROM {_databaseName}.{_schemaName}.{_activitiesTableName} WHERE Id = '{id}'";
            ExecuteNonQuery(query);
        }

        private void ExecuteNonQuery(string query)
        {
            using (var connection = new SqlConnection(_builder.ConnectionString))
            {
                var command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }
    }
}
