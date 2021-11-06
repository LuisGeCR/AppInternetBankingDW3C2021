using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AppWebInternetBanking.Models;
namespace AppWebInternetBanking.Controllers
{
    public class ErrorManager
    {
        string UrlBase = "http://localhost:49220/api/Errors/";

        public async Task<Error> Ingresar(Error error)
        {
            HttpClient httpClient = new HttpClient();

            var response = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(error), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Error>(await response.Content.ReadAsStringAsync());
        }

        public async Task<IEnumerable<Error>> ObtenerErrores()
        {
            HttpClient httpClient = new HttpClient();

            var response = await httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<Error>>(response);
        }
    }
}