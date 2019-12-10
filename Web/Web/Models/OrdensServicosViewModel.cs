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
        public String ProblemaIdentificado { get; set; }
    }

    public class VisualizarServicoViewModel
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        [Display(Name = "Nome do cliente")]
        public String NomeCliente { get; set; }
        [Display(Name = "E-mail")]
        public String EmailCliente { get; set; }
        [Display(Name = "Telefones")]
        public String FonesCliente { get; set; }
        public int VeiculoId { get; set; }
        [Display(Name = "Modelo")]
        public String ModeloVeiculo { get; set; }
        [Display(Name = "Placa")]
        public String PlacaVeiculo { get; set; }
        [Display(Name = "Ano")]
        public int AnoVeiculo { get; set; }
        public String CodigoOrdemServico { get; set; }
        [Display(Name = "Funcionário responsavel")]
        public String NomeFuncionarioResponsavel { get; set; }
        [Display(Name = "Data do orçamento:")]
        public String DataOrcamento { get; set; }
        [Display(Name = "Data inicial prevista:")]
        public String DataInicialPrevista { get; set; }
        [Display(Name = "Data inicial:")]
        public String DataInicial { get; set; }
        [Display(Name = "Data de entrega prevista:")]
        public String DataFinal { get; set; }
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
        [Display(Name = "Problema identificado junto com o cliente:")
        public string ProblemaIdentificado { get; set; }
        [Display(Name = "Aprovação do serviço pelo cliente:")]
        public string AprovacaoCliente { get; set; }
        [Display(Name = "Informações adcionais:")]
        public string InformacoesAdicionais { get; set; }

        public List<OrdensServicosMateriais> Materiais { get; set; }
        public List<OrdensServicosServicos> Servicos { get; set; }
    }
}