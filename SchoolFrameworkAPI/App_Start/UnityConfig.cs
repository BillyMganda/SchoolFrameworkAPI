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
            container.RegisterType<ITeacherRepository, TeacherRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IFormRepository, FormRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IStudentRepository, StudentRepository>(new HierarchicalLifetimeManager());

            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}