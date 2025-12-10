using Microsoft.VisualBasic.FileIO;
using Supabase;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.IO;

namespace ConnectSQLite
{
    class Program
    {
        #region SQLite 활용

        static void Main(string[] args)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Autodesk\Revit\Addins\2025\sampleDB.db");
            DatabaseManager dbManager = new DatabaseManager(dbPath);
            string tableName = "SampleTable4";
            dbManager.CreateTableIfNotExists(tableName);
            dbManager.InsertData(tableName, new List<int> { 123, 456, 789 }, new List<string> { "Alice", "Bob", "Charlie" });
            dbManager.UpdateData(tableName, 123, "Apple");
            dbManager.UpdateData(tableName, 456, "Bravo");

            var names = dbManager.GetData(tableName, "Name");
            foreach (var item in names)
            {
                Console.WriteLine(item);
            }
        }
        #endregion
    }
}
