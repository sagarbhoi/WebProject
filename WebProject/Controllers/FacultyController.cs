using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebProject.ViewModel;
using IronPdf;
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
        public ActionResult AssignTaskClassCheck()
        {
            ViewBag.Path = Server.MapPath("~/Pdf/1.pdf"); 
            return View();
        }
        [HttpPost]
        public ActionResult AssignTaskClassCheck(string path)
        {
                //string a = Server.MapPath(path);
                PdfDocument Pdf = PdfDocument.FromFile(path);
                var BackgroundStamp = new HtmlStamp() { Html = "<img src='data:image/jpeg;base64,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' />", Width = 50, Height = 25 };
                Pdf.StampHTML(BackgroundStamp);
                Pdf.SaveAs(path);
                return RedirectToAction("AssignTaskClassCheck");
        }

    }
}