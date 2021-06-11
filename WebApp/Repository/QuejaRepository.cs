using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Data;

namespace WebApp.Repository
{
    public class QuejaRepository : GenericRepository, IQuejaRepository
    {
        private static IConfiguration _configuration;
        public QuejaRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public void PostSeguimiento<T>(T obj, string pController, string token)
        {
            ConsumoAPI.Execute($"{_configuration["API"]}/{pController}/PostSeguimiento", "POST", token, obj);
        }

        public string GetQueja(string codigo, string pController, string token)
        {
            return ConsumoAPI.Execute($"{_configuration["API"]}/{pController}/{codigo}", "GET", token);
        }

        public string GetConsulta(string method, string parameters, string token)
        {
            return ConsumoAPI.Execute($"{_configuration["API"]}/Quejas/{method}{parameters}", "GET", token);
        }
    }
}
