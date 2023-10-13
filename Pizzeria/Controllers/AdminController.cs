using Pizzeria.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pizzeria.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        ModelDbContext db = new ModelDbContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View(db.T_Prodotti.ToList());
        }



        //Create dei prodotti
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(T_Prodotti pizza, HttpPostedFileBase Img)
        {
            if (Img != null && Img.ContentLength > 0)
            {

                pizza.Foto = Img.FileName;


                string imagePath = Server.MapPath("~/Content/Img/") + Img.FileName;
                Img.SaveAs(imagePath);
            }


            db.T_Prodotti.Add(pizza);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }




        //Modifica dei prodotti
        public ActionResult Modifica(int id)
        {
            T_Prodotti pizza = db.T_Prodotti.Find(id);
            return View(pizza);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifica(T_Prodotti pizza, HttpPostedFileBase Img, int id)
        {
            if (ModelState.IsValid)
            {
                var existingPizza = db.T_Prodotti.Find(id);
                if (existingPizza != null)
                {
                    if (Img != null && Img.ContentLength > 0)
                    {

                        existingPizza.Foto = Img.FileName;

                        string imagePath = Server.MapPath("~/Content/Img/") + Img.FileName;
                        Img.SaveAs(imagePath);
                    }


                    existingPizza.Nome = pizza.Nome;
                    existingPizza.Costo = pizza.Costo;
                    existingPizza.TempoConsegna = pizza.TempoConsegna;
                    existingPizza.Ingredienti = pizza.Ingredienti;

                    db.Entry(existingPizza).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }


        //Elimina prodotto
        public ActionResult Elimina (int id)
        {
            T_Prodotti pizza = db.T_Prodotti.Find(id);
            return View(pizza);
        }

        [HttpPost, ActionName("Elimina")]
        public ActionResult EliminaConferma(int id)
        {
            // Recupera tutti i dettagli con il dato FKProdotto
            var dettagliDaRimuovere = db.T_Dettaglio.Where(d => d.FKProdotto == id).ToList();

            // Rimuovi tutti i dettagli
            foreach (var dettaglio in dettagliDaRimuovere)
            {
                db.T_Dettaglio.Remove(dettaglio);
            }

            // Salva le modifiche nel database
            db.SaveChanges();

            // Rimuovi anche il prodotto
            var prodottoDaRimuovere = db.T_Prodotti.Find(id);
            if (prodottoDaRimuovere != null)
            {
                db.T_Prodotti.Remove(prodottoDaRimuovere);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        //Lista completa degli ordini
        public ActionResult Lista () 
        {

            var ListaProdotti = db.T_Ordine.Where(o => o.Evaso == false || o.Evaso == null);
            return View(ListaProdotti.ToList()); 
        }


        //Modifica del ordine
        public ActionResult ModificaOrdine(int id)
        {
           
            T_Ordine ordine = db.T_Ordine.Find(id);

            if (ordine == null)
            {
               
                return RedirectToAction("Lista");
            }

            return View(ordine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModificaOrdine(T_Ordine ordine)
        {
            if (ModelState.IsValid)
            {
                // Cerca l'ordine nel database
                var existingOrdine = db.T_Ordine.Find(ordine.IdOrdine);

                if (existingOrdine == null)
                {
                    // Ordine non trovato, gestisci l'errore o restituisci una vista di errore
                    return RedirectToAction("Lista");
                }

                // Assicurati che il campo Evaso abbia un valore prima di accedervi
                if (ordine.Evaso.HasValue)
                {
                    existingOrdine.Evaso = ordine.Evaso.Value;
                }

                // Esegui le altre modifiche necessarie
                existingOrdine.DataOrdine = ordine.DataOrdine;
                existingOrdine.FKUtente = ordine.FKUtente;

                db.Entry(existingOrdine).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Lista");
            }

            // Se ModelState.IsValid è false, significa che ci sono errori di convalida, quindi restituisci la vista con gli errori
            return View(ordine);
        }


        public ActionResult Ricerca()
        {
            return View();
        }



        //Json per ordini evasi in data odierna
        public JsonResult TotOrdiniEvasi(DateTime data)
        {
            DateTime? giorno = data;
            decimal? totaleOrdiniEvasi = 0;
            List<Ordine> ordini = new List<Ordine>();
            var ordiniEvasi = db.T_Ordine.Where(o => o.Evaso == true && o.DataOrdine == giorno);

            foreach (var or in ordiniEvasi)
            {
                totaleOrdiniEvasi += or.Totale;
                Ordine ordine = new Ordine
                {
                    total = totaleOrdiniEvasi,
                    evaso = or.Evaso
                };
                
                ordini.Add(ordine);
            }
            int totale = ordini.Count;

            var result = new { TotaleOrdini = totale, TotalePrezzo = totaleOrdiniEvasi };

            return Json(result, JsonRequestBehavior.AllowGet);
        }



    }
}