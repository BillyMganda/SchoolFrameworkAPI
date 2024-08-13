using SchoolFrameworkAPI.Controllers;
using SchoolFrameworkAPI.Repositories;
using System.Web.Http;
using Unity;
using Unity.Lifetime;

namespace SchoolFrameworkAPI.App_Start
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // Register repositories and other dependencies here
            container.RegisterType<IDepartmentRepository, DepartmentRepository>(new HierarchicalLifetimeManager());

            // Register controllers if necessary
            //container.RegisterType<DepartmentsController>();

            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}