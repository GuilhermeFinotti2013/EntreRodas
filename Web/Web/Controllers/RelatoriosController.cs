using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class RelatoriosController : Controller
    {
        private entre_rodasEntities db = new entre_rodasEntities();

        // GET: Relatorios
        public ActionResult Orcamento(int ordemServicoId)
        {
            OrdensServicos ordensServicos = db.OrdensServicos.Find(ordemServicoId);

            return View();
        }
    }
}