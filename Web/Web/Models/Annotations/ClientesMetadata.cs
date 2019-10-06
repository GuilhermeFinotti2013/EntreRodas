using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models
{
    public class ClientesMetadata
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
        [Display(Name = "Observações:")]
        public string Observacao { get; set; }
    }
}