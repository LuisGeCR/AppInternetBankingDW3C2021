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
    public class CuentaManager
    {
        string UrlBase = "http://localhost:49220/api/Cuentas/";

        HttpClient GetClient(string token)
        {
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", token);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            return httpClient;
        }

        public async Task<Cuenta> ObtenerCuenta(string token, string codigo)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(string.Concat(UrlBase, codigo));

            return JsonConvert.DeserializeObject<Cuenta>(response);
        }

        public async Task<IEnumerable<Cuenta>> ObtenerCuentas(string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.GetStringAsync(UrlBase);

            return JsonConvert.DeserializeObject<IEnumerable<Cuenta>>(response);
        }

        public async Task<Cuenta> Ingresar(Cuenta cuenta, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PostAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(cuenta),
                Encoding.UTF8,
                "application/json"));

            return JsonConvert.DeserializeObject<Cuenta>(await
                response.Content.ReadAsStringAsync());
        }

        public async Task<Cuenta> Actualizar(Cuenta cuenta, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PutAsync(UrlBase,
                new StringContent(JsonConvert.SerializeObject(cuenta),
                Encoding.UTF8,
                "application/json"));

            return JsonConvert.DeserializeObject<Cuenta>(await response.
                Content.ReadAsStringAsync());
        }

        public async Task<Cuenta> Eliminar(string id, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.DeleteAsync(string.Concat(UrlBase, id));

            return JsonConvert.DeserializeObject<Cuenta>(await
                response.Content.ReadAsStringAsync());
        }
    }
}