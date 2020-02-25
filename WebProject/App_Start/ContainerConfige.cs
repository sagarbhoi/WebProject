using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebProject.ViewModel;

namespace WebProject.App_Start
{
    public class ContainerConfige
    {
        internal static void Register()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterType<StudentEvolutionSystemEntities>().InstancePerRequest();
            builder.RegisterType<FacultyModel>().As<IFacultyModel>();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}