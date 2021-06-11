using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Data;

namespace WebApp.Repository
{
    public class UserRepository : GenericRepository, IUserRepository
    {
        private static IConfiguration _configuration;
        public UserRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public string GetCuenta(string usuario, string pController)
        {
            return ConsumoAPI.Execute($"{_configuration["API"]}/{pController}/GetCuenta/{usuario}", "GET", string.Empty);
        }

        public string PostToken<T>(T obj, string pController)
        {
            return ConsumoAPI.Execute($"{_configuration["API"]}/{pController}/PostToken", "POST", string.Empty, obj);
        }

        public void PutPassword<T>(int id, T obj, string pController, string token)
        {
            ConsumoAPI.Execute($"{_configuration["API"]}/{pController}/PutPassword/{id}", "PUT", token, obj);
        }
    }
}
