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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        #region Methods

        //// 데이터 조회
        //private static async Task GetDataAsync()
        //{
        //    try
        //    {
        //        var result = await _supabaseClient
        //            .From<Student>()
        //            .Get();

        //        if (result != null && result.Models != null && result.Models.Count > 0)
        //        {
        //            Console.WriteLine($"조회된 학생 수: {result.Models.Count}\n");

        //            foreach (var student in result.Models)
        //            {
        //                Console.WriteLine($"Category: {student.Category}");
        //                Console.WriteLine($"ElementId: {student.ElementId}");
        //                Console.WriteLine("---");
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine("조회된 데이터가 없습니다.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"❌ 데이터 조회 실패: {ex.Message}");
        //        throw;
        //    }
        //}

    }


    // 테이블
    [Table("Student")]
    public class Student : BaseModel
    {
        // id - int8 (BaseModel에서 자동 상속)
        // created_at - timestamp (BaseModel에서 자동 상속)

        [Column("category")]
        public string Category { get; set; }

        [Column("elementid")]
        public int ElementId { get; set; }
    }
    #endregion

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
