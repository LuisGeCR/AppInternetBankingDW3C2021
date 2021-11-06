using AppWebInternetBanking.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace AppWebInternetBanking.Models
{
    public class Correo
    {
        public async void Enviar(string asunto, string mensaje, string destinatario, int codigoUsuario)
        {
            try
            {
                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("ibulacit@gmail.com", "Ulacit123456"),
                    EnableSsl = true
                };

                string htmlBody = "<!DOCTYPE html>" +
                                    "<head><meta charset=\"utf-8\"><meta name = \"viewport\" content = \"width=device-width, initial-scale=1\" >" +
                                    "<link rel=\"stylesheet\" href = \"https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css\">" +
                                    "<script src=\"https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js\" ></script>" +
                                    "<script src=\"https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js\"></script>" +
                                    "</head>" +
                                    "<body>" +
                                        "<div class=\"container\">" +
                                    "<div class=\"jumbotron\">" +
                                    "<h1>Internet Banking ULACIT</h1>" +
                                    "<p>Inclusion de nuevo servicio</p>" +
                                    "</div>" +
                                    "<p>Se incluyo el servicio " + mensaje + ".</p>" +
                                "</div>" +
                                "</body>" +
                                "</html>";

                MailMessage message = new MailMessage();
                message.From = new MailAddress("ibulacit@gmail.com");
                message.To.Add(destinatario);
                message.Subject = asunto;
                message.IsBodyHtml = true;
                message.Body = htmlBody;

                client.Send(message);
            }
            catch (Exception ex)
            {
                ErrorManager errorManager = new ErrorManager();
                Error error = new Error()
                {
                    CodigoUsuario = codigoUsuario,
                    FechaHora = DateTime.Now,
                    Vista = "Correo",
                    Accion = "Enviar",
                    Fuente = ex.Source,
                    Numero = ex.HResult,
                    Descripcion = ex.Message
                };

                Error errorIngresado = await errorManager.Ingresar(error);
            }
        }
    }
}