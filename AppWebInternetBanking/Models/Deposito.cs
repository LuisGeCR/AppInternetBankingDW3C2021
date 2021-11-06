
namespace AppWebInternetBanking.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Deposito
    {
        public int Codigo { get; set; }
        public int CodigoUsuario { get; set; }
        public decimal Monto { get; set; }
        public int CodigoMoneda { get; set; }
        public int Plazo { get; set; }
        public decimal Interes { get; set; }
        public string Estado { get; set; }
    
    }
}
