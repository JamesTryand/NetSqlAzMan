using System;
using System.Security;

namespace NetSqlAzManCacheServiceInvalidateUtility
{
    class Program
    {
        [PreEmptive.Attributes.Setup(CustomEndpoint = "so-s.info/PreEmptive.Web.Services.Messaging/MessagingServiceV2.asmx")]
        [PreEmptive.Attributes.SystemProfile()]
        [PreEmptive.Attributes.PerformanceProbe()]
        [PreEmptive.Attributes.Feature("NetSqlAzMan Cache Service InvalidateUtility: Invalidate Cache Invoked")]
        [PreEmptive.Attributes.Teardown()]
        [STAThread()]
        static void Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine("-----------------------------------------------------------------");
            Console.WriteLine(".NET Sql Authorization Manager - Cache Service Invalidate Utility");
            Console.WriteLine("Andrea Ferendeles - http://netsqlazman.codeplex.com");
            Console.WriteLine("-----------------------------------------------------------------");
            Console.WriteLine();
            Console.Write("1/3) Creating WCF Client ... ");
            sr.CacheServiceClient csc = new NetSqlAzManCacheServiceInvalidateUtility.sr.CacheServiceClient();
            csc.Open();
            Console.WriteLine("done.");
            Console.WriteLine("2/3) NetSqlAzManCacheService found at: {0}", csc.Endpoint.ListenUri.ToString());
            try
            {
                //Console.Write("3/3) InvalidateCache invoke ... ");
                //if (args.Length == 2)
                //    csc.InvalidateStoreApplicationCache(args[0], args[1]);
                //else if (args.Length == 1)
                //    csc.InvalidateStoreCache(args[0]);
                //else
                    csc.InvalidateCache();
                Console.WriteLine("done.");
                Console.WriteLine();
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR.\r\n\r\n{0}\r\nr\nStack Trace:\r\n{1}\r\n\r\n", ex.Message, ex.StackTrace);
                Console.ReadLine();
            }
            finally
            {
                if (csc != null && csc.State == System.ServiceModel.CommunicationState.Opened)
                {
                    try
                    {
                        csc.Close();
                    }
                    catch
                    { }
                }
                Console.WriteLine("Finished.");
            }
        }
    }
}
