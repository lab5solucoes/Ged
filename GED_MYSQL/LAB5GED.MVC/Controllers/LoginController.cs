﻿using LAB5GED.DOMAIN.Acessorio;
using LAB5GED.DOMAIN.AppContext;
using LAB5GED.DOMAIN.DAO.Business;
using LAB5GED.DOMAIN.Entidades;
using LAB5GED.MVC.Acessorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace LAB5GED.MVC.Controllers
{
    public class LoginController : CustomController
    {
        //
        // GET: /Login/
        private UsuarioBO _DAO = new UsuarioBO();

        public ActionResult Index()
        {



            return View();
        }

        [HttpPost]
        public ActionResult Logon(FormCollection fc)
        {


            //try
            //{
            if (DateTime.Now > DateTime.Parse("2020-02-28"))
            {
                return RedirectToAction("Index", "Home").ComMensagemDeErro("SISTEMA INDISPONÍVEL. CONTATE O ADMINISTRADOR");
            }

            string erroLogon = "usuário ou senha incorreto";
            Usuario usr = _DAO.GetUsuario(fc["login"], new Seguranca().getMD5Hash(fc["senha"]));

            if (usr != null)
            {
                if (usr.Ativo)
                {
                    FormsAuthentication.SetAuthCookie(usr.Registro.ToString(), false);
                    return RedirectToAction("Index", "Home");
                }
                else
                    erroLogon = "usuário INATIVO";
                Logador.LogAcao(Logador.LogAcoes.Acesso, erroLogon);
            }
            Logador.LogAcao(Logador.LogAcoes.Acesso, erroLogon);
            ViewBag.Erro = erroLogon;
            //return View("Index");
            return RedirectToAction("Index").ComMensagemDeErro(erroLogon);



            //}
            //catch (Exception ex)
            //{
            //    return RedirectToAction("Index").ComMensagemDeErro(ex.Message);
            //}
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Logador.LogAcao(Logador.LogAcoes.Acesso, "Saiu do sistema");
            return RedirectToAction("Index");

        }

        public Usuario GetUsuarioLogado()
        {
            return _DAO.GetByRegistro(int.Parse(HttpContext.User.Identity.Name));
        }

        public ActionResult AcessoNegado()
        {
            return View();
        }
    }
}
