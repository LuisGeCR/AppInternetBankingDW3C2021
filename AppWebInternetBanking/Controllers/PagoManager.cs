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
    public class PagoManager
    {
        string UrlBase = "http://localhost:49220/api/Pagos/";

        HttpClient GetClient(string token)
        {
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", token);

            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            return httpClient;
        }

        public async Task<Pago> ObtenerPago(string token, string codigo)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<Pago>(response);
        }

        public async Task<IEnumerable<Pago>> ObtenerPagos(string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<Pago>>(response);
        }

        public async Task<Pago> Ingresar(Pago pago, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(pago),
                Encoding.UTF8,
                "application/json"));

            return JsonConvert.DeserializeObject<Pago>(await
                response.Content.ReadAsStringAsync());
        }

        public async Task<Pago> Actualizar(Pago pago, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PutAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(pago),
                Encoding.UTF8,
                "application/json"));

            return JsonConvert.DeserializeObject<Pago>(await response.
                Content.ReadAsStringAsync());
        }

        public async Task<Pago> Eliminar(string id, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.DeleteAsync(string.Concat(UrlBase, id));

            return JsonConvert.DeserializeObject<Pago>(await
                response.Content.ReadAsStringAsync());
        }
    }
}