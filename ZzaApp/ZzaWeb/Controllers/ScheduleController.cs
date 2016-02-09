using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZzaWeb.ZzaService;

namespace ZzaWeb.Controllers
{
    public class ScheduleController : Controller
    {
        //
        // GET: /Schedule/
        
        public ActionResult Index()
        {
            ZzaService.ZzaService sv = new ZzaService.ZzaService();
            var task = sv.GetScheduleByTaskId((int)Session["taskid"], true).ToList();
            return View(task);
            
        }

        public ActionResult Details(int id)
        {
            try
            {
                ZzaService.ZzaService sv = new ZzaService.ZzaService();
                Session["taskid"] = id;
                var detailsView = sv.GetScheduleByTaskId(id, true).ToList();
                return View(detailsView);
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
        public ActionResult Create(Schedule schedule)
        {
            try
            {
                bool result;
                bool test;
                schedule.taskIDSpecified = true;
                schedule.endAtSpecified = true;
                schedule.startAtSpecified = true;
                schedule.intHoursSpecified = true;
                schedule.intMinSpecified = true;
                schedule.intSecSpecified = true;
                schedule.taskID = (int) Session["taskid"];
                ZzaService.ZzaService sv = new ZzaService.ZzaService();
                sv.CreateSchedule(schedule, out result, out test);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return null;
            }
        }

        
        public ActionResult Delete(int id)
        {
            bool result;
            bool resp;
            ZzaService.ZzaService sv = new ZzaService.ZzaService();
            sv.DeleteSchedule( id, true, out result, out resp);
            return RedirectToAction("Index");
        }



        public ActionResult Edit(int id)
        {
            try
            {
                ZzaService.ZzaService sv = new ZzaService.ZzaService();
                var schedule = sv.GetScheduleInfoById(id, true);
                return View(schedule);
            }
            catch (Exception)
            {
                return null;
            }

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, Schedule schedule)
        {
            try
            {
                bool result;
                bool test;
                ZzaService.ZzaService sv = new ZzaService.ZzaService();
                schedule.taskIDSpecified = true;
                schedule.sIDSpecified = true;
                schedule.sID = id;
                schedule.taskID =(int)Session["taskid"];
                schedule.endAtSpecified = true;
                schedule.startAtSpecified = true;
                schedule.intHoursSpecified = true;
                schedule.intMinSpecified = true;
                schedule.intSecSpecified = true;

                sv.UpdateScheduleInformation(schedule, out result, out test);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return null;
            }

        }

    }
}
