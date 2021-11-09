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
    public partial class frmTransferencia : System.Web.UI.Page
    {
        IEnumerable<Transferencia> transferencias = new ObservableCollection<Transferencia>();
        TransferenciaManager transferenciaManager = new TransferenciaManager();
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
                transferencias = await transferenciaManager.ObtenerTransferencias(Session["Token"].ToString());
                gvTransferencias.DataSource = transferencias.ToList();
                gvTransferencias.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de transferencias";
                lblStatus.Visible = true;
            }
        }

        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
            {
                Transferencia transferencia = new Transferencia()
                {
                    CuentaOrigen = Convert.ToInt32(txtCuentaOrigen.Text),
                    CuentaDestino = Convert.ToInt32(txtCuentaDestino.Text),
                    FechaHora = Convert.ToDateTime(txtFechaHora.Text),
                    Descripcion = txtDescripcion.Text,
                    Monto = Convert.ToDecimal(txtMonto.Text),
                    Estado = ddlEstadoMant.SelectedValue
                };

                Transferencia transferenciaIngresado = await transferenciaManager.Ingresar(transferencia, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(transferenciaIngresado.Descripcion))
                {
                    lblResultado.Text = "transferencia ingresada con exito";
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
                Transferencia transferencia = new Transferencia()
                {
                    Codigo = Convert.ToInt32(txtCodigoMant.Text),
                    CuentaOrigen = Convert.ToInt32(txtCuentaOrigen.Text),
                    CuentaDestino = Convert.ToInt32(txtCuentaDestino.Text),
                    FechaHora = Convert.ToDateTime(txtFechaHora.Text),
                    Descripcion = txtDescripcion.Text,
                    Monto = Convert.ToDecimal(txtMonto.Text),
                    Estado = ddlEstadoMant.SelectedValue
                };

                Transferencia transferenciaActualizado = await transferenciaManager.Actualizar(transferencia, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(transferenciaActualizado.Descripcion))
                {
                    lblResultado.Text = "Servicio actualizado con exito";
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

                Transferencia transferencia = await transferenciaManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(transferencia.Descripcion))
                {
                    ltrModalMensaje.Text = "transferencia eliminada";
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
            ltrTituloMantenimiento.Text = "Nueva transferencia";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;
            ltrCodigoMant.Visible = true;
            txtCodigoMant.Visible = true;
            txtCuentaOrigen.Visible = true;
            ltrCuentaOrigen.Visible = true;
            txtCuentaDestino.Visible = true;
            ltrCuentaDestino.Visible = true;
            txtFechaHora.Visible = true;
            ltrFechaHora.Visible = true;
            txtDescripcion.Visible = true;
            ltrDescripcion.Visible = true;
            txtMonto.Visible = true;
            ltrMonto.Visible = true;
            ddlEstadoMant.Enabled = false;
            txtCodigoMant.Text = string.Empty;
            txtCuentaOrigen.Text = string.Empty;
            txtCuentaDestino.Text = string.Empty;
            txtFechaHora.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            txtMonto.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
        }

        protected void gvTransferencias_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvTransferencias.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar servicio";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    txtCuentaOrigen.Text = row.Cells[1].Text.Trim();
                    txtCuentaDestino.Text = row.Cells[2].Text.Trim();
                    txtFechaHora.Text = row.Cells[3].Text.Trim();
                    txtDescripcion.Text = row.Cells[4].Text.Trim();
                    txtMonto.Text = row.Cells[5].Text.Trim();
                    btnAceptarMant.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "Esta seguro que desea eliminar el servicio?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }
    }
}