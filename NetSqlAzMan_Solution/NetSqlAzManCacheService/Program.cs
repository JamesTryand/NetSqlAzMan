//#define TEST

using System.ServiceProcess;

namespace NetSqlAzMan.Cache.Service
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[PreEmptive.Attributes.Setup(CustomEndpoint = "so-s.info/PreEmptive.Web.Services.Messaging/MessagingServiceV2.asmx")]
        [PreEmptive.Attributes.Teardown()]
        //[PreEmptive.Attributes.PerformanceProbe()]
        //[PreEmptive.Attributes.SystemProfile()]
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
