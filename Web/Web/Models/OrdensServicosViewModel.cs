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

    public class VisualizarServicoViewModel
    {
        public int ClienteId { get; set; }
        [Display(Name = "Nome do cliente")]
        public String NomeCliente { get; set; }
        [Display(Name = "E-mail")]
        public String EmailCliente { get; set; }
        [Display(Name = "Telefones")]
        public String FonesCliente { get; set; }
        public int VeiculoId { get; set; }
        [Display(Name = "Modelo")]
        public String Modelo { get; set; }
        [Display(Name = "Placa")]
        public String Placa { get; set; }
        [Display(Name = "Ano")]
        public int Ano { get; set; }
        public String CodigoOrdemServico { get; set; }
        [Display(Name = "Funcionário responsavel")]
        public String NomeFuncionarioResponsavel { get; set; }
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

        public List<OrdensServicosMateriais> Materiais { get; set; }
        public List<OrdensServicosServicos> Servicos { get; set; }
    }
}