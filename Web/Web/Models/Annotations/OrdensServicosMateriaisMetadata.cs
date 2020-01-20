using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class OrdensServicosMateriaisMetadata
    {
        public int Id { get; set; }
        public int OrdensServicosId { get; set; }
        [Display(Name = "Descrição:")]
        [Required(ErrorMessage = "A descrição deve ser informada!")]
        public string Descricao { get; set; }
        [Display(Name = "Quantidade:")]
        [Required(ErrorMessage = "A quantidade deve ser informada!")]
        public int Quantidade { get; set; }
        [Display(Name = "Valor unitário:")]
        [Required(ErrorMessage = "O valor unitário deve ser informado!")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
        public decimal PrecoUnitario { get; set; }
        [Display(Name = "Valor total:")]
        [Required(ErrorMessage = "O valor total deve ser informado!")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
        public decimal PrecoTotal { get; set; }

        public OrdensServicos OrdensServicos { get; set; }
    }
}