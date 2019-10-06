using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class OrdensServicosModel
    {
        [Key]
        [Display(Name = "Código:")]
        public int Id { get; set; }
        [Display(Name = "Cliente:")]
        public int ClienteId { get; set; }
        [Display(Name = "Responsável pelo orçamento:")]
        public int Responsavel { get; set; }
        [Display(Name = "Data de realização do orçamento:")]
        public System.DateTime DataOrcamento { get; set; }
        [Display(Name = "Data prevista para o início do trabalho:")]
        public Nullable<System.DateTime> DataInicialPrevista { get; set; }
        [Display(Name = "Data prevista para o final do trabalho:")]
        public Nullable<System.DateTime> DataFinalPrevista { get; set; }
        [Display(Name = "Situação da ordem de serviços:")]
        public string Status { get; set; }
        [Display(Name = "Subtotal serviços:")]
        public Nullable<float> SubTotalServicos { get; set; }
        [Display(Name = "Subtotal materiais:")]
        public Nullable<float> SubTotalMateriais { get; set; }
        [Display(Name = "Valor total:")]
        public Nullable<float> ValorTotal { get; set; }
        [Display(Name = "Valor  à  pagar:")]
        public Nullable<float> ValorAPagar { get; set; }
        [Display(Name = "Forma de pagamento:")]
        public string FormaPagamento { get; set; }
        [Display(Name = "Informações adcionais:")]
        public string InformacoesAdicionais { get; set; }
        public Nullable<int> VeiculosId { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual Clientes Clientes { get; set; }
        public virtual Veiculos Veiculos { get; set; }
    }
}