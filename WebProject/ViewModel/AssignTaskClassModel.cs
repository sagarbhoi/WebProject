using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProject.ViewModel
{
    public class AssignTaskClassModel
    {
        public IEnumerable<int> sem { get; set; }
        public IEnumerable<String> sub { get; set; }
        public IEnumerable<assignTask> assign_Tasks { get; set; }

    }
}