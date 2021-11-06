namespace AppWebInternetBanking.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Usuario
    {
       
        public int Codigo { get; set; }
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public System.DateTime FechaNacimiento { get; set; }
        public string Estado { get; set; }
    
    }
}
