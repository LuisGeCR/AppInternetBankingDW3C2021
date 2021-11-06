using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Models;
namespace API.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {

        [HttpPost]
        [Route("authenticate")]
        public IHttpActionResult Authenticate(LoginRequest loginRequest)
        {
            if (loginRequest == null)
                return BadRequest();

            Usuario usuario = new Usuario();

            try
            {
                using (SqlConnection sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["INTERNET_BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT Codigo, Identificacion, Nombre,
                                                             Username, Password, Email, FechaNacimiento, Estado
                                                            FROM Usuario Where Username = @Username and Password = @Password ", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@Username", loginRequest.Username);
                    sqlCommand.Parameters.AddWithValue("@Password", loginRequest.Password);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        usuario.Codigo = sqlDataReader.GetInt32(0);
                        usuario.Identificacion = sqlDataReader.GetString(1);
                        usuario.Nombre = sqlDataReader.GetString(2);
                        usuario.Username = sqlDataReader.GetString(3);
                        usuario.Password = sqlDataReader.GetString(4);
                        usuario.Email = sqlDataReader.GetString(5);
                        usuario.FechaNacimiento = sqlDataReader.GetDateTime(6);
                        usuario.Estado = sqlDataReader.GetString(7);

                        var token =
                            TokenGenerator.GenerateTokenJwt(usuario.Identificacion);
                        usuario.Token = token;
                    }

                    sqlConnection.Close();

                    if (!string.IsNullOrEmpty(usuario.Token))
                        return Ok(usuario);
                    else
                        return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

    }
  }