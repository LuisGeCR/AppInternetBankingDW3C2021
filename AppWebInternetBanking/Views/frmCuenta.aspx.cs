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
    public partial class frmCuenta : System.Web.UI.Page
    {
        IEnumerable<Cuenta> cuentas = new ObservableCollection<Cuenta>();
        CuentaManager cuentaManager = new CuentaManager();
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
                cuentas = await cuentaManager.ObtenerCuentas(Session["Token"].ToString());
                gvCuentas.DataSource = cuentas.ToList();
                gvCuentas.DataBind();
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

                    Cuenta cuenta = new Cuenta()
                    {
                        CodigoUsuario = Convert.ToInt32(txtUsuario.Text),
                        CodigoMoneda = Convert.ToInt32(txtMoneda.Text),
                        Descripcion = txtDescripcion.Text,
                        IBAN = System.Text.Encoding.UTF8.GetBytes(txtIBAN.Text),
                        Saldo = 0,
                        Estado = ddlEstadoMant.SelectedValue
                    };

                    Cuenta cuentaIngresada = await cuentaManager.Ingresar(cuenta, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(cuentaIngresada.Descripcion))
                    {
                        lblResultado.Text = "Cuenta ingresada con exito";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Green;
                        btnAceptarMant.Visible = false;
                        InicializarControles();

                        Correo correo = new Correo();
                        correo.Enviar("Nueva cuenta incluida", cuentaIngresada.Descripcion, "lhidalgo22@gmail.com",
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
                    Cuenta cuenta = new Cuenta()
                    {
                        Codigo = Convert.ToInt32(txtCodigoMant.Text),
                        CodigoUsuario = Convert.ToInt32(txtUsuario.Text),
                        CodigoMoneda = Convert.ToInt32(txtMoneda.Text),
                        Descripcion = txtDescripcion.Text,
                        IBAN = System.Text.Encoding.UTF8.GetBytes(txtIBAN.Text),
                        Saldo = Convert.ToDecimal(txtSaldo.Text),
                        Estado = ddlEstadoMant.SelectedValue
                    };

                    Cuenta cuentaActualizada = await cuentaManager.Actualizar(cuenta, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(cuentaActualizada.Descripcion))
                    {
                        lblResultado.Text = "La cuenta se ha actualizado con exito";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Green;
                        btnAceptarMant.Visible = false;
                        InicializarControles();

                        Correo correo = new Correo();
                        correo.Enviar("Cuenta actualizada con exito", cuentaActualizada.Descripcion, "lhidalgo22@gmail.com",
                            Convert.ToInt32(Session["CodigoUsuario"].ToString()));

                        ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    }
                    else
                    {
                        lblResultado.Text = "Hubo un error la actualizacion";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Maroon;
                    }
                }
            }
            catch(Exception)
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

                Cuenta cuenta = await cuentaManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(cuenta.Descripcion))
                {
                    ltrModalMensaje.Text = "Cuenta eliminada";
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
                    Vista = "frmCuenta.aspx",
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
            ltrTituloMantenimiento.Text = "Nueva Cuenta";
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

        protected async void gvCuentas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvCuentas.Rows[index];

            Cuenta cuentaExistente = await cuentaManager.ObtenerCuenta(Session["Token"].ToString(), row.Cells[0].Text.Trim());
            string iban = System.Text.Encoding.UTF8.GetString(cuentaExistente.IBAN);

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar cuenta";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    txtUsuario.Text = row.Cells[1].Text.Trim();
                    txtMoneda.Text = row.Cells[2].Text.Trim();
                    txtDescripcion.Text = row.Cells[3].Text.Trim();
                    txtIBAN.Text = iban.Trim();
                    txtSaldo.Text = row.Cells[4].Text.Trim();
                    btnAceptarMant.Visible = true;
                    lblResultado.Text = string.Empty;
                    ddlEstadoMant.Enabled = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "Esta seguro que desea eliminar la cuenta?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }
    }
}