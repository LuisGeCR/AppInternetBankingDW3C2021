//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace API.Models
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
    
        public virtual Moneda Moneda { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
