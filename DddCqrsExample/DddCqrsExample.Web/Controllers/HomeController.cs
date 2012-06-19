﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DddCqrsExample.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to The DDD + CQRS Demo App!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
