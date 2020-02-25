using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProject.ViewModel
{
    public interface IFacultyModel
    {
        facultyLogin Authenticate(facultyLogin faculty_Login);
        facultyLogin getFacultyByEmail(string email);

        AssignTaskClassModel AssignTask(int facId,int ?sem);
        bool SaveAssignTask(assignTask assignTask, int facId, string tFS);
        List<String> getSubjectBySem(string sem,int facId);
        AssignTaskPracModel AssignTaskPrac(int facId, int? sem,string batch);
        List<string> getSubByBatch(string Batch, int facId, string sem);
        List<string> getBatchBySem(string sem, int facId);
        bool SaveAssignTaskPrac(assignTaskPrac assignTask, int facId, string tFS);
    }
}