using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZzaWeb.ZzaService;

namespace ZzaWeb.Controllers
{
    public class TaskController : Controller
    {
        //
        // GET: /Task/

        public int ClientID { get; set; }

        public ActionResult Index()
        {
            ZzaService.ZzaService sv = new ZzaService.ZzaService();
            var task = sv.GetDetailsView((int)Session["clientid"], true);
            return View(task);
        }


        
        public ActionResult Delete(int id)
        {
            bool result;
            bool resp;
            ZzaService.ZzaService sv = new ZzaService.ZzaService();
            sv.DeleteTask(id, true, out result, out resp);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            try
            {
                ZzaService.ZzaService sv = new ZzaService.ZzaService();
                var task = sv.GetTaskInfoById(id, true);
                return View(task);
            }
            catch (Exception)
            {
                return null;
            }

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, Task task)
        {
            try
            {
                bool result;
                bool test;
                task.ClientIDSpecified = true;
                task.taskIDSpecified = true;
                ZzaService.ZzaService sv = new ZzaService.ZzaService();
                sv.UpdateTaskInformation(task, out result, out test);
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
        public ActionResult Create(Task task)
        {
            try
            {
                bool result;
                bool test;
                task.ClientIDSpecified = true;
                task.ClientID = Convert.ToInt32(Session["clientid"]);
                task.taskIDSpecified = true;
                task.Status = "Queued";
                ZzaService.ZzaService sv = new ZzaService.ZzaService();
                sv.CreateTask(task, out result, out test);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
