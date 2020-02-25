using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebProject.ViewModel;

namespace WebProject.Controllers
{
    public class FacultyController : Controller
    {
        IFacultyModel facultyModel;
        public FacultyController(IFacultyModel facultyModel)
        {
            this.facultyModel = facultyModel;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(facultyLogin faculty_Login)
        {
            if (string.IsNullOrEmpty(faculty_Login.email) || string.IsNullOrEmpty(faculty_Login.password))
            {
                return View();
            }
            var r = facultyModel.Authenticate(faculty_Login);
            if (r!=null)
            {
                Session["id"] = r.facId;
                Session["email"] = faculty_Login.email;
                return View("Home");
            }
            else
            {
                TempData["error"] = "Invalid UserName and Password";
                return View();
            }
        }
        public ActionResult GetSub(string sem)
        {
            if (Session["id"] == null)
            {
                return RedirectToAction("Login");
            }
            int facId = Convert.ToInt32(Session["id"].ToString());
            var b=facultyModel.getSubjectBySem(sem, facId);
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(b);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetBatch(string sem)
        {
            if (Session["id"] == null)
            {
                return RedirectToAction("Login");
            }
            int facId = Convert.ToInt32(Session["id"].ToString());
            var b = facultyModel.getBatchBySem(sem, facId);
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(b);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSubPrac(string sem,string Batch)
        {
            if (Session["id"] == null)
            {
                return RedirectToAction("Login");
            }
            int facId = Convert.ToInt32(Session["id"].ToString());
            var b = facultyModel.getSubByBatch(Batch, facId,sem);
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(b);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AssignTaskClass(int ?sem)
        {   if(Session["id"]==null)
            {
                return RedirectToAction("Login");
            }
            int facId = Convert.ToInt32(Session["id"].ToString());
            
            return View(facultyModel.AssignTask(facId, sem));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignTaskClass(assignTask assignTask,String TFS)
        {
            int facId = Convert.ToInt32(Session["id"].ToString());
            if (facultyModel.SaveAssignTask(assignTask, facId, TFS))
                return RedirectToAction("AssignTaskClass");
            else
            {
                TempData["error"] = "Something Went Wrong";
                return RedirectToAction("AssignTaskClass");
            }
        }
        public ActionResult AssignTaskPrac(int? sem,string batch)
        {
            if (Session["id"] == null)
            {
                return RedirectToAction("Login");
            }
            int facId = Convert.ToInt32(Session["id"].ToString());

            return View(facultyModel.AssignTaskPrac(facId, sem,batch));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignTaskPrac(assignTaskPrac assignTask, String TFS)
        {
            int facId = Convert.ToInt32(Session["id"].ToString());
            if (facultyModel.SaveAssignTaskPrac(assignTask, facId, TFS))
                return RedirectToAction("AssignTaskPrac");
            else
            {
                TempData["error"] = "Something Went Wrong";
                return RedirectToAction("AssignTaskPrac");
            }
        }
    }
}