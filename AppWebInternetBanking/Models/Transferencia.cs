namespace AppWebInternetBanking.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transferencia
    {
        public int Codigo { get; set; }
        public int CuentaOrigen { get; set; }
        public int CuentaDestino { get; set; }
        public System.DateTime FechaHora { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; }
   
    }
}
