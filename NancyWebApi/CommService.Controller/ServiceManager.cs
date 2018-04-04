using System;
using System.Configuration;
using Autofac;
using log4net;
using Microsoft.Owin.Hosting;

namespace CommService.Controller
{
    public class ServiceManager
    {
        private IDisposable _webApp;
        public ILog Logger { get; set; }
        public IContainer Container { get; set; }
        public void Start()
        {       
      
            if (_webApp == null)
            {
                var host = ConfigurationManager.AppSettings["BindHost"];
                Logger.Debug("Starting");
                _webApp = WebApp.Start<Startup>(host);
                Logger.Info($"Start:{host}");
            }
        }
    }
}
