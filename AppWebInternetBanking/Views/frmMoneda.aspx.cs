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
    public partial class frmMoneda : System.Web.UI.Page
    {
        IEnumerable<Moneda> monedas = new ObservableCollection<Moneda>();
        MonedaManager monedaManager = new MonedaManager();
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
                monedas = await monedaManager.ObtenerMonedas(Session["Token"].ToString());
                gvServicios.DataSource = monedas.ToList();
                gvServicios.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de monedas";
                lblStatus.Visible = true;
            }
        }

        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
            {
                Moneda moneda = new Moneda()
                {
                    Descripcion = txtDescripcion.Text,
                    Estado = ddlEstadoMant.SelectedValue
                };

                Moneda monedaIngresado = await monedaManager.Ingresar(moneda, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(monedaIngresado.Descripcion))
                {
                    lblResultado.Text = "Moneda ingresada con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    Correo correo = new Correo();
                    correo.Enviar("Nueva moneda incluido", monedaIngresado.Descripcion, "lhidalgo22@gmail.com",
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
                Moneda moneda = new Moneda()
                {
                    Codigo = Convert.ToInt32(txtCodigoMant.Text),
                    Descripcion = txtDescripcion.Text,
                    Estado = ddlEstadoMant.SelectedValue
                };

                Moneda monedaActualizado = await monedaManager.Actualizar(moneda, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(monedaActualizado.Descripcion))
                {
                    lblResultado.Text = "Moneda actualizada con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    Correo correo = new Correo();
                    correo.Enviar("Moneda actualizada", monedaActualizado.Descripcion, "lhidalgo22@gmail.com",
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
        }

        protected void btnCancelarMant_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseMantenimiento(); });", true);
        }

        protected async void btnAceptarModal_Click(object sender, EventArgs e)
        {
            try
            {

                Moneda moneda = await monedaManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(moneda.Descripcion))
                {
                    ltrModalMensaje.Text = "Moneda eliminada";
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
                    Vista = "frmMoneda.aspx",
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
            ltrTituloMantenimiento.Text = "Nueva Moneda";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;
            ltrCodigoMant.Visible = true;
            txtCodigoMant.Visible = true;
            txtDescripcion.Visible = true;
            ltrDescripcion.Visible = true;
            ddlEstadoMant.Enabled = true;
            txtCodigoMant.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            lblResultado.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);

        }

        protected void gvServicios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvServicios.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar Moneda";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    txtDescripcion.Text = row.Cells[1].Text.Trim();
                    btnAceptarMant.Visible = true;
                    lblResultado.Text = string.Empty;
                    ddlEstadoMant.Enabled = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "Esta seguro que desea eliminar la moneda?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }


    }
}
