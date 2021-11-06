using AppWebInternetBanking.Controllers;
using AppWebInternetBanking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppWebInternetBanking
{
    public partial class Registro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        async protected void btnAceptar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    UsuarioManager usuarioManager = new UsuarioManager();

                    Usuario usuario = new Usuario()
                    {
                        Identificacion = txtIdentificacion.Text,
                        Nombre = txtNombre.Text,
                        Email = txtEmail.Text,
                        FechaNacimiento = Convert.ToDateTime(txtFechaNacimiento.Text),
                        Username = txtUsername.Text,
                        Password = txtPassword.Text,
                        Estado = "A"
                    };

                    Usuario usuarioRegistrado = await usuarioManager.Registrar(usuario);

                    if (!string.IsNullOrEmpty(usuario.Identificacion))
                        Response.Redirect("Login.aspx");
                    else
                    {
                        lblStatus.Text = "Hubo un error al registrar el usuario.";
                        lblStatus.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    ErrorManager errorManager = new ErrorManager();
                    Error error = new Error()
                    {
                        CodigoUsuario = 0,
                        FechaHora = DateTime.Now,
                        Vista = "Registro.aspx",
                        Accion = "btnAceptar_Click",
                        Fuente = ex.Source,
                        Numero = ex.HResult,
                        Descripcion = ex.Message
                    };
                    Error errorIngresado = await errorManager.Ingresar(error);
                    lblStatus.Text = "Hubo un error al registrar el usuario.";
                    lblStatus.Visible = true;
                }
            }
        }

        protected void cldFechaNacimiento_SelectionChanged(object sender, EventArgs e)
        {
            txtFechaNacimiento.Text = cldFechaNacimiento.SelectedDate.ToString("dd/MM/yyyy");
            cldFechaNacimiento.Visible = false;
        }

        protected void btnFechaNac_Click(object sender, EventArgs e)
        {
            cldFechaNacimiento.Visible = true;
        }
    }
}