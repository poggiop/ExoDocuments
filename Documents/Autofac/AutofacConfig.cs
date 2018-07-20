using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Documents;
using Services;
using Services.Interface;

namespace Documents.Autofac
{
    public class AutofacConfig
    {

        private static IContainer container;

        public static IContainer Container
        {
            get { return container; }
            set { container = value; }
        }

        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            //// MVC - Register your MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);




            //// MVC - OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinderProvider();

            //// MVC - OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            //// MVC - OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            //// MVC - OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();



            ////Services
            builder.RegisterType<FolderService>().As<IFolderService>();


            ////Repositories


            //// MVC - Set the dependency resolver to be Autofac.
            Container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));



        }
    }
}