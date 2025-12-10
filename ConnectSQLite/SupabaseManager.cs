using Microsoft.Data.Sqlite;
using Supabase;
using Supabase.Postgrest.Models;
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

        // SELECT - 전체 조회
        public async Task<List<T>> GetAllDataAsync<T>() where T : BaseModel, new()
        {
            try
            {
                var result = await _supabaseClient
                    .From<T>()
                    .Get();

                return result?.Models ?? new List<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 조회 실패: {ex.Message}");
                throw;
            }
        }

        // SELECT - 특정 컬럼 조회
        public async Task<List<T>> GetDataByColumnAsync<T>(params string[] columnNames) where T : BaseModel, new()
        {
            try
            {
                string columns = string.Join(",", columnNames);
                var result = await _supabaseClient
                                   .From<T>()
                                   .Select(columns)
                                   .Get();

                return result?.Models ?? new List<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 컬럼 조회 실패: {ex.Message}");
                throw;
            }
        }

        // SELECT - 조건부 조회 (WHERE)
        public async Task<List<T>> GetDataByFilterAsync<T>(string columnName, string filterValue)
            where T : BaseModel, new()
        {
            try
            {
                var result = await _supabaseClient
                    .From<T>()
                    .Select("*")
                    .Filter(columnName, Supabase.Postgrest.Constants.Operator.Equals, filterValue)
                    .Get();

                return result?.Models ?? new List<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 필터링 조회 실패: {ex.Message}");
                throw;
            }
        }

        // INSERT - 단일 데이터 입력
        public async Task<T> InsertDataAsync<T>(T data) where T : BaseModel, new()
        {
            try
            {
                var result = await _supabaseClient
                    .From<T>()
                    .Insert(data);

                Console.WriteLine("✅ Insert 완료");
                return result?.Models?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Insert 실패: {ex.Message}");
                throw;
            }
        }

        // INSERT - 대량 데이터 입력
        public async Task<List<T>> InsertBulkDataAsync<T>(List<T> dataList) where T : BaseModel, new()
        {
            try
            {
                var result = await _supabaseClient
                    .From<T>()
                    .Insert(dataList);

                Console.WriteLine($"✅ Bulk Insert 완료 ({dataList.Count}개)");
                return result?.Models ?? new List<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Bulk Insert 실패: {ex.Message}");
                throw;
            }
        }

        // UPDATE - 데이터 업데이트
        public async Task<T> UpdateDataAsync<T>(T data) where T : BaseModel, new()
        {
            try
            {
                var result = await _supabaseClient
                    .From<T>()
                    .Update(data);

                Console.WriteLine("✅ Update 완료");
                return result?.Models?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Update 실패: {ex.Message}");
                throw;
            }
        }

        // UPSERT - Insert와 Update를 합친 기능 (있으면 Update, 없으면 Insert)
        public async Task<T> UpsertDataAsync<T>(T data) where T : BaseModel, new()
        {
            try
            {
                var result = await _supabaseClient
                    .From<T>()
                    .Upsert(data);

                Console.WriteLine("✅ Upsert 완료 (Insert 또는 Update)");
                return result?.Models?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Upsert 실패: {ex.Message}");
                throw;
            }
        }

        // UPSERT - 대량 데이터
        public async Task<List<T>> UpsertBulkDataAsync<T>(List<T> dataList) where T : BaseModel, new()
        {
            try
            {
                var result = await _supabaseClient
                    .From<T>()
                    .Upsert(dataList);

                Console.WriteLine($"✅ Bulk Upsert 완료 ({dataList.Count}개)");
                return result?.Models ?? new List<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Bulk Upsert 실패: {ex.Message}");
                throw;
            }
        }

        // DELETE - 데이터 삭제
        public async Task DeleteDataAsync<T>(string columnName, string filterValue)
            where T : BaseModel, new()
        {
            try
            {
                await _supabaseClient
                    .From<T>()
                    .Where(x => x.ToString().Contains(filterValue))
                    .Delete();

                Console.WriteLine("✅ Delete 완료");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Delete 실패: {ex.Message}");
                throw;
            }
        }

        // 인증: 회원가입
        public async Task<Supabase.Gotrue.Session> SignUpAsync(string email, string password)
        {
            try
            {
                var session = await _supabaseClient.Auth.SignUp(email, password);
                Console.WriteLine($"✅ 회원가입 성공: {email}");
                return session;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 회원가입 실패: {ex.Message}");
                throw;
            }
        }

        // 인증: 로그인
        public async Task<Supabase.Gotrue.Session> SignInAsync(string email, string password)
        {
            try
            {
                var session = await _supabaseClient.Auth.SignInWithPassword(email, password);
                Console.WriteLine($"✅ 로그인 성공: {email}");
                return session;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 로그인 실패: {ex.Message}");
                throw;
            }
        }

        // 인증: 로그아웃
        public async Task SignOutAsync()
        {
            try
            {
                await _supabaseClient.Auth.SignOut();
                Console.WriteLine("✅ 로그아웃 성공");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 로그아웃 실패: {ex.Message}");
                throw;
            }
        }

        // 현재 사용자 정보
        public Supabase.Gotrue.User GetCurrentUser()
        {
            return _supabaseClient.Auth.CurrentUser;
        }
        #endregion
    }
}

