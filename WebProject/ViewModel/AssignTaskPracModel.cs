using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProject.ViewModel
{
    public class AssignTaskPracModel
    {
        public IEnumerable<int?> sem { get; set; }
        public IEnumerable<String> sub { get; set; }
        public IEnumerable<String> batch { get; set; }

        public IEnumerable<assignTaskPrac> assignTaskPrac { get; set; }

    }
}