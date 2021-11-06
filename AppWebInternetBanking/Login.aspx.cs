using AppWebInternetBanking.Controllers;
using AppWebInternetBanking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppWebInternetBanking
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected async void btnAceptar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    LoginRequest loginRequest = new LoginRequest() { Username = txtUsername.Text, Password = txtPassword.Text };
                    UsuarioManager usuarioManager = new UsuarioManager();
                    Usuario usuario = new Usuario();

                    usuario = await usuarioManager.Autenticar(loginRequest);

                    if (usuario != null)
                    {
                        Session["CodigoUsuario"] = usuario.Codigo;
                        Session["Identificacion"] = usuario.Identificacion;
                        Session["Nombre"] = usuario.Nombre;
                        Session["Estado"] = usuario.Estado;
                        Session["Token"] = usuario.Token;

                        FormsAuthentication.RedirectFromLoginPage(usuario.Username, false);
                    }
                    else
                    {
                        lblStatus.Text = "Credenciales invalidas";
                        lblStatus.Visible = true;
                    }
                }
                catch (Exception)
                {
                    lblStatus.Text = "Hubo un error, contacte al administrador del sistema";
                    lblStatus.Visible = true;
                }
            }
        }
    }
}