using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiRest.Repository
{
    public interface IGenericRepository
    {
        Task ExecuteAsync(string sql);
        Task ExecuteAsync(string sql, object param);
        Task ExecuteSPAsync(string name);
        Task ExecuteSPAsync(string name, object param);
        Task<object> ExecuteScalarAsync(string name);
        Task<object> ExecuteScalarAsync(string name, object param);
        Task<object> ExecuteScalarSPAsync(string name);
        Task<object> ExecuteScalarSPAsync(string name, object param);
        Task<T> GetAsync<T>(string sql);
        Task<T> GetAsync<T>(string sql, object param);
        Task<T> GetSPAsync<T>(string name, int id);
        Task<T> GetSPAsync<T>(string name, object param);
        Task<IEnumerable<T>> GetAllAsync<T>(string sql);
        Task<IEnumerable<T>> GetAllAsync<T>(string sql, object param);
        Task<IEnumerable<T>> GetAllSPAsync<T>(string name);
        Task<IEnumerable<T>> GetAllSPAsync<T>(string name, object param);
    }
}
