using System.ServiceProcess;

namespace HpKeyService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new HPKeyboardService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
