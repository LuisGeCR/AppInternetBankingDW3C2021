using AppWebInternetBanking.Controllers;
using AppWebInternetBanking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppWebInternetBanking.CustomErrors
{
    public partial class frmError : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            Exception err = Session["LastError"] as Exception;

            if (err != null)
            {
                err = err.GetBaseException();
                lblError.Text = err.Message;
                Session["LastError"] = null;
              
                ErrorManager errorManager = new ErrorManager();
                Error errorApi = new Error()
                {
                    CodigoUsuario = 0,
                    FechaHora = DateTime.Now,
                    Vista = "frmError.aspx",
                    Accion = "Page_load",
                    Fuente = err.Source,
                    Numero = err.HResult,
                    Descripcion = err.Message
                };

                Error errorIngresado = await errorManager.Ingresar(errorApi);
            }
        }
    }
}