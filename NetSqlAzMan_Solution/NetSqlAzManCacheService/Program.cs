//#define TEST

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace NetSqlAzMan.Cache.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new WindowsCacheService() 
			};
#if TEST
            using (WindowsCacheService s = new WindowsCacheService())
            {
                s.OnStartInternal();
            }
#else
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
