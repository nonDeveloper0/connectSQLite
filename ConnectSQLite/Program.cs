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
        #region Supabase 활용
        // Supabase 프로젝트 정보
        private const string SUPABASE_URL = "https://ziyybfaaosqmutgsfzav.supabase.co";
        private const string SUPABASE_KEY = "sb_publishable_e5wJLZR6apBZR7K74kwvQA_zO0w0IMy";

        // Main 메서드 - async 사용
        static async Task Main(string[] args)
        {
            try
            {
                var supabase = new SupabaseManager(SUPABASE_URL, SUPABASE_KEY);
                await supabase.InitializeAsync();

                var task = supabase.GetAllDataAsync<Element>(); //Task 반환. 아직 완료 안됨
                List<Element> elments = await task;             //실제 데이터 반환

                foreach (var e in elments)
                {
                    Console.WriteLine($"Category: {e.Category}, ElementId: {e.ElementId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    // 테이블
    [Table("Element")]
    public class Element : BaseModel
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("category")]
        public string Category { get; set; }
        [Column("elementId")]
        public int ElementId { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
    #endregion

    #region SQLite 활용
    /*
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
    */
    #endregion
}
