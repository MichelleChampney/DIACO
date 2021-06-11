using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Data;

namespace WebApp.Repository
{
    public class GenericRepository : IGenericRepository
    {
        private static IConfiguration _configuration;
        public GenericRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetAll(string pController, string parameters, string token)
        {
            return ConsumoAPI.Execute($"{_configuration["API"]}/{pController}/GetAll{parameters}", "GET", token);
        }

        public string Get(int id, string pController, string token)
        {
            return ConsumoAPI.Execute($"{_configuration["API"]}/{pController}/{id}", "GET", token);
        }

        public void Post<T>(T obj, string pController, string token)
        {
            ConsumoAPI.Execute($"{_configuration["API"]}/{pController}", "POST", token, obj);
        }

        public string PostScalar<T>(T obj, string pController, string token)
        {
            return ConsumoAPI.Execute($"{_configuration["API"]}/{pController}", "POST", token, obj);
        }

        public void Put<T>(int id, T obj, string pController, string token)
        {
            ConsumoAPI.Execute($"{_configuration["API"]}/{pController}/{id}", "PUT", token, obj);
        }

        public void Delete(int id, string pController, string token)
        {
            ConsumoAPI.Execute($"{_configuration["API"]}/{pController}/{id}", "DELETE", token);
        }

        public string GetAllValueList(string pController, string parameters, bool valorVacio, string valor, string action = "GetAllValueList")
        {
            return ConsumoAPI.Execute($"{_configuration["API"]}/{pController}/{action}{parameters}/{valorVacio}/{valor}", "GET", string.Empty);
        }
    }
}
