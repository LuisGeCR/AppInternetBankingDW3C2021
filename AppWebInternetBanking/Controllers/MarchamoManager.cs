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
    public class MarchamoManager
    {
        string UrlBase = "http://localhost:49220/api/Marchamos/";

        HttpClient GetClient(string token)
        {
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", token);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            return httpClient;
        }

        public async Task<Marchamo> ObtenerMarchamo(string codigo, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await
                httpClient.GetStringAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<Marchamo>(response);
        }

        public async Task<IEnumerable<Marchamo>> ObtenerMarchamos(string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await
                httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<Marchamo>>(response);
        }

        public async Task<Marchamo> Ingresar(Marchamo marchamo, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(marchamo), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Marchamo>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Marchamo> Actualizar(Marchamo marchamo, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PutAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(marchamo), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Marchamo>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Marchamo> Eliminar(string id, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.DeleteAsync(string.Concat(UrlBase, id));

            return JsonConvert.DeserializeObject<Marchamo>(await response.Content.ReadAsStringAsync());
        }
    }
}