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
    public class CreditoManager
    {
        string UrlBase = "http://localhost:49220/api/Creditos/";

        HttpClient GetClient(string token)
        {
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", token);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            return httpClient;
        }

        public async Task<Credito> ObtenerCredito(string codigo, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await
                httpClient.GetStringAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<Credito>(response);
        }

        public async Task<IEnumerable<Credito>> ObtenerCreditos(string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await
                httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<Credito>>(response);
        }

        public async Task<Credito> Ingresar(Credito credito, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(credito), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Credito>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Credito> Actualizar(Credito credito, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PutAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(credito), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Credito>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Credito> Eliminar(string id, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.DeleteAsync(string.Concat(UrlBase, id));

            return JsonConvert.DeserializeObject<Credito>(await response.Content.ReadAsStringAsync());
        }
    }
}