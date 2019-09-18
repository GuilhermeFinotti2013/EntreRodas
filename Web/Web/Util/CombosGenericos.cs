using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Util
{
    public class CombosGenericos
    {
        public List<TODropDownListGenerico> ListarTipoCombustivel()
        {
            return new List<TODropDownListGenerico>()
            {
                new TODropDownListGenerico{ Texto = "Bicombustível", Valor = "B"},
                new TODropDownListGenerico{ Texto = "Etanol", Valor = "E"},
                new TODropDownListGenerico{ Texto = "Gasolina", Valor = "G"}
            };
        }

        public List<TODropDownListGenerico> ListarSimNao()
        {
            return new List<TODropDownListGenerico>()
            {
                new TODropDownListGenerico{ Texto = "Sim", Valor = "S"},
                new TODropDownListGenerico{ Texto = "Não", Valor = "N"}
            };
        }

        public List<TODropDownListGenerico> ListarSexo()
        {
            return new List<TODropDownListGenerico>()
            {
                new TODropDownListGenerico{ Texto = "Masculino", Valor = "M"},
                new TODropDownListGenerico{ Texto = "Feminino", Valor = "F"}
            };
        }

        public List<TODropDownListGenerico> ListarAnos()
        {
            List<TODropDownListGenerico> listaDeAnos = new List<TODropDownListGenerico>();
            TODropDownListGenerico downListGenerico = new TODropDownListGenerico()
            {
                Texto = DateTime.Now.AddYears(1).Year.ToString(),
                Valor = DateTime.Now.AddYears(1).Year.ToString()
            };
            listaDeAnos.Add(downListGenerico);
            for (int i = 0; i < 50; i++)
            {
                listaDeAnos.Add(new TODropDownListGenerico()
                {
                    Texto = DateTime.Now.AddYears(-i).Year.ToString(),
                    Valor = DateTime.Now.AddYears(-i).Year.ToString()
                });
            }
            return listaDeAnos;
        }
    }
}