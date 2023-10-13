using Pizzeria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pizzeria.Controllers
{
    
    public class HomeController : Controller
    {

      
        ModelDbContext dbContext = new ModelDbContext();
        // GET: Home
        public ActionResult Index()
        {
            return View(dbContext.T_Prodotti.ToList());
        }



        //Action Result per mandare al db i dati del carrello
        [HttpPost]
        public ActionResult Index(List<int> pizzaIds, List<int> quantities,  string indirizzo, string nota, decimal totalCartPrice)
        {
            //Prendo userid dalla session
            int? userId = Session["UserId"] as int?;

            //Controllo se userID ha value
            if (!userId.HasValue)
            {                          
                return RedirectToAction("Login", "Login"); 
            }

            //Controllo se la nota è vuota 
            if (string.IsNullOrEmpty(nota))
            {
                //Se è vuota la imposto a una string empty
                nota = string.Empty;
            }

            //Creo un nuovo ordine
            T_Ordine newOrder = new T_Ordine
            {
                DataOrdine = DateTime.Now,
                FKUtente = userId.Value,
                Indirizzo = indirizzo, 
                Totale = totalCartPrice,
                Nota = nota ,
                Evaso = false
            };


           //Salvo l'ordine nel db
            dbContext.T_Ordine.Add(newOrder);
            dbContext.SaveChanges();

           //Faccio un for per agggiungere agli order details
            for (int i = 0; i < pizzaIds.Count; i++)
            {
                int pizzaId = pizzaIds[i];
                int quantity = quantities[i];
            

                T_Dettaglio newOrderDetail = new T_Dettaglio
                {
                    FKOrdine = newOrder.IdOrdine,
                    FKProdotto = pizzaId,
                    Quantita = quantity
                };

               
                dbContext.T_Dettaglio.Add(newOrderDetail);
                dbContext.SaveChanges();
            }

         
            return RedirectToAction("Index");
        }

        public ActionResult Lista()
        {
            int? userId = Session["UserId"] as int?;

           
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Login");
            }
            var listaOrdini = dbContext.T_Ordine.Where(O => O.FKUtente == userId).ToList();
            return View(listaOrdini);
        }



    }
}