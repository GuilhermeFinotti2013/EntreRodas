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
        [Required(ErrorMessage = "O veículo deve ser informado!")]
        [Display(Name = "Veículos:")]
        public int VeiculoId { get; set; }
    }
}