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
    public class TransferenciaManager
    {
        string UrlBase = "http://localhost:49220/api/Transferencias/";

        HttpClient GetClient(string token)
        {
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", token);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            return httpClient;
        }

        public async Task<Transferencia> ObtenerTransferencia(string token, string codigo)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<Transferencia>(response);
        }

        public async Task<IEnumerable<Transferencia>> ObtenerTransferencias(string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<Transferencia>>(response);
        }

        public async Task<Transferencia> Ingresar(Transferencia transferencia, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(transferencia),
                Encoding.UTF8,
                "application/json"));

            return JsonConvert.DeserializeObject<Transferencia>(await
                response.Content.ReadAsStringAsync());
        }

        public async Task<Transferencia> Actualizar(Transferencia transferencia, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PutAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(transferencia),
                Encoding.UTF8,
                "application/json"));

            return JsonConvert.DeserializeObject<Transferencia>(await response.
                Content.ReadAsStringAsync());
        }

        public async Task<Transferencia> Eliminar(string id, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.DeleteAsync(string.Concat(UrlBase, id));

            return JsonConvert.DeserializeObject<Transferencia>(await
                response.Content.ReadAsStringAsync());
        }
    }
}
