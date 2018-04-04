using System;
using System.IO;
using Autofac;

namespace CommService.Controller
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            { 
                Console.WriteLine("CommService Start...");
                ModuleManage.InitContainer();

                ServiceManager manage = ModuleManage.Container.Resolve<ServiceManager>();
                manage.Start();
                Console.WriteLine("CommService Started");
                Console.ReadKey();
            }
            catch(System.Exception ex)
            {
                File.WriteAllText("error.log", ex.ToString());
                Console.WriteLine($@"Error:{ex}");                
                Console.ReadKey();
            }
        }
    }
}
