namespace AppWebInternetBanking.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Estadistica
    {
        public int Codigo { get; set; }
        public int CodigoUsuario { get; set; }
        public System.DateTime FechaHora { get; set; }
        public string PlataformaDispositivo { get; set; }
        public string Navegador { get; set; }
        public string Vista { get; set; }
        public string Accion { get; set; }
    
    }
}
