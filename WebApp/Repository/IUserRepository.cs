using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Repository
{
    public interface IUserRepository : IGenericRepository
    {
        string GetCuenta(string usuario, string pController);
        string PostToken<T>(T obj, string pController);
        void PutPassword<T>(int id, T obj, string pController, string token);
    }
}
