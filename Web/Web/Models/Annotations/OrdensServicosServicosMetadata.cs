using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class OrdensServicosServicosMetadata
    {
        public int Id { get; set; }
        public int OrdensServicosId { get; set; }
        [Display(Name = "Descrição:")]
        [Required(ErrorMessage = "A descrição deve ser informada!")]
        public string Descricao { get; set; }
        [Display(Name = "Valor:")]
        [Required(ErrorMessage = "O valor deve ser informado!")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
        public decimal Valor { get; set; }
        public OrdensServicos OrdensServicos { get; set; }
    }
}