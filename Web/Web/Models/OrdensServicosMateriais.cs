//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrdensServicosMateriais
    {
        public int Id { get; set; }
        public int OrdensServicosId { get; set; }
        public string Descricao { get; set; }
        public float PrecoUnitario { get; set; }
        public int Quantidade { get; set; }
        public float PrecoTotal { get; set; }
    
        public virtual OrdensServicos OrdensServicos { get; set; }
    }
}
