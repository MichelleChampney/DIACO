using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Repository
{
    public interface IGenericRepository
    {
        string GetAll(string pController, string parameters, string token);
        string Get(int id, string pController, string token);
        void Post<T>(T obj, string pController, string token);
        string PostScalar<T>(T obj, string pController, string token);
        void Put<T>(int id, T obj, string pController, string token);
        void Delete(int id, string pController, string token);
        string GetAllValueList(string pController, string parameters, bool valorVacio, string valor, string action = "GetAllValueList");
    }
}
