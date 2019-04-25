using FinanceManager.Data.Models;
using System;
using System.Data.SqlClient;
using System.Text;

namespace FinanceManager.Console
{
    class Program
    {
        static string connectionString = @"Server=(localdb)\mssqllocaldb;Trusted_Connection=True;MultipleActiveResultSets=true;";
        static string databaseName = "FinanceManager";
        static string schemaName = "dbo";
        static string[] tableNames = new string[] { "Categories", "Activities" };
        static void Main(string[] args)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder("Server = (localdb)\\mssqllocaldb; Trusted_Connection = True; MultipleActiveResultSets = true;");
            SeedData(builder);
            System.Console.WriteLine("Seeding finished");
        }

        private static void SeedData(SqlConnectionStringBuilder builder)
        {
            EnsureDatabaseCreated();
            builder.InitialCatalog = databaseName;

            Category[] categories = new Category[]
            {
                new Category { Id = Guid.NewGuid(), ActivityType = Data.Enums.ActivityType.Income, Name="'Salary'"},
                new Category { Id = Guid.NewGuid(), ActivityType = Data.Enums.ActivityType.Income, Name="'Capital'"},
                new Category { Id = Guid.NewGuid(), ActivityType = Data.Enums.ActivityType.Income, Name="'Trade'"},
                new Category { Id = Guid.NewGuid(), ActivityType = Data.Enums.ActivityType.Income, Name="'Labour'"},
                new Category { Id = Guid.NewGuid(), ActivityType = Data.Enums.ActivityType.Income, Name="'Bonus salary'"},
                new Category { Id = Guid.NewGuid(), ActivityType = Data.Enums.ActivityType.Outcome, Name="'Tax'"},
                new Category { Id = Guid.NewGuid(), ActivityType = Data.Enums.ActivityType.Outcome, Name="'Advertisment'"},
                new Category { Id = Guid.NewGuid(), ActivityType = Data.Enums.ActivityType.Outcome, Name="'Resources'"},
                new Category { Id = Guid.NewGuid(), ActivityType = Data.Enums.ActivityType.Outcome, Name="'Materials'"},
                new Category { Id = Guid.NewGuid(), ActivityType = Data.Enums.ActivityType.Outcome, Name="'Utility'"},
                new Category { Id = Guid.NewGuid(), ActivityType = Data.Enums.ActivityType.Outcome, Name="'Credit'"}
            };

            Random rnd = new Random();

            var query = GetSeedQuery(categories, rnd);
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    for (int i = 0; i < 10; i++)
                    {
                        command.CommandText = GetActivityQuery(categories, rnd);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        static string GetSeedQuery(Category[] categories,Random rnd)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append($"DELETE FROM {databaseName}.{schemaName}.Activities;DELETE FROM {databaseName}.{schemaName}.Categories;");

            queryBuilder.Append($"INSERT INTO {databaseName}.{schemaName}.Categories (Id, Name, ActivityType) VALUES ");
            for (int i = 0; i < categories.Length; i++)
            {
                queryBuilder.Append($" ('{categories[i].Id}',{categories[i].Name},{(int)categories[i].ActivityType})");
                queryBuilder.Append((i == categories.Length - 1 ? ";" : ","));
            }

            return queryBuilder.ToString();
        }

        private static string GetActivityQuery(Category[] categories, Random rnd)
        {
            StringBuilder queryBuilder = new StringBuilder();
            int activityCount = 1000;
            queryBuilder.Append($"INSERT INTO {databaseName}.{schemaName}.Activities (CategoryId,  Description, Value, Date) VALUES ");
            for (int i = 0; i < activityCount; i++)
            {
                int categoryIndex = rnd.Next(0, categories.Length);
                Guid categoryGuid = categories[categoryIndex].Id;
                queryBuilder.Append($" ('{categoryGuid}',{categories[categoryIndex].Name},{rnd.Next(100000, 2000000)},'201{rnd.Next(4,10)}-{rnd.Next(1, 13)}-{rnd.Next(1, 28)}')");
                queryBuilder.Append((i == activityCount - 1 ? ";" : ","));
            }
            return queryBuilder.ToString();
        }

        private static void EnsureDatabaseCreated()
        {

            var tables = new string[]
            {
                    @"[Id] [uniqueidentifier] NOT NULL DEFAULT newid() PRIMARY KEY,
                      [Name] [nvarchar](20) NOT NULL,
                      [ActivityType] [bit] NOT NULL",
                    @"[Id] [uniqueidentifier] DEFAULT newid() PRIMARY KEY,
                      [CategoryId] [uniqueidentifier] NOT NULL FOREIGN KEY REFERENCES Categories(Id) ON DELETE CASCADE ON UPDATE CASCADE,
                      [Description] [nvarchar](50) NULL,
                      [Value] [decimal] NULL,
                      [Created] [datetime2] DEFAULT getdate(),
                      [Date] [datetime2] NOT NULL"
            };

            EnsureDatabaseCreatedExecuteQuerys(tables);
        }

        private static void EnsureDatabaseCreatedExecuteQuerys(string[] tables)
        {
            string query = $"SELECT database_id FROM sys.databases WHERE Name='{databaseName}'";
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    var result = command.ExecuteScalar();
                    if (result == null)
                    {
                        query = $"CREATE DATABASE {databaseName}";
                        command.CommandText = query;
                        command.ExecuteNonQuery();
                        for (int i = 0; i < tables.Length; i++)
                        {
                            query = $@"CREATE TABLE {databaseName}.{schemaName}.{tableNames[i]} ({tables[i]});";
                            command.CommandText = query;
                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    throw;
                }
            }
        }
    }
}

