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
    public class SesionManager
    {
        string UrlBase = "http://localhost:49220/api/Sesions/";

        HttpClient GetClient(string token)
        {
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", token);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            return httpClient;
        }

        public async Task<Sesion> Ingresar(Sesion sesion, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(sesion), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Sesion>(await response.Content.ReadAsStringAsync());
        }

        public async Task<IEnumerable<Sesion>> ObtenerSesiones(string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<Sesion>>(response);
        }
    }
}