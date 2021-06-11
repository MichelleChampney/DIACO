using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiRest.Repository
{
    public class GenericRepository : IGenericRepository
    {
        private readonly IDbConnection db;
        public GenericRepository(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("CS"));
        }

        public async Task ExecuteAsync(string sql)
        {
            await db.ExecuteAsync(sql, null);
        }

        public async Task ExecuteAsync(string sql, object param)
        {
            await db.ExecuteAsync(sql, param);
        }

        public async Task ExecuteSPAsync(string name)
        {
            await db.ExecuteAsync(name, null, commandType: CommandType.StoredProcedure);
        }

        public async Task ExecuteSPAsync(string name, object param)
        {
            await db.ExecuteAsync(name, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<object> ExecuteScalarAsync(string sql)
        {
            return await db.ExecuteScalarAsync(sql, null);
        }

        public async Task<object> ExecuteScalarAsync(string sql, object param)
        {
            return await db.ExecuteScalarAsync(sql, param);
        }

        public async Task<object> ExecuteScalarSPAsync(string name)
        {
            return await db.ExecuteScalarAsync(name, null, commandType: CommandType.StoredProcedure);
        }

        public async Task<object> ExecuteScalarSPAsync(string name, object param)
        {
            return await db.ExecuteScalarAsync(name, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<T> GetAsync<T>(string sql)
        {
            var result = await db.QueryAsync<T>(sql);
            return result.FirstOrDefault();
        }

        public async Task<T> GetAsync<T>(string sql, object param)
        {
            var result = await db.QueryAsync<T>(sql, param);
            return result.FirstOrDefault();
        }

        public async Task<T> GetSPAsync<T>(string name, int id)
        {
            return await GetSPAsync<T>(name, new { id });
        }
        public async Task<T> GetSPAsync<T>(string name, object param)
        {
            var result = await db.QueryAsync<T>(name, param, commandType: CommandType.StoredProcedure);

            if (result != null)
                return result.FirstOrDefault();

            return default(T);
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(string sql)
        {
            return await db.QueryAsync<T>(sql);
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(string sql, object param)
        {
            return await db.QueryAsync<T>(sql, param);
        }

        public async Task<IEnumerable<T>> GetAllSPAsync<T>(string name)
        {
            var result = await db.QueryAsync<T>(name, commandType: CommandType.StoredProcedure);

            if (result != null)
                return result.ToList();

            return new List<T>();
        }

        public async Task<IEnumerable<T>> GetAllSPAsync<T>(string name, object param)
        {
            var result = await db.QueryAsync<T>(name, param, commandType: CommandType.StoredProcedure);

            if (result != null)
                return result.ToList();

            return new List<T>();
        }
    }
}
