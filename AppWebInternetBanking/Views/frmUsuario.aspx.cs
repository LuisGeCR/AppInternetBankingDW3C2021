using AppWebInternetBanking.Controllers;
using AppWebInternetBanking.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppWebInternetBanking.Views
{
    public partial class frmUsuario : System.Web.UI.Page
    {
        IEnumerable<Usuario> usuarios = new ObservableCollection<Usuario>();
        UsuarioManager usuarioManager = new UsuarioManager();
        static string _codigo = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["CodigoUsuario"] == null)
                    Response.Redirect("~/Login.aspx");
                else
                    InicializarControles();
            }

        }
        private async void InicializarControles()
        {
            try
            {
                usuarios = await usuarioManager.ObtenerUsuarios(Session["Token"].ToString());
                gvUsuarios.DataSource = usuarios.ToList();
                gvUsuarios.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de usuarios";
                lblStatus.Visible = true;
            }
        }

        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
            {
                Usuario usuario = new Usuario()
                {
                    Identificacion = txtIdentificacion.Text,
                    Nombre = txtNombre.Text,
                    Username = txtUsername.Text,
                    Password = txtPassword.Text,
                    Email = txtEmail.Text,
                    FechaNacimiento = Convert.ToDateTime(txtFechaNacimiento.Text),
                    Estado = ddlEstadoMant.SelectedValue
                };

                Usuario usuarioIngresado = await usuarioManager.Ingresar(usuario, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(usuarioIngresado.Identificacion))
                {
                    lblResultado.Text = "Usuario ingresado con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    Correo correo = new Correo();
                    correo.Enviar("Nuevo usuario incluido", usuarioIngresado.Identificacion, "svillagra07@gmail.com",
                        Convert.ToInt32(Session["CodigoUsuario"].ToString()));

                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                }
                else
                {
                    lblResultado.Text = "Hubo un error al efectuar la operacion";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                }
            }
            else // modificar
            {
                Usuario usuario = new Usuario()
                {
                    Codigo = Convert.ToInt32(txtCodigoMant.Text),
                    Identificacion = txtIdentificacion.Text,
                    Nombre = txtNombre.Text,
                    Username = txtUsername.Text,
                    Password = txtPassword.Text,
                    Email = txtEmail.Text,
                    FechaNacimiento = Convert.ToDateTime(txtFechaNacimiento.Text),
                    Estado = ddlEstadoMant.SelectedValue
                };

                Usuario usuarioActualizado = await usuarioManager.Actualizar(usuario, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(usuarioActualizado.Identificacion))
                {
                    lblResultado.Text = "Usuario actualizado con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                }
                else
                {
                    lblResultado.Text = "Hubo un error al efectuar la operacion";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                }
            }
        }

        protected void btnCancelarMant_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseMantenimiento(); });", true);
        }

        protected async void btnAceptarModal_Click(object sender, EventArgs e)
        {
            try
            {

                Usuario usuario = await usuarioManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(usuario.Identificacion))
                {
                    ltrModalMensaje.Text = "Usuario eliminado";
                    btnAceptarModal.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { openModal(); });", true);
                    InicializarControles();
                }
            }
            catch (Exception ex)
            {
                ErrorManager errorManager = new ErrorManager();
                Error error = new Error()
                {
                    CodigoUsuario =
                    Convert.ToInt32(Session["CodigoUsuario"].ToString()),
                    FechaHora = DateTime.Now,
                    Vista = "frmUsuario.aspx",
                    Accion = "btnAceptarModal_Click",
                    Fuente = ex.Source,
                    Numero = ex.HResult,
                    Descripcion = ex.Message
                };
                Error errorIngresado = await errorManager.Ingresar(error);
            }
        }

        protected void btnCancelarModal_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseModal(); });", true);
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            ltrTituloMantenimiento.Text = "Nuevo ";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;
            ltrCodigoMant.Visible = true;
            txtCodigoMant.Visible = true;
            ltrIdentificacion.Visible = true;
            txtIdentificacion.Visible = true;
            ltrNombre.Visible = true;
            txtNombre.Visible = true;
            ltrUsername.Visible = true;
            txtUsername.Visible = true;
            ltrPassword.Visible = true;
            txtPassword.Visible = true;
            ltrEmail.Visible = true;
            txtEmail.Visible = true;
            ltrFechaNacimiento.Visible = true;
            txtFechaNacimiento.Visible = true;

            ddlEstadoMant.Enabled = true;
            txtIdentificacion.Text = string.Empty;
            txtCodigoMant.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtUsername.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtFechaNacimiento.Text = string.Empty;

            lblResultado.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);

        }

        protected void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvUsuarios.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar usuario";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    txtIdentificacion.Text = row.Cells[1].Text.Trim();
                    txtNombre.Text = row.Cells[2].Text.Trim();
                    txtUsername.Text = row.Cells[3].Text.Trim();
                    txtPassword.Text = row.Cells[4].Text.Trim();
                    txtEmail.Text = row.Cells[5].Text.Trim();
                    txtFechaNacimiento.Text = row.Cells[6].Text.Trim();
                    btnAceptarMant.Visible = true;
                    lblResultado.Text = string.Empty;
                    ddlEstadoMant.Enabled = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "Esta seguro que desea eliminar el usuario?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }


    }
}