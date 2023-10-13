﻿using Pizzeria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Pizzeria.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        ModelDbContext db = new ModelDbContext();
        
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult login()
        {
            return View();
        }

        [HttpPost]

        public ActionResult login(T_Utenti u)
        {

            var user = db.T_Utenti.FirstOrDefault(
                usr => usr.Username == u.Username && usr.Password == u.Password);

            if (user != null)
            {
                Session["UserId"] = user.IdUtente;
                FormsAuthentication.SetAuthCookie(user.Username, false);


                return RedirectToAction("Index", "Home");
            }
            else
            {

                ModelState.AddModelError("", "Credenziali non valide. Riprova.");
                return View();
            }
        }

        public ActionResult Registrati()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Registrati([Bind(Exclude = "Role")] T_Utenti u)
        {
            u.Role = "User";
            db.T_Utenti.Add(u);
            db.SaveChanges();
            return RedirectToAction("Login", "Login");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            
            return RedirectToAction("Index", "Home");
        }
    }
}