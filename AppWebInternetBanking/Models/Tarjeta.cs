namespace AppWebInternetBanking.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tarjeta
    {
        public int Codigo { get; set; }
        public int CodigoUsuario { get; set; }
        public string Tipo { get; set; }
        public string Emisor { get; set; }
        public byte[] Categoria { get; set; }
        public int CodigoMoneda { get; set; }
        public string Estado { get; set; }
    
    }
}
