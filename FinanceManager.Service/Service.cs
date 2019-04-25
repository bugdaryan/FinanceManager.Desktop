using FinanceManager.Data;
using FinanceManager.Data.Enums;
using FinanceManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace FinanceManager.Service
{
    public class Service : IActivity, ICategory
    {
        readonly SqlConnectionStringBuilder _builder;
        private readonly string _activitiesTableName;
        private readonly string _categoriesTableName;
        private readonly string _schemaName;
        private readonly string _databaseName;

        public Service(string connection, string activitiesTableName, string categoriesTableName, string schemaName, string databaseName)
        {
            _builder = new SqlConnectionStringBuilder(connection);
            _activitiesTableName = activitiesTableName;
            _categoriesTableName = categoriesTableName;
            _schemaName = schemaName;
            _databaseName = databaseName;
        }

        public IEnumerable<Activity> GetActivitiesInRange(DateTime from, DateTime to)
        {
            if (from >= to)
            {
                throw new ArgumentOutOfRangeException();
            }

            return GetAllActivities().Where(activity => activity.Date > from && activity.Date < to);
        }

        public Activity GetActivity(Guid id)
        {
            Activity activity = null;
            string query = $"SELECT c.id as [CategoryId],c.ActivityType, c.Name as [CategoryName], a.Id as [ActivityId],a.Value,a.Description ,a.Date,a.Created FROM {_databaseName}.{_schemaName}.{_categoriesTableName} c INNER JOIN {_databaseName}.{_schemaName}.{_activitiesTableName} a ON a.CategoryId = c.Id WHERE a.Id = '{id}'";
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
                            ActivityType = (ActivityType)reader["ActivityType"],
                            Id = (Guid)reader["CategoryId"],
                            Name = reader["CategoryName"].ToString()
                        };
                        activity = new Activity
                        {
                            Category = category,
                            Created = (DateTime)reader["Created"],
                            Date = (DateTime)reader["Date"],
                            Description = reader["Description"].ToString(),
                            Id = (Guid)reader["ActivityId"],
                            Value = (decimal)reader["Value"]
                        };
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                return activity;
            }
        }

        public IEnumerable<Activity> GetAllActivities()
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
                            ActivityType = (ActivityType)reader["ActivityType"],
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

        public IEnumerable<Category> GetAllCategories()
        {
            List<Category> categories = new List<Category>();
            string query = $"SELECT * FROM {_databaseName}.{_schemaName}.{_categoriesTableName}";
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
                            ActivityType = (ActivityType)reader["ActivityType"],
                            Id = (Guid)reader["Id"],
                            Name = reader["Name"].ToString()
                        };
                        categories.Add(category);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                return categories;
            }
        }

        public Category GetCategory(Guid id)
        {
            Category category = null;
            string query = $"SELECT * FROM {_databaseName}.{_schemaName}.{_categoriesTableName} WHERE Id = '{id}'";
            using (var connection = new SqlConnection(_builder.ConnectionString))
            {
                var command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        category = new Category
                        {
                            ActivityType = (ActivityType)reader["ActivityType"],
                            Id = (Guid)reader["Id"],
                            Name = reader["Name"].ToString()
                        };
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                return category;
            }
        }

        public void PostActivity(Activity activity)
        {
            string query = $@"INSERT INTO {_databaseName}.{_schemaName}.{_activitiesTableName} (CategoryId, Date, Desctiption,  Value) VALUES 
                                ('{activity.Category.Id}','{activity.Date.ToString("yyyy-mm-dd")}', '{activity.Description}', {activity.Value});";
            ExecuteNonQuery(query);
        }

        public void PostCategory(Category category)
        {
            string query = $@"INSERT INTO {_databaseName}.{_schemaName}.{_categoriesTableName} (Name, ActivityType) VALUES 
                                ('{category.Name}', {(int)category.ActivityType});";
            ExecuteNonQuery(query);
        }

        public void PutActivity(Activity activity)
        {
            string query = $@"UPDATE {_databaseName}.{_schemaName}.{_activitiesTableName} SET Date = '{activity.Date}', Desctiption = '{activity.Description}', Value={activity.Value}";
            ExecuteNonQuery(query);
        }

        public void PutCategory(Category category)
        {
            string query = $@"UPDATE {_databaseName}.{_schemaName}.{_categoriesTableName} SET Name = '{category.Name}',ActivityType = '{(int)category.ActivityType}'";
            ExecuteNonQuery(query)
        }

        public void DeleteActivity(Guid id)
        {
            string query = $"DELETE FROM {_databaseName}.{_schemaName}.{_activitiesTableName} WHERE Id = '{id}'";
            ExecuteNonQuery(query);
        }

        public void DeleteCategory(Guid id)
        {
            string query = $"DELETE FROM {_databaseName}.{_schemaName}.{_categoriesTableName} WHERE Id = '{id}'";
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
