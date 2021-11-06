
namespace AppWebInternetBanking.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cuenta
    {

        public int Codigo { get; set; }
        public int CodigoUsuario { get; set; }
        public int CodigoMoneda { get; set; }
        public string Descripcion { get; set; }
        public string IBAN { get; set; }
        public decimal Saldo { get; set; }
        public string Estado { get; set; }
    
    }
}
