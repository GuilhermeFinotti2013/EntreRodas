using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Web.Infra;

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

    public class EditarInformacoesDoServicoViewModel
    {
        public int OrdensServicosId { get; set; }
    /*    [DataType(DataType.Date)]
        [DataFutura]
        [Display(Name = "Data inícial prevista:")]
        public System.DateTime DataInicialPrevista { get; set; }*/
        [Display(Name = "Problema identificado junto com o cliente:")]
        [MaxLength(250, ErrorMessage = "O campo Problema identificado deve ter, no maxímo, 250 caracteres!")]
        public string ProblemaIdentificado { get; set; }
        [Display(Name = "Informações adcionais:")]
        [MaxLength(250, ErrorMessage = "O campo Informações adcionais deve ter, no maxímo, 250 caracteres!")]
        public string InformacoesAdicionais { get; set; }
    }

    public class VisualizarServicoViewModel
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        [Display(Name = "Nome do cliente:")]
        public String NomeCliente { get; set; }
        [Display(Name = "E-mail:")]
        public String EmailCliente { get; set; }
        [Display(Name = "Telefones:")]
        public String FonesCliente { get; set; }
        public int VeiculoId { get; set; }
        [Display(Name = "Modelo:")]
        public String ModeloVeiculo { get; set; }
        [Display(Name = "Placa:")]
        public String PlacaVeiculo { get; set; }
        [Display(Name = "Ano:")]
        public int AnoVeiculo { get; set; }
        public String CodigoOrdemServico { get; set; }
        [Display(Name = "Funcionário responsavel:")]
        public String NomeFuncionarioResponsavel { get; set; }
        [Display(Name = "Data do orçamento:")]
        public String DataOrcamento { get; set; }
        [Display(Name = "Data inicial prevista:")]
        public String DataInicialPrevista { get; set; }
        [Display(Name = "Data inicial:")]
        public String DataInicial { get; set; }
        [Display(Name = "Data da entrega:")]
        public String DataFinal { get; set; }
        [Display(Name = "Situação:")]
        public string Status { get; set; }
        [Display(Name = "Subtotal serviços:")]
        public string SubTotalServicos { get; set; }
        [Display(Name = "Subtotal materiais:")]
        public string SubTotalMateriais { get; set; }
        [Display(Name = "Valor total:")]
        public string ValorTotal { get; set; }
        [Display(Name = "Valor à pagar:")]
        public string ValorAPagar { get; set; }
        [Display(Name = "Valor em dinheiro:")]
        public string ValorDinheiro { get; set; }
        [Display(Name = "Valor no cartão:")]
        public string ValorCartao { get; set; }
        [Display(Name = "Forma de pagamento:")]
        public string FormaPagamento { get; set; }
        [Display(Name = "Problema identificado junto com o cliente:")]
        public string ProblemaIdentificado { get; set; }
        [Display(Name = "Aprovação do serviço pelo cliente:")]
        public string AprovacaoCliente { get; set; }
        [Display(Name = "Informações adcionais:")]
        public string InformacoesAdicionais { get; set; }

        public List<OrdensServicosMateriais> Materiais { get; set; }
        public List<OrdensServicosServicos> Servicos { get; set; }
        public List<String> Erros { get; set; }
    }

    public class FinalizarServicoViewModel
    {
        public int OrdensServicosId { get; set; }
        public int ClienteId { get; set; }
        [Display(Name = "Nome do cliente:")]
        public String NomeCliente { get; set; }
        [Display(Name = "Modelo:")]
        public String ModeloVeiculo { get; set; }
        [Display(Name = "Subtotal serviços:")]
        public string SubTotalServicos { get; set; }
        public string SubTotalMateriais { get; set; }
        [Display(Name = "Valor total:")]
        public string ValorTotal { get; set; }
        [Display(Name = "Valor à pagar:")]
        public float ValorAPagar { get; set; }
        [Required(ErrorMessage = "A forma de pagamento deve ser informada!")]
        [Display(Name = "Forma de pagamento:")]
        public string FormaPagamento { get; set; }
        [Display(Name = "Valor em dinheiro:")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
        public float ValorDinheiro { get; set; }
        [Display(Name = "Valor no cartão:")]
        public float ValorCartao { get; set; }
        [Display(Name = "Informações adcionais:")]
        public string InformacoesAdicionais { get; set; }
        public List<OrdensServicosMateriais> Materiais { get; set; }
        public List<OrdensServicosServicos> Servicos { get; set; }
        public List<String> Erros { get; set; }
    }

    public class AgendarServicoViewModel
    {
        public int OrdensServicosId { get; set; }
        [Required(ErrorMessage = "A data de início do trabalho deve ser informada!")]
        [Display(Name = "Data de início:")]
        [DataType(DataType.Date)]
        [DataFutura]
        public System.DateTime DataInicial{ get; set; }
    }
}