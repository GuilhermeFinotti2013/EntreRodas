using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cadastros.Model;

namespace Cadastros
{
    public partial class Form1 : Form
    {
        entre_rodasEntities db = new entre_rodasEntities();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                Veiculos veiculo = new Veiculos();
                veiculo.Ano = 2000; ;
                veiculo.CategoriaCarro = "suv";
                veiculo.MarcaVeiculoId = 2;
                veiculo.Modelo = "a";
                veiculo.Modelo = "qwer";
                veiculo.Observacoes = "wq";
                veiculo.Placa = "qwq";
                veiculo.TipoCompustivel = "qa21";
                veiculo.TipoMotor = "21er";

                Clientes cliente = new Clientes();
                cliente.Bairro = "canudos";
                cliente.Celular = "51999026857";
                cliente.CEP = "93542070";
                cliente.Cidade = "NH";
                cliente.Complemento = "casa";
                cliente.CPF = "02743970057";
                cliente.DataNascimento = DateTime.Now;
                cliente.EhWhats = "S";
                cliente.Email = "gu@q.com";
                cliente.Nome = "gui";
                cliente.Numero = 387;
                cliente.RG = "1233211";
                cliente.Rua = "aq";
                cliente.Sexo = "m";
                cliente.Sobrenome = "finotti";
                cliente.Telefone = "12";
                cliente.Veiculos = new List<Veiculos>();
                cliente.Veiculos.Add(veiculo);

                db.Clientes.Add(cliente);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
