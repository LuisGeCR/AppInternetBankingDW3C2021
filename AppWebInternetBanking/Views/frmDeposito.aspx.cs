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
    public partial class frmDeposito : System.Web.UI.Page
    {
        IEnumerable<Deposito> depositos = new ObservableCollection<Deposito>();
        DepositoManager depositoManager = new DepositoManager();
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
                depositos = await depositoManager.ObtenerDepositos(Session["Token"].ToString());
                gvDepositos.DataSource = depositos.ToList();
                gvDepositos.DataBind();
            }
            catch (Exception e)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de depositos" +  e.ToString() ;
                lblStatus.Visible = true;
            }
        }

        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
            {
                Deposito deposito = new Deposito()
                {
                    CodigoUsuario = Convert.ToInt32(txtCodigoUsuario.Text),
                    Monto = Convert.ToInt32(txtCodigoUsuario.Text),
                    CodigoMoneda = Convert.ToInt32(txtCodigoUsuario.Text),
                    Plazo = Convert.ToInt32(txtCodigoUsuario.Text),
                    Interes = Convert.ToDecimal(txtCodigoUsuario.Text),
                    Estado = ddlEstadoMant.SelectedValue
                };

                Deposito depositoIngresado = await depositoManager.Ingresar(deposito, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(depositoIngresado.CodigoUsuario.ToString()))
                {
                    lblResultado.Text = "Deposito ingresado con exito";
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
                Deposito deposito = new Deposito()
                {
                    CodigoUsuario = Convert.ToInt32(txtCodigoUsuario.Text),
                    Monto = Convert.ToInt32(txtCodigoUsuario.Text),
                    CodigoMoneda = Convert.ToInt32(txtCodigoUsuario.Text),
                    Plazo = Convert.ToInt32(txtCodigoUsuario.Text),
                    Interes = Convert.ToDecimal(txtCodigoUsuario.Text),
                    Estado = ddlEstadoMant.SelectedValue
                };

                Deposito depositoActualizado = await depositoManager.Actualizar(deposito, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(depositoActualizado.CodigoUsuario.ToString()))
                {
                    lblResultado.Text = "Deposito actualizado con exito";
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

                Deposito deposito = await depositoManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(deposito.CodigoUsuario.ToString()))
                {
                    ltrModalMensaje.Text = "Servicio eliminado";
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
            ltrTituloMantenimiento.Text = "Nuevo deposito";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;
            ltrCodigoMant.Visible = true;
            txtCodigoMant.Visible = true;
            ltrCodigoUsuario.Visible = true;
            txtCodigoUsuario.Visible = true;
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
            ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
        }

        protected void gvDepositos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvDepositos.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar servicio";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    txtCodigoUsuario.Text = row.Cells[1].Text.Trim();
                    txtMonto.Text = row.Cells[2].Text.Trim();
                    txtCodigoMoneda.Text = row.Cells[3].Text.Trim();
                    txtPlazo.Text = row.Cells[4].Text.Trim();
                    txtInteres.Text = row.Cells[5].Text.Trim();
                    btnAceptarMant.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "Esta seguro que desea eliminar el deposito?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }
    }
}