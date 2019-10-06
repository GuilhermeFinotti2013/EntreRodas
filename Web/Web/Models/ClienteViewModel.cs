using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class ClienteViewModel
    {
        [Key]
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required(ErrorMessage = "O nome deve ser informado!")]
        [StringLength(75)]
        [Display(Name = "Nome:")]
        public string Nome { get; set; }
        [Display(Name = "Sexo:")]
        public string Sexo { get; set; }
        [Required(ErrorMessage = "A data de nascimento deve ser informada!")]
        [Display(Name = "Data de nascimento:")]
        [DataType(DataType.Date)]
        public System.DateTime DataNascimento { get; set; }
        [Required(ErrorMessage = "O CPF deve ser informado!")]
        [StringLength(14)]
        [Display(Name = "CPF:")]
        public string CPF { get; set; }
        [Required(ErrorMessage = "O RG deve ser informado!")]
        [StringLength(30)]
        [Display(Name = "RG:")]
        public string RG { get; set; }
        [Required(ErrorMessage = "O e-mail deve ser informado!")]
        [StringLength(120)]
        [Display(Name = "E-mail:")]
        public string Email { get; set; }
        [StringLength(20)]
        [Display(Name = "Telefone fixo:")]
        public string Telefone { get; set; }
        [StringLength(20)]
        [Display(Name = "Celular:")]
        public string Celular { get; set; }
        [StringLength(1)]
        [Display(Name = "É WhatsApp?:")]
        public string EhWhats { get; set; }
        [Required(ErrorMessage = "O CEP deve ser informado!")]
        [StringLength(10)]
        [Display(Name = "CEP:")]
        public string CEP { get; set; }
        [Required(ErrorMessage = "A rua deve ser informado!")]
        [StringLength(100)]
        [Display(Name = "Rua:")]
        public string Rua { get; set; }
        [Required(ErrorMessage = "O número deve ser informado!")]
        [Display(Name = "Número:")]
        public int Numero { get; set; }
        [StringLength(30)]
        [Display(Name = "Complemento:")]
        public string Complemento { get; set; }
        [Required(ErrorMessage = "O bairro deve ser informado!")]
        [StringLength(50)]
        [Display(Name = "Bairro:")]
        public string Bairro { get; set; }
        [Required(ErrorMessage = "A cidade deve ser informada!")]
        [StringLength(100)]
        [Display(Name = "Cidade:")]
        public string Cidade { get; set; }
        [MaxLength(300)]
        [Display(Name = "Observações sobre o cliente:")]
        public string Observacao { get; set; }
        [Required(ErrorMessage = "A fabrícante deve ser informada!")]
        [Display(Name = "Fabrícante:")]
        public int MarcaVeiculoId { get; set; }
        [Required(ErrorMessage = "O modelo do carro deve ser informado!")]
        [StringLength(100)]
        [Display(Name = "Modelo:")]
        public string Modelo { get; set; }
        [Required(ErrorMessage = "O ano de fabrícação deve ser informado!")]
        [Display(Name = "Ano:")]
        public int Ano { get; set; }
        [Required(ErrorMessage = "A placa deve ser informado!")]
        [StringLength(20)]
        [Display(Name = "Placa:")]
        public string Placa { get; set; }
        [Required(ErrorMessage = "A categoria deve ser informado!")]
        [StringLength(30)]
        [Display(Name = "Categoria:")]
        public string CategoriaCarro { get; set; }
        [Display(Name = "Tipo de combustível:")]
        public string TipoCombustivel { get; set; }
        [StringLength(30)]
        [Display(Name = "Tipo de motor:")]
        public string TipoMotor { get; set; }
        [Required(ErrorMessage = "A quilometragem atual deve ser informado!")]
        [Display(Name = "Quilometragem atual:")]
        public int QuilometragemAtual { get; set; }
        [Display(Name = "Observações sobre o veículo:")]
        public string ObservacaoCarro { get; set; }
        public virtual MarcasCarros MarcasCarros { get; set; }
        public List<Veiculos> VeiculosDoCliente { get; set; }
    }
}