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
    public class UsuarioManager
    {
        string UrlLogin = "http://localhost:49220/api/login/authenticate/";
        string UrlRegister = "http://localhost:49220/api/usuarios/";

        public async Task<Usuario> Autenticar(LoginRequest loginRequest)
        {
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.PostAsync(UrlLogin,
                new StringContent(JsonConvert.SerializeObject(loginRequest),
                    Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Usuario>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Usuario> Registrar(Usuario usuario)
        {
            HttpClient httpClient = new HttpClient();

            var response = await httpClient.PostAsync(UrlRegister,
                new StringContent(JsonConvert.SerializeObject(usuario),
                Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Usuario>(await response.Content.ReadAsStringAsync());
        }
    }
}