using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Web.Mvc;
using ZzaWeb.Models;
using ZzaWeb.ZzaService;

namespace ZzaWeb.Controllers
{
    public class CLientsController : Controller
    {
        //
        // GET: /CLients/

        public ActionResult Index(string hostname)
        {
            try
            {
                ZzaService.ZzaService sv = new ZzaService.ZzaService();
                var clients = sv.GetMachineInfo(hostname).ToList();
                return View(clients);
            }
            catch (Exception)
            {
                return null;
            }
        
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id)
        {
            bool result;
            bool resp;
            ZzaService.ZzaService sv = new ZzaService.ZzaService();
            sv.DeleteClient(id: id, idSpecified: true, DeleteClientResult: out result, DeleteClientResultSpecified: out resp);
            return RedirectToAction("Index");
        }


        public ActionResult Edit(int id)
        {
            try
            {
                ZzaService.ZzaService sv = new ZzaService.ZzaService();
                var client = sv.GetMachineInfoById(id,true);
                return View(client);
            }
            catch (Exception)
            {
                return null;
            }
           
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, Client client)
        {
            try
            {
                bool result;
                bool test;
                ZzaService.ZzaService sv = new ZzaService.ZzaService();
                client.IDSpecified = true;

                sv.UpdateClientInformation(client, out result, out test);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return null;
            }
           
        }

        public ActionResult Create()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Client client)
        {
            try
            {
                bool result;
                bool test;
                client.IDSpecified = true;
                ZzaService.ZzaService sv = new ZzaService.ZzaService();
                sv.CreateClient(client, out result, out test);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult Details(int id)
        {
            try
            {
                ZzaService.ZzaService sv = new ZzaService.ZzaService();
                Session["clientid"] = id;
                var detailsView = sv.GetDetailsView(id, true).ToList();
                return View(detailsView);
            }
            catch (Exception)
            {
                return null;
            }
           
        }

    }
}
