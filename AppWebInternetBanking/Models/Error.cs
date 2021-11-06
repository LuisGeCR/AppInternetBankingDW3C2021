
namespace AppWebInternetBanking.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Error
    {
        public int Codigo { get; set; }
        public int CodigoUsuario { get; set; }
        public System.DateTime FechaHora { get; set; }
        public string Fuente { get; set; }
        public int Numero { get; set; }
        public string Descripcion { get; set; }
        public string Vista { get; set; }
        public string Accion { get; set; }
    
    }
}
