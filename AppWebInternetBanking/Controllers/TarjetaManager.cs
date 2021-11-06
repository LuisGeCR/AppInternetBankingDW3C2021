using AppWebInternetBanking.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AppWebInternetBanking.Controllers
{
    public class TarjetaManager
    {
        string UrlBase = "http://localhost:49220/api/Tarjetas/";

        HttpClient GetClient(string token)
        {
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", token);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            return httpClient;
        }

        public async Task<Tarjeta> ObtenerTarjeta(string codigo, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await
                httpClient.GetStringAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<Tarjeta>(response);
        }

        public async Task<IEnumerable<Tarjeta>> ObtenerTarjetas(string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await
                httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<Tarjeta>>(response);
        }

        public async Task<Tarjeta> Ingresar(Tarjeta tarjeta, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(tarjeta), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Tarjeta>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Tarjeta> Actualizar(Tarjeta tarjeta, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PutAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(tarjeta), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Tarjeta>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Tarjeta> Eliminar(string id, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.DeleteAsync(string.Concat(UrlBase, id));

            return JsonConvert.DeserializeObject<Tarjeta>(await response.Content.ReadAsStringAsync());
        }
    }
}