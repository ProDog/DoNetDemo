using System;
using System.IO;
using Autofac;
using Autofac.Extras.Log4Net;
using log4net;
using Skyline;

namespace CommService.Controller
{
    public class ModuleManage : Autofac.Module
    {
        public static IContainer Container { get; set; }

        public static ILog Logger { get; set; }

        public ModuleManage()
        {
            LoadSettings();
        }

        private static void LoadSettings()
        {
            var logfile = Utility.MakeAbsolutePath("log4net.xml", null);
            if (!File.Exists(logfile))
            {
                throw new FileNotFoundException("can not find log config file", "log4net.xml");
            }
            log4net.Config.XmlConfigurator.Configure(new FileInfo(logfile));
            Logger = LogManager.GetLogger("ManageServer");

        }

        static string GetLoggerName(Type t, object instance)
        {
            return t.Name;
        }

        public static void InitContainer()
        {
            if (Container != null) return;
            var builder = new ContainerBuilder();
            builder.RegisterModule(new Log4NetModule() { GetLoggerName = GetLoggerName });
            builder.RegisterModule(new ModuleManage());
            builder.RegisterInstance(builder);
            builder.RegisterType<ServiceManager>().PropertiesAutowired().SingleInstance();
            Container = builder.Build();
            builder.RegisterInstance<IContainer>(Container);
            var builder2 = new ContainerBuilder();
            builder2.RegisterInstance<IContainer>(Container);
       
        }
    }
}
