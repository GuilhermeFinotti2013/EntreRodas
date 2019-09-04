using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class ClienteViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O nome do cliente deve ser informado!")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O sobrenome do cliente deve ser informado!")]
        [Display(Name = "Sobrenome")]
        public string Sobrenome { get; set; }
        [Display(Name = "Sexo")]
        public string Sexo { get; set; }
        [Required(ErrorMessage = "A data de nascimento do cliente deve ser informado!")]
        [Display(Name = "Data de nascimento")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }
        [Required(ErrorMessage = "O CPF do cliente deve ser informado!")]
        [Display(Name = "CPF")]
        public string CPF { get; set; }
        [Required(ErrorMessage = "O RG do cliente deve ser informado!")]
        [Display(Name = "RG")]
        public string RG { get; set; }
        //[Required(ErrorMessage = "O E-mail do cliente deve ser informado!")]
        [Display(Name = "E-mail")]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Telefone")]
        public string Telefone { get; set; }
        [Display(Name = "Celular")]
        public string Celular { get; set; }
        [Display(Name = "Esse celular tem WhatsApp?")]
        public string EhWhats { get; set; }
        //[Required(ErrorMessage = "O CEP do cliente deve ser informado!")]
        [Display(Name = "CEP")]
        public string CEP { get; set; }
        //[Required(ErrorMessage = "O endereço do cliente deve ser informado! Por favor, imforme a rua!")]
        [Display(Name = "Rua")]
        public string Rua { get; set; }
        //[Required(ErrorMessage = "O endereço do cliente deve ser informado! Por favor, imforme o número!")]
        [Display(Name = "Número")]
        public int Numero { get; set; }
        [Display(Name = "Complemento")]
        public string Complemento { get; set; }
        //[Required(ErrorMessage = "O endereço do cliente deve ser informado! Por favor, imforme o bairro!")]
        [Display(Name = "Bairro")]
        public string Bairro { get; set; }
        //[Required(ErrorMessage = "O endereço do cliente deve ser informado! Por favor, imforme a cidade!")]
        [Display(Name = "Cidade")]
        public string Cidade { get; set; }
        [Display(Name = "Observações sobre o cliente")]
        public string Observacao { get; set; }
        public IEnumerable<MarcasCarros> MarcasCarros { get; set; }
        [Display(Name = "Marca")]
        public int MarcaSelecionada { get; set; }
        //[Required(ErrorMessage = "O modelo do carro deve ser informado!")]
        [Display(Name = "Modelo")]
        public string Modelo { get; set; }
        //[Required(ErrorMessage = "O ano de fabricação do carro deve ser informado!")]
        [Display(Name = "Ano")]
        public int Ano { get; set; }
        //[Required(ErrorMessage = "A placa do carro deve ser informado!")]
        [Display(Name = "Placa")]
        public string Placa { get; set; }
        [Display(Name = "Tipo de combustível")]
        public string TipoCombustivel { get; set; }
        //[Required(ErrorMessage = "O tipo de motor do carro deve ser informado!")]
        [Display(Name = "Tipo de motor")]
        public string TipoMotor { get; set; }
        [Display(Name = "Observações sobre o carro")]
        public string ObservacaoCarro { get; set; }
    }
}