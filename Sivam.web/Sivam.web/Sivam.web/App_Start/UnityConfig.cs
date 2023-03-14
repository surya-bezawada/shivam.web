using Sivam.web.Services;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace Sivam.web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IAdminService, AdminService>();
            container.RegisterType<IHomeSerives, HomeSerives>();
            container.RegisterType<ISMSService, SMSService>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}