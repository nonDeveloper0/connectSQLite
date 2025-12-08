using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace ConnectSQLite
{
    public class DatabaseManager
    {
        #region Fields
        private readonly SqliteConnection connection;
        #endregion

        #region Properties

        #endregion

        #region Constructors
        public DatabaseManager(string dbPath)
        {
            connection = new SqliteConnection($"Data Source={dbPath}");            connection.Open();
            connection.Open();
        }
        #endregion

        #region Methods
        // CREATE
        public void CreateTableIfNotExists(string tableName)
        {
            string sql = $"""
                CREATE TABLE IF NOT EXISTS {tableName} 
                (Name TEXT, 
                Id INTEGER PRIMARY KEY)
                """;
            using var command = new SqliteCommand(sql, connection);
            command.ExecuteNonQuery();
        }

        // INSERT - Bulk 입력시 transaction 사용 권장
        public void InsertData(string tableName, List<int> ids, List<string> names)
        {
            using var transaction = connection.BeginTransaction();
            try
            {
                string sql = $"""
                    INSERT INTO {tableName} (Name, Id)
                    VALUES (@name, @id)
                    """;
                using var command = new SqliteCommand(sql, connection, transaction);

                // 파라미터 정의 (타입 명시)
                command.Parameters.Add("@name", SqliteType.Text);
                command.Parameters.Add("@id", SqliteType.Integer);

                for (int i = 0; i < ids.Count; i++)
                {
                    command.Parameters["@id"].Value = ids[i];
                    command.Parameters["@name"].Value = names[i];
                    command.ExecuteNonQuery();  // 생략 불가. "실제 저장 실행" 명령
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"오류 발생: {ex.Message}");
                transaction.Rollback();
            }
        }

        // UPDATE
        public void UpdateData(string tableName, int id, string newName)
        {
            string sql = $"""
                UPDATE {tableName}
                SET Name = @newName
                WHERE Id = @id
                """;
            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@newName", newName);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        // SELECT 조회
        public List<object> GetData(string tableName, string columnName)
        {
            var data = new List<object>();
            string sql = $"""
                SELECT {columnName} FROM {tableName}
                """;
            using var command = new SqliteCommand(sql, connection);
            using var reader = command.ExecuteReader();

            // 읽는 동안
            while (reader.Read())
            {
                data.Add(reader.GetValue(0));
            }

            return data;
        }

        #endregion
    }
}
