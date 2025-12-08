using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace ConnectSQLite
{
    class Program
    {
        static void Main(string[] args)
        {
            string dbPath = @"C:\Users\admin\OneDrive\바탕 화면\Sample\sampleDB.db";
            DatabaseManager dbManager = new DatabaseManager(dbPath);
            dbManager.CreateTableIfNotExists("SampleTable");
            //dbManager.InsertData("SampleTable", new List<int> { 123, 456, 789 }, new List<string> { "Alice", "Bob", "Charlie" });
            dbManager.UpdateData("SampleTable", 123, "Alpha");
            dbManager.UpdateData("SampleTable", 456, "Bravo");

            var names = dbManager.GetData("SampleTable", "Name");
            foreach (var item in names)
            {
                Console.WriteLine(item);
            }
        }

        #region Methods
        // 테이블 생성 메서드
        private static void CreateTableIfNotExists(SqliteConnection connection, string tableName)
        {
            string sql = $@"
                    CREATE TABLE IF NOT EXISTS {tableName} (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL
                );";

            using (SqliteCommand command = new SqliteCommand(sql, connection))
            {
                int affectedRows = command.ExecuteNonQuery();  //쿼리를 실행하고 영향받은 행 수를 반환
            }
        }
        #endregion
    }
}