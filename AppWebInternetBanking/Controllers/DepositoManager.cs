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
    public class DepositoManager
    {
        string UrlBase = "http://localhost:49220/api/Depositos/";

        HttpClient GetClient(string token)
        {
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", token);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            return httpClient;
        }

        public async Task<Deposito> ObtenerDeposito(string codigo, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await
                httpClient.GetStringAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<Deposito>(response);
        }

        public async Task<IEnumerable<Deposito>> ObtenerDepositos(string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await
                httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<Deposito>>(response);
        }

        public async Task<Deposito> Ingresar(Deposito deposito, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(deposito), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Deposito>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Deposito> Actualizar(Deposito deposito, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PutAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(deposito), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Deposito>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Deposito> Eliminar(string id, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.DeleteAsync(string.Concat(UrlBase, id));

            return JsonConvert.DeserializeObject<Deposito>(await response.Content.ReadAsStringAsync());
        }
    }
}