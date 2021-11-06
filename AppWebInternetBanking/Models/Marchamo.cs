
namespace AppWebInternetBanking.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Marchamo
    {
        public int Codigo { get; set; }
        public string Placa { get; set; }
        public string Tipo { get; set; }
        public decimal Monto { get; set; }
        public int CodigoCuenta { get; set; }
        public string Estado { get; set; }
    
    }
}
