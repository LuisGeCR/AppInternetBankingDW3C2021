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
    public partial class frmCredito : System.Web.UI.Page
    {
        IEnumerable<Credito> creditos = new ObservableCollection<Credito>();
        CreditoManager creditoManager = new CreditoManager();
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
                creditos = await creditoManager.ObtenerCreditos(Session["Token"].ToString());
                gvCreditos.DataSource = creditos.ToList();
                gvCreditos.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de creditos";
                lblStatus.Visible = true;
            }
        }

        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
            {
                Credito credito = new Credito()
                {
                    Descripcion = txtDescripcion.Text,
                    Estado = ddlEstadoMant.SelectedValue
                };

                Credito creditoIngresado = await creditoManager.Ingresar(credito, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(creditoIngresado.Descripcion))
                {
                    lblResultado.Text = "Servicio ingresado con exito";
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
            else // modificar
            {
                Credito credito = new Credito()
                {
                    Codigo = Convert.ToInt32(txtCodigoMant.Text),
                    Monto = Convert.ToDecimal(txtCodigoUsuario.Text),
                    CodigoMoneda = Convert.ToInt32(txtCodigoUsuario.Text),
                    Plazo = Convert.ToInt32(txtCodigoUsuario.Text),
                    Interes = Convert.ToDecimal(txtCodigoUsuario.Text),
                    Descripcion = txtDescripcion.Text,
                    Estado = ddlEstadoMant.SelectedValue
                };

                Credito creditoActualizado = await creditoManager.Actualizar(credito, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(creditoActualizado.Descripcion))
                {
                    lblResultado.Text = "credito actualizado con exito";
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

                Credito credito = await creditoManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(credito.Descripcion))
                {
                    ltrModalMensaje.Text = "credito eliminado";
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
                    Vista = "frmServicio.aspx",
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
            ltrCodigoUsuario.Visible = true;
            txtCodigoUsuario.Visible = true;
            txtDescripcion.Visible = true;
            ltrDescripcion.Visible = true;
            ltrMonto.Visible = true;
            txtMonto.Visible = true;
            ltrCodigoMoneda.Visible = true;
            txtCodigoMoneda.Visible = true;
            ltrPlazo.Visible = true;
            txtPlazo.Visible = true;
            ltrInteres.Visible = true;
            txtInteres.Visible = true;
            ddlEstadoMant.Enabled = false;
            txtCodigoMant.Text = string.Empty;
            txtCodigoUsuario.Text = string.Empty;
            ltrMonto.Text = string.Empty;
            ltrCodigoMoneda.Text = string.Empty;
            ltrPlazo.Text = string.Empty;
            ltrInteres.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
        }

        protected void gvCreditos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvCreditos.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar servicio";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    txtCodigoUsuario.Text = row.Cells[1].Text.Trim();
                    txtDescripcion.Text = row.Cells[2].Text.Trim();
                    txtMonto.Text = row.Cells[3].Text.Trim();
                    txtCodigoMoneda.Text = row.Cells[4].Text.Trim();
                    txtPlazo.Text = row.Cells[5].Text.Trim();
                    txtInteres.Text = row.Cells[6].Text.Trim();
                    btnAceptarMant.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "Esta seguro que desea eliminar el credito?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }
    }
}