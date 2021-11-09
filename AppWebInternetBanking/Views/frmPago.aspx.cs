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
    public partial class frmPago : System.Web.UI.Page
    {
        IEnumerable<Pago> pagos = new ObservableCollection<Pago>();
        PagoManager pagoManager = new PagoManager();
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
                    pagos = await pagoManager.ObtenerPagos(Session["Token"].ToString());
                    gvPagos.DataSource = pagos.ToList();
                    gvPagos.DataBind();
                }
                catch (Exception)
                {
                    lblStatus.Text = "Hubo un error al cargar la lista de pagos";
                    lblStatus.Visible = true;
                }
            }

            protected async void btnAceptarMant_Click(object sender, EventArgs e)
            {
                if (string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
                {
                    Pago pago = new Pago()
                    {
                        CodigoServicio =Convert.ToInt32(txtCodigoServicio.Text),
                        CodigoCuenta = Convert.ToInt32(txtCodigoCuenta.Text),
                        FechaHora = Convert.ToDateTime(txtFechaHora.Text),
                        Monto = Convert.ToDecimal(txtMonto.Text)

                    };

                    Pago pagoIngresado = await pagoManager.Ingresar(pago, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(pagoIngresado.CodigoServicio.ToString()))
                    {
                        lblResultado.Text = "Pago ingresado con exito";
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
                    Pago pago = new Pago()
                    {
                        Codigo = Convert.ToInt32(txtCodigoMant.Text),
                        CodigoServicio = Convert.ToInt32(txtCodigoServicio.Text),
                        CodigoCuenta = Convert.ToInt32(txtCodigoCuenta.Text),
                        FechaHora = Convert.ToDateTime(txtFechaHora.Text),
                        Monto = Convert.ToDecimal(txtMonto.Text)
                    };

                    Pago pagoActualizado = await pagoManager.Actualizar(pago, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(pagoActualizado.CodigoServicio.ToString()))
                    {
                        lblResultado.Text = "Pago actualizado con exito";
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

                    Pago pago = await pagoManager.Eliminar(_codigo, Session["Token"].ToString());
                    if (!string.IsNullOrEmpty(pago.CodigoServicio.ToString()))
                    {
                        ltrModalMensaje.Text = "Pago eliminado";
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
                        Vista = "frmPago.aspx",
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
               ltrCodigoServicio.Visible = true;
               txtCodigoServicio.Visible =  true;
               ltrCodigoCuenta.Visible = true;
               txtCodigoCuenta.Visible = true;
               ltrFechaHora.Visible = true;
               txtFechaHora.Visible = true;
               ltrMonto.Visible = true;
               txtMonto.Visible = true;


            txtCodigoMant.Text = string.Empty;
            txtCodigoServicio.Text = string.Empty;
            txtCodigoCuenta.Text = string.Empty;
            txtFechaHora.Text = string.Empty;
            txtMonto.Text = string.Empty;

            lblResultado.Text = string.Empty;
                ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);

            }

            protected void gvPagos_RowCommand(object sender, GridViewCommandEventArgs e)
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvPagos.Rows[index];

                switch (e.CommandName)
                {
                    case "Modificar":
                        ltrTituloMantenimiento.Text = "Modificar pago";
                        btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                        txtCodigoMant.Text = row.Cells[0].Text.Trim();
                        txtCodigoServicio.Text = row.Cells[1].Text.Trim();
                        txtCodigoCuenta.Text = row.Cells[2].Text.Trim();
                        txtFechaHora.Text = row.Cells[3].Text.Trim();
                        txtMonto.Text = row.Cells[4].Text.Trim();
                    btnAceptarMant.Visible = true;
                        lblResultado.Text = string.Empty;
                        ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                        break;
                    case "Eliminar":
                        _codigo = row.Cells[0].Text.Trim();
                        ltrModalMensaje.Text = "Esta seguro que desea eliminar el pago?";
                        ScriptManager.RegisterStartupScript(this,
                   this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                        break;
                    default:
                        break;
                }
            }
        }
    }
