namespace AppWebInternetBanking.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sesion
    {
        public int Codigo { get; set; }
        public int CodigoUsuario { get; set; }
        public System.DateTime FechaHoraInicio { get; set; }
        public System.DateTime FechaHoraExpiracion { get; set; }
        public string Estado { get; set; }
   
    }
}
