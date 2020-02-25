using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProject.ViewModel
{
    public class FacultyModel :IFacultyModel
    {
        StudentEvolutionSystemEntities db;
        public FacultyModel(StudentEvolutionSystemEntities db)
        {
            this.db = db;
        }

        public AssignTaskClassModel AssignTask(int facId,int ?sem)
        {
            AssignTaskClassModel a = new AssignTaskClassModel();
            a.assign_Tasks = db.assignTasks.Where(x => x.facId == facId).ToList();
            if (sem==null)
            {
                sem = db.assignFacSubs.Where(x => x.facId == facId).FirstOrDefault().sem;

            }
            var b = db.assignFacSubs.Where(x => x.facId == facId && x.sem==sem).Select(x=>x.sub).ToList();
            a.sub = b;
            a.sem = db.assignFacSubs.Where(x=>x.facId==facId).Select(x => x.sem).ToList();
            return a;
        }

        public AssignTaskPracModel AssignTaskPrac(int facId, int? sem, string  batch)
        {
            AssignTaskPracModel a = new AssignTaskPracModel();
            a.assignTaskPrac = db.assignTaskPracs.Where(x => x.facId == facId).ToList();
            var t = db.assignFacSubBatches.Where(x => x.facId == facId).OrderBy(x=>x.facId);
            var q= t.Where(x => x.facId == facId).Select(x => x.sem).Distinct().ToList(); 
            a.sem =q; 
            if (sem == null)
            {
                sem = q.FirstOrDefault();
            }
            if (batch==null)
            {
                batch = t.Where(x => x.sem == sem).FirstOrDefault().batch;
            }
            var b = t.Where(x => x.facId == facId && x.sem == sem && x.batch.Equals(batch)).ToList();
  
            a.sub = b.Select(x=>x.sub);
            
            var c= b.Select(x=>x.classId).FirstOrDefault();
            a.batch = t.Where(x => x.facId == facId && x.classId == c).Select(x => x.batch).ToList();
            return a;
        }

        public facultyLogin Authenticate(facultyLogin faculty_Login)
        {
            return db.facultyLogins.Where(x => x.email.Equals(faculty_Login.email) && x.password.Equals(faculty_Login.password)).SingleOrDefault();

            
        }



        public facultyLogin getFacultyByEmail(string email)
        {
            return db.facultyLogins.Where(x => x.email.Equals(email)).SingleOrDefault();
        }

        public List<string> getSubjectBySem(string sem,int facId)
        { int a = Convert.ToInt32(sem);
            var b = db.assignFacSubs.Where(x => x.facId == facId && x.sem==a).Select(x => x.sub).ToList();
            return b;
        }
        public List<string> getBatchBySem(string sem, int facId)
        {
            int a = Convert.ToInt32(sem);
            var b = db.assignFacSubBatches.Where(x => x.facId == facId && x.sem == a).Select(x => x.batch).ToList();
            return b;
        }
        public List<string> getSubByBatch(string Batch, int facId,string sem)
        {
            int a = Convert.ToInt32(sem);
            var b = db.assignFacSubBatches.Where(x => x.facId == facId && x.sem == a && x.sem==a).Select(x => x.sub).ToList();
            return b;
        }


        public bool SaveAssignTask(assignTask assignTask, int facId, string tFS)
        {
            try
            {
                assignTask.classId = db.assignFacSubs.Where(x => x.facId == facId && x.sem == assignTask.sem).FirstOrDefault().classId;
                assignTask.facId = facId;
                assignTask.startDate = DateTime.Now;
                assignTask.endDate = assignTask.startDate.AddDays(Convert.ToInt32(tFS));
                db.assignTasks.Add(assignTask);
                db.SaveChanges();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool SaveAssignTaskPrac(assignTaskPrac assignTask, int facId, string tFS)
        {
            try
            {
                assignTask.classId = db.assignFacSubs.Where(x => x.facId == facId && x.sem == assignTask.sem).FirstOrDefault().classId;
                assignTask.facId = facId;
                assignTask.startDate = DateTime.Now;
                assignTask.endDate = assignTask.startDate.AddDays(Convert.ToInt32(tFS));
                db.assignTaskPracs.Add(assignTask);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}