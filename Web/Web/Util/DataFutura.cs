using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Web.Models;

namespace Web.Util
{
    public class DataFutura : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime data = DateTime.Now; ;
            Type tipo = validationContext.ObjectInstance.GetType();
            if (tipo == typeof(AgendarServicoViewModel))
            {
                var agendamento = (AgendarServicoViewModel)validationContext.ObjectInstance;
                data = agendamento.DataInicial;
            }
            if (tipo == typeof(EditarInformacoesDoServicoViewModel))
            {
                var agendamento = (EditarInformacoesDoServicoViewModel)validationContext.ObjectInstance;
                data = agendamento.DataInicial;
            }
            int comparacao = DateTime.Compare(data, DateTime.Now);
            if (comparacao == 0 || comparacao > 0)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("A data prevista para o início do trabalho deve ser igual ou maior que a data atual!");
            }
        }
    }
}