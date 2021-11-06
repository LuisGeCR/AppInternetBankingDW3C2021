namespace AppWebInternetBanking.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pago
    {
        public int Codigo { get; set; }
        public int CodigoServicio { get; set; }
        public int CodigoCuenta { get; set; }
        public System.DateTime FechaHora { get; set; }
        public decimal Monto { get; set; }
   
    }
}
