using Microsoft.Data.Sqlite;
using Supabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConnectSQLite
{
    public class SupabaseManager
    {
        #region Fields
        // Supabase 클라이언트
        private readonly Client _supabaseClient;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public SupabaseManager(string supabaseUrl, string publishableKey)
        {
            var options = new SupabaseOptions
            {
                AutoRefreshToken = true,    // 토큰 자동 갱신
                AutoConnectRealtime = true,  // 실시간 연결 자동 설정
            };

            _supabaseClient = new Client(supabaseUrl, publishableKey, options);
        }
        #endregion

        #region Methods
        // 초기화
        public async Task InitializeAsync()
        {
            try
            {
                Console.WriteLine("Supabase 연결중...");
                await _supabaseClient.InitializeAsync();
                Console.WriteLine("Supabase 연결 성공");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Supabase 초기화 실패: {ex.Message}");
                throw;
            }
        }
        #endregion
    }
}
