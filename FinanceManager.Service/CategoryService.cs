﻿using FinanceManager.Data;
using FinanceManager.Data.Enums;
using FinanceManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace FinanceManager.Service
{
    public class CategoryService : ICategory
    {
        readonly SqlConnectionStringBuilder _builder;
        private readonly string _categoriesTableName;
        private readonly string _schemaName;
        private readonly string _databaseName;

        public CategoryService(string connection, string databaseName, string schemaName, string categoriesTableName)
        {
            _builder = new SqlConnectionStringBuilder(connection);
            _categoriesTableName = categoriesTableName;
            _schemaName = schemaName;
            _databaseName = databaseName;
        }

        public IEnumerable<Category> GetCategories()
        {
            var categories = new List<Category>();
            string query = $"SELECT * FROM {_databaseName}.{_schemaName}.{_categoriesTableName} ORDER BY Name";

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
                            ActivityType = ((bool)reader["ActivityType"]?ActivityType.Income:ActivityType.Outcome),
                            Id = Guid.Parse(reader["Id"].ToString()),
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
    }
}