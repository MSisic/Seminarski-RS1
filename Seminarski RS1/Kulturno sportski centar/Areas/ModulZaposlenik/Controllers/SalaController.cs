﻿using Kulturno_sportski_centar.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;
using System.Data.Entity;

namespace Kulturno_sportski_centar.Areas.ModulZaposlenik.Controllers
{

    public class SalaController : Controller
    {
        MojContext ctx = new MojContext();
        // GET: Sala
        public ActionResult Index()
        {
            return View("index");
        }
        

        public ActionResult Prikazi()
        {
            List<Sala> Sale = new List<Sala>();
      
          
            Sale = ctx.Sala.Include(x=>x.KulturnoSportskiCentar).ToList();
            ViewData["Sala"] = Sale;
         
            return View("Prikazi");
        }
    }


}