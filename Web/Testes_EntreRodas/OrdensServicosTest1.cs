using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.Controllers;
using Web.Models;

namespace Testes_EntreRodas
{
    [TestClass]
    public class OrdensServicosTest1
    {
        [TestMethod]
        public void CalcularValorTotalServicosSemItens()
        {
            OrdensServicosController ordensServicosController = new OrdensServicosController();
            OrdensServicos ordensServicos = new OrdensServicos();
            float valor = ordensServicosController.CalcularValorTotal(ordensServicos);
            Assert.AreEqual(0, valor);
        }
        [TestMethod]
        public void CalcularValorTotalServicosSomenteMateriais()
        {
            OrdensServicosController ordensServicosController = new OrdensServicosController();
            OrdensServicos ordensServicos = new OrdensServicos();
            List<OrdensServicosMateriais> materiais = new List<OrdensServicosMateriais>();
            OrdensServicosMateriais material = new OrdensServicosMateriais();
            material.PrecoTotal = 75;
            materiais.Add(material);
            material = new OrdensServicosMateriais();
            material.PrecoTotal = 50;
            materiais.Add(material);
            ordensServicos.OrdensServicosMateriais = materiais;
            float valor = ordensServicosController.CalcularValorTotal(ordensServicos);
            Assert.AreEqual(125, valor);
        }
        [TestMethod]
        public void CalcularValorTotalServicosSomenteServicos()
        {
            OrdensServicosController ordensServicosController = new OrdensServicosController();
            OrdensServicos ordensServicos = new OrdensServicos();
            List<OrdensServicosServicos> servicos = new List<OrdensServicosServicos>();
            OrdensServicosServicos servico = new OrdensServicosServicos();
            servico.Valor = 135;
            servicos.Add(servico);
            servico = new OrdensServicosServicos();
            servico.Valor = 420;
            servicos.Add(servico);
            ordensServicos.OrdensServicosServicos = servicos;
            float valor = ordensServicosController.CalcularValorTotal(ordensServicos);
            Assert.AreEqual(555, valor);
        }
        [TestMethod]
        public void CalcularValorTotalServicosCompleto()
        {
            OrdensServicosController ordensServicosController = new OrdensServicosController();
            OrdensServicos ordensServicos = new OrdensServicos();
            List<OrdensServicosMateriais> materiais = new List<OrdensServicosMateriais>();
            OrdensServicosMateriais material = new OrdensServicosMateriais();
            material.PrecoTotal = 75;
            materiais.Add(material);
            material = new OrdensServicosMateriais();
            material.PrecoTotal = 50;
            materiais.Add(material);
            ordensServicos.OrdensServicosMateriais = materiais;
            List<OrdensServicosServicos> servicos = new List<OrdensServicosServicos>();
            OrdensServicosServicos servico = new OrdensServicosServicos();
            servico.Valor = 135;
            servicos.Add(servico);
            servico = new OrdensServicosServicos();
            servico.Valor = 420;
            servicos.Add(servico);
            ordensServicos.OrdensServicosServicos = servicos;
            float valor = ordensServicosController.CalcularValorTotal(ordensServicos);
            Assert.AreEqual(680, valor);
        }
        [TestMethod]
        public void CalcularValorSubtotalServicos()
        {
            OrdensServicosController ordensServicosController = new OrdensServicosController();
            List<OrdensServicosServicos> servicos = new List<OrdensServicosServicos>();
            OrdensServicosServicos servico = new OrdensServicosServicos();
            servico.Valor = 135;
            servicos.Add(servico);
            servico = new OrdensServicosServicos();
            servico.Valor = 420;
            servicos.Add(servico);
            float valor = ordensServicosController.CalcularValorTotalDeServicos(servicos);
            Assert.AreEqual(555, valor);
        }
        [TestMethod]
        public void CalcularValorSubtotalMateriais()
        {
            OrdensServicosController ordensServicosController = new OrdensServicosController();
            List<OrdensServicosMateriais> materiais = new List<OrdensServicosMateriais>();
            OrdensServicosMateriais material = new OrdensServicosMateriais();
            material.PrecoTotal = 75;
            materiais.Add(material);
            material = new OrdensServicosMateriais();
            material.PrecoTotal = 50;
            materiais.Add(material);
            float valor = ordensServicosController.CalcularValorTotalDeMateriais(materiais);
            Assert.AreEqual(125, valor);
        }
    }
}
