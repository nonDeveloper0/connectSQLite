using Microsoft.VisualBasic.FileIO;
using Supabase;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.IO;

namespace ConnectSupabase
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

                // 클라이언트 가져오기 + 조회 test
                Client client = supabase.GetClient();
                var a = await client.From<Rebar>().Select("rebarDiameter").Get(); // 예시 쿼리
                foreach (var b in a.Models)
                    Console.WriteLine(b.Diameter);

                // SELECT ALL
                var selectAllElments = await supabase.GetAllDataAsync<Element>();

                // SELECT - 특정 컬럼
                Console.WriteLine("\n---특정 컬럼 조회---");
                var selectedColumnsElements = await supabase.GetDataByColumnAsync<Element>("category", "elementId");
                foreach (var element in selectAllElments)
                    Console.WriteLine($"Category: {element.Category}, ElementId: {element.ElementId}");

                // SELECT - 조건부
                // WHERE 조건을 직접 넣을 수 있는지 확인? 현재는 지정한 열 데이터가 지정한 값과 동일한 경우만 조회
                // ex. 특정 값이 >, <, >=, <= 인 경우 등
                Console.WriteLine("\n---데이터 조회 WHERE---");
                var filteredElements = await supabase.GetDataByFilterAsync<Element>("category", "Doors");
                foreach (var element in filteredElements)
                    Console.WriteLine($"Category: {element.Category}, ElementId: {element.ElementId}");

                /*
                // INSERT- 단일
                Console.WriteLine("\n---데이터 조회 WHERE---");
                var newElement = new Element { Category = "Furniture", ElementId = 999999 };
                var insertedElement = await supabase.InsertDataAsync(newElement);
                Console.WriteLine($"Inserted Element - Category: {insertedElement.Category}, ElementId: {insertedElement.ElementId}");

                // INSERT - 다중
                Console.WriteLine("\n---다중 데이터 삽입---");
                var bulkElements = new List<Element>
                {
                    new Element { Category = "Furniture", ElementId = 111111 },
                    new Element { Category = "Fixtures", ElementId = 222222 },
                    new Element { Category = "Equipment", ElementId = 333333 },
                };
                var insertedBulkElements = await supabase.InsertBulkDataAsync(bulkElements);
                foreach (var element in insertedBulkElements)
                    Console.WriteLine($"Inserted Element - Category: {element.Category}, ElementId: {element.ElementId}");
                */

                // UPDATE
                Console.WriteLine("\n---단일 업데이트---");
                var updateElement = new Element
                {
                    Category = "NewCategory",
                    ElementId = 333333,
                };
                var result = await supabase.UpdateDataAsync(updateElement);
                Console.WriteLine($"Updated Element - Category: {result.Category}, ElementId: {result.ElementId}");

                // UPSERT - 단일
                Console.WriteLine("\n---단일 업서트---");

                var upsertElement = new Element
                {
                    Category = "upsert단일",
                    ElementId = 222222,
                };
                var upsertedResult = await supabase.UpsertDataAsync(upsertElement, "elementId");

                // UPSERT - 다중
                Console.WriteLine("\n---다중 업서트---");

                List<Element> newElements = new List<Element>
                {
                    new Element { Category = "Walls", ElementId = 123456 },
                    new Element { Category = "Doors", ElementId = 897654 },
                    new Element { Category = "Windows", ElementId = 456789 },
                    new Element { Category = "BuiltInCategory.Walls", ElementId = 846554 },
                    new Element { Category = "BuiltInCategory.Slabs", ElementId = 546521 },
                    new Element { Category = "BuiltInCategory.New", ElementId = 77777 },
                };
                List<Element> upsertedElements = await supabase.UpsertBulkDataAsync(newElements, "elementId");

                // DELETE
                Console.WriteLine("\n---Delete---");
                // await client.From<Element>().Where(e => e.ElementId == 546521).Delete();
                await supabase.DeleteDataAsync<Element>("elementId", 77777);
                                
                // 최종 결과 확인
                Console.WriteLine("\n---전체 row 출력---");
                foreach (var e in selectAllElments)
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
        [Column("category")]
        public string Category { get; set; }
        [Column("elementId")]
        [PrimaryKey("elementId")]
        public int ElementId { get; set; }
    }

    [Table("Rebar")]
    public class Rebar : BaseModel
    {
        [Column("rebarDiameter")]
        [PrimaryKey("rebarDiameter")]
        public double Diameter { get; set; }
    }

    #endregion
}
