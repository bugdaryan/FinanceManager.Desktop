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
        static string[] tableNames = new string[] { "Categories", "Wallets", "Users", "Activities" };
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

            string[] names = new string[]
            {
                "'Spartak'", "'Benedict'", "'Eduardo'", "'Leila'",
                "'John'", "'Fleur'", "'Vrur'", "'Parur'", "'Aubrey'",
                "'Yair'", "'Leonardo'" ,"'Payne'","'Carl'", "'Henson'",
                "'Jensen'" ,"'Combs'", "'Amiah'", "'Burton'", "'Kamren'"
            };

            Wallet[] wallets = new Wallet[names.Length];
            User[] users = new User[wallets.Length];

            Random rnd = new Random();

            var query = GetSeedQuery(categories, wallets, users, names, rnd);
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    for (int i = 0; i < 100; i++)
                    {
                        command.CommandText = GetActivityQuery(categories, rnd, wallets);
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

        static string GetSeedQuery(Category[] categories, Wallet[] wallets, User[] users, string[] names, Random rnd)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append($"DELETE FROM {databaseName}.{schemaName}.Activities;DELETE FROM {databaseName}.{schemaName}.Users;DELETE FROM {databaseName}.{schemaName}.Wallets;DELETE FROM {databaseName}.{schemaName}.Categories;");

            queryBuilder.Append($"INSERT INTO {databaseName}.{schemaName}.Categories (Id, Name, ActivityType) VALUES ");
            for (int i = 0; i < categories.Length; i++)
            {
                queryBuilder.Append($" ('{categories[i].Id}',{categories[i].Name},{(int)categories[i].ActivityType})");
                queryBuilder.Append((i == categories.Length - 1 ? ";" : ","));
            }

            queryBuilder.Append($"INSERT INTO {databaseName}.{schemaName}.Wallets (Id, Balance) VALUES ");
            for (int i = 0; i < wallets.Length; i++)
            {
                wallets[i] = new Wallet { Id = Guid.NewGuid(), Balance = rnd.Next(10000, 50000) };
                users[i] = new User { Id = Guid.NewGuid(), Wallet = wallets[i], FullName = names[i], Username = names[i] };
                queryBuilder.Append($" ('{wallets[i].Id}',{wallets[i].Balance})");
                queryBuilder.Append((i == wallets.Length - 1 ? ";" : ","));
            }

            queryBuilder.Append($"INSERT INTO {databaseName}.{schemaName}.Users (Id, WalletId, FullName, Username) VALUES");
            for (int i = 0; i < users.Length; i++)
            {
                queryBuilder.Append($" ('{users[i].Id}','{users[i].Wallet.Id}',{users[i].FullName},{users[i].Username})");
                queryBuilder.Append((i == wallets.Length - 1 ? ";" : ","));
            }



            return queryBuilder.ToString();
        }

        private static string GetActivityQuery(Category[] categories, Random rnd, Wallet[] wallets)
        {
            StringBuilder queryBuilder = new StringBuilder();
            int activityCount = 1000;
            queryBuilder.Append($"INSERT INTO {databaseName}.{schemaName}.Activities (CategoryId, WalletId, Description, Value, Date) VALUES ");
            for (int i = 0; i < activityCount; i++)
            {
                Guid categoryGuid = categories[rnd.Next(0, categories.Length)].Id;
                Guid walletGuid = wallets[rnd.Next(0, wallets.Length)].Id;
                queryBuilder.Append($" ('{categoryGuid}','{walletGuid}',{categories[rnd.Next(0, categories.Length)].Name},{rnd.Next(100000, 2000000)},'2019-{rnd.Next(1, 13)}-{rnd.Next(1, 28)}')");
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
                      [Balance] [decimal](18, 0) NULL",
                    @"[Id] [uniqueidentifier] NOT NULL DEFAULT newid() PRIMARY KEY,
                      [FullName] [nvarchar](50) NOT NULL,
                      [Username] [nvarchar](20) NOT NULL UNIQUE,
                      [WalletId] [uniqueidentifier] NULL FOREIGN KEY REFERENCES Wallets(Id),",
                    @"[Id] [uniqueidentifier] DEFAULT newid() PRIMARY KEY,
                      [CategoryId] [uniqueidentifier] NOT NULL FOREIGN KEY REFERENCES Categories(Id),
                      [WalletId] [uniqueidentifier] NOT NULL FOREIGN KEY REFERENCES Wallets(Id),
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

