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
    public class EstadisticaManager
    {
        string UrlBase = "http://localhost:49220/api/Estadistica/";

        public async Task<Estadistica> Ingresar(Estadistica estdistica)
        {
            HttpClient httpClient = new HttpClient();

            var response = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(estdistica), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Estadistica>(await response.Content.ReadAsStringAsync());
        }

        public async Task<IEnumerable<Estadistica>> ObtenerEstadisticas()
        {
            HttpClient httpClient = new HttpClient();

            var response = await httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<Estadistica>>(response);
        }
    }
}