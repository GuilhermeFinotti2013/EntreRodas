using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models
{
    public class VeiculosMetadata
    {
        [Key]
        [Column("Id")]
        [Display(Name =  "Código")]
        public int Id { get; set; }
        public Nullable<int> ClienteId { get; set; }
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
        [Required(ErrorMessage = "O tipo de motor deve ser informado!")]
        [StringLength(30)]
        [Display(Name = "Tipo de motor:")]
        public string TipoMotor { get; set; }
        public string Observacoes { get; set; }
        [Required(ErrorMessage = "A quilometragem atual deve ser informado!")]
        [Display(Name = "Quilometragem atual:")]
        public int QuilometragemAtual { get; set; }

        public virtual Clientes Clientes { get; set; }
    }
}