using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace WebApp.Data
{
    public class ConsumoAPI
    {
        public static string Execute(string url, string pMethod, string token)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = pMethod;
            if (string.IsNullOrEmpty(token) == false)
                request.Headers.Add("Authorization", "bearer " + token);
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.ContentLength = 0;

            try
            {
                WebResponse response = request.GetResponse();
                Stream strReader = response.GetResponseStream();
                if (strReader == null) return "";
                StreamReader objReader = new StreamReader(strReader);
                return objReader.ReadToEnd();
            }
            catch(WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    throw new Exception(reader.ReadToEnd());
                }
            }
            catch
            {
                throw;
            }
        }

        public static string Execute(string url, string pMethod, string token, object obj)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = pMethod;
            if (string.IsNullOrEmpty(token) == false)
                request.Headers.Add("Authorization", "bearer " + token);
            request.ContentType = "application/json";
            request.Accept = "application/json";

            string json = JsonConvert.SerializeObject(obj);
            byte[] byteArray = Encoding.UTF8.GetBytes(json.ToString());
            request.ContentLength = byteArray.Length;

            // Escribir la peticion
            Stream postStream = request.GetRequestStream();
            postStream.Write(byteArray, 0, byteArray.Length);
            postStream.Close();

            try
            {
                WebResponse response = request.GetResponse();
                Stream strReader = response.GetResponseStream();
                if (strReader == null) return "";
                StreamReader objReader = new StreamReader(strReader);
                return objReader.ReadToEnd();
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    throw new Exception(reader.ReadToEnd());
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
