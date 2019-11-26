using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class AbrirOrdensServicosViewModel
    {
        [Required(ErrorMessage = "O cliente deve ser informado!")]
        [Display(Name = "Cliente:")]
        public int ClienteId { get; set; }
        [Display(Name = "Veículos:")]
        public int VeiculosId { get; set; }
        [Required(ErrorMessage = "A data prevista para o início do trabalho deve ser informada!")]
        [Display(Name = "Data inícial prevista:")]
        [DataType(DataType.Date)]
        public System.DateTime DataInicialPrevista { get; set; }
    }
}