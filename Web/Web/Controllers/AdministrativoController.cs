using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    [Authorize(Roles = "Administradores")]
    public class AdministrativoController : Controller
    {
        // GET: Administrativo
        public ActionResult Index()
        {
            return View();
        }
    }
}