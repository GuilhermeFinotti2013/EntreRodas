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
            var agendamento = (AgendarServicoViewModel)validationContext.ObjectInstance;
            int comparacao = DateTime.Compare(agendamento.DataInicial, DateTime.Now);
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