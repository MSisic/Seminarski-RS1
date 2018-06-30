﻿using Kulturno_sportski_centar.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;
using System.Data.Entity;
using Kulturno_sportski_centar.Areas.ModulZaposlenik.Models;

namespace Kulturno_sportski_centar.Areas.ModulZaposlenik.Controllers
{
    public class TerminController : Controller
    {
        MojContext ctx = new MojContext();
       
        public ActionResult Index()
        {
            return View("Index");
        }
        public ActionResult Prikazi(int? SalaId)
        {
            TerminPrikaziViewModel Model = new TerminPrikaziViewModel();
            Model.sale = ctx.Sala.Include(x => x.KulturnoSportskiCentar).ToList();
            ProvjeraTermin();
            if (SalaId != null)
            {
                Model.Termini = ctx.Termin
                    .Where(x => x.SalaId == SalaId && x.Rezervisan == false&&x.Zavrsena==false)
                .Include(x => x.Sala)
                .ToList();
            }
            else
            {
                Model.Termini = ctx.Termin
                    .Where(x => x.Rezervisan == false && x.Zavrsena == false)
                .Include(x => x.Sala)
                .ToList();
            }


            return View("Prikazi", Model);
        }

        public ActionResult Dodaj()
        {
            TerminEditViewModel Model = new TerminEditViewModel();
            Model.Sale = UcitajSale();
            
            
            return View("Dodaj",Model);
        }

        public ActionResult Spremi(TerminEditViewModel T)
        {
            List<Sala> Sale = ctx.Sala.ToList();
            TimeSpan pocetak = new TimeSpan(10,0,0);
            TimeSpan kraj = new TimeSpan(23,0,0);
            TimeSpan povecalo = new TimeSpan(1, 0, 0);
            foreach (var x in Sale)
            {
                Termin termin = new Termin();
                termin.Sala = x;
                termin.SalaId = x.Id;
                termin.Datum = T.Datum;
                termin.Zavrsena = false;
                while (pocetak < kraj)
                {
                    termin.Pocetak = pocetak;
                    pocetak += povecalo;
                    termin.Kraj = pocetak;
                    ctx.Termin.Add(termin);
                    ctx.SaveChanges();
                }
                pocetak = new TimeSpan(10, 0, 0);


            }
            

            return RedirectToAction("Prikazi");
        }
        private List<SelectListItem> UcitajSale()
        {
            List<SelectListItem> lista = new List<SelectListItem>();
            lista.AddRange(ctx.Sala.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.KulturnoSportskiCentar.Naziv + " - " + x.Naziv }));
            return lista;
        }

        private void ProvjeraTermin()
        {
            List<Termin> termini = ctx.Termin.ToList();
            DateTime Datum = DateTime.Today;
            foreach (var x in termini)
            {
                if (x.Datum < Datum)
                    x.Zavrsena = true;
            }
            ctx.SaveChanges();
        }
    }
}