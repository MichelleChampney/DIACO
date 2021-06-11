using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Repository
{
    public interface IQuejaRepository : IGenericRepository
    {
        string GetQueja(string codigo , string pController, string token);
        string GetConsulta(string method, string paramters, string token);
        void PostSeguimiento<T>(T obj, string pController, string token);
    }
}
