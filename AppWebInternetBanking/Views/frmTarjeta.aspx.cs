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
    public partial class frmTarjeta : System.Web.UI.Page
    {
        IEnumerable<Tarjeta> tarjetas = new ObservableCollection<Tarjeta>();
        TarjetaManager tarjetaManager = new TarjetaManager();
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
                tarjetas = await tarjetaManager.ObtenerTarjetas(Session["Token"].ToString());
                gvTarjetas.DataSource = tarjetas.ToList();
                gvTarjetas.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de servicios";
                lblStatus.Visible = true;
            }
        }

        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario usuarioExistente = await usuarioManager.ObtenerUsuario(Session["Token"].ToString(), txtUsuario.Text);

                if (string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
                {

                    Tarjeta tarjeta = new Tarjeta()
                    {
                        CodigoUsuario = Convert.ToInt32(txtUsuario.Text),
                        Tipo = ddlTipoMant.Text,
                        Emisor = ddlEmisorMant.Text,
                        Categoria = System.Text.Encoding.UTF8.GetBytes(ddlCategoriaMant.Text),
                        CodigoMoneda = Convert.ToInt32(txtMoneda.Text),
                        Estado = ddlEstadoMant.SelectedValue
                    };

                    Tarjeta tarjetaIngresada = await tarjetaManager.Ingresar(tarjeta, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(tarjetaIngresada.Tipo))
                    {
                        lblResultado.Text = "Solicitud de Tarjeta ingresada con exito";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Green;
                        btnAceptarMant.Visible = false;
                        InicializarControles();

                        Correo correo = new Correo();
                        correo.Enviar("Nueva solicitud de tarjeta incluida", tarjetaIngresada.Tipo, "lhidalgo22@gmail.com",
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
                    Tarjeta tarjeta = new Tarjeta()
                    {
                        Codigo = Convert.ToInt32(txtCodigoMant.Text),
                        CodigoUsuario = Convert.ToInt32(txtUsuario.Text),
                        Tipo = ddlTipoMant.Text,
                        Emisor = ddlEmisorMant.Text,
                        Categoria = System.Text.Encoding.UTF8.GetBytes(ddlCategoriaMant.Text),
                        CodigoMoneda = Convert.ToInt32(txtMoneda.Text),
                        Estado = ddlEstadoMant.SelectedValue
                    };

                    Tarjeta tarjetaActualizada = await tarjetaManager.Actualizar(tarjeta, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(tarjetaActualizada.Tipo))
                    {
                        lblResultado.Text = "La solicitud de tarjeta se ha actualizado con exito";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Green;
                        btnAceptarMant.Visible = false;
                        InicializarControles();

                        Correo correo = new Correo();
                        correo.Enviar("Solicitud de tarjeta actualizada con exito", tarjetaActualizada.Tipo, "lhidalgo22@gmail.com",
                            Convert.ToInt32(Session["CodigoUsuario"].ToString()));

                        ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    }
                    else
                    {
                        lblResultado.Text = "Hubo un error en la actualizacion";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Maroon;
                    }
                }
            }
            catch (Exception)
            {
                lblResultado.Text = "El usuario no existe";
                lblResultado.Visible = true;
                lblResultado.ForeColor = Color.Maroon;

                ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
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

                Tarjeta tarjeta = await tarjetaManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(tarjeta.Emisor))
                {
                    ltrModalMensaje.Text = "Solicitud eliminada";
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
                    CodigoUsuario = Convert.ToInt32(Session["CodigoUsuario"].ToString()),
                    FechaHora = DateTime.Now,
                    Vista = "frmTarjeta.aspx",
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
            ltrTituloMantenimiento.Text = "Nuevo servicio";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;
            ltrCodigoMant.Visible = true;
            txtCodigoMant.Visible = true;
            ltrUsuario.Visible = true;
            txtUsuario.Visible = true;
            ddlTipoMant.Enabled = true;
            ddlEmisorMant.Enabled = true;
            ddlCategoriaMant.Enabled = true;
            ltrMoneda.Visible = true;
            txtMoneda.Visible = true;
            ddlEstadoMant.Enabled = true;
            txtCodigoMant.Text = string.Empty;
            txtUsuario.Text = string.Empty;
            txtMoneda.Text = string.Empty;
            lblResultado.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
        }

        protected async void gvTarjetas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvTarjetas.Rows[index];

            Tarjeta tarjetaExistente = await tarjetaManager.ObtenerTarjeta(row.Cells[0].Text.Trim(), Session["Token"].ToString());
            string categoria = System.Text.Encoding.UTF8.GetString(tarjetaExistente.Categoria);

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar servicio";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    txtUsuario.Text = row.Cells[1].Text.Trim();
                    ddlTipoMant.Text = row.Cells[2].Text.Trim();
                    ddlEmisorMant.Text = row.Cells[3].Text.Trim();
                    ddlCategoriaMant.Text = categoria.Trim();
                    txtMoneda.Text = row.Cells[4].Text.Trim();
                    btnAceptarMant.Visible = true;
                    lblResultado.Text = string.Empty;
                    ddlEstadoMant.Enabled = true;
                    ScriptManager.RegisterStartupScript(this,this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "Esta seguro que desea eliminar la solicitud?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }

    }
}