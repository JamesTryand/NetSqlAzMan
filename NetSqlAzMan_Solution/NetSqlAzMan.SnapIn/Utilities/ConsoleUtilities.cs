using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSqlAzMan.SnapIn.Utilities
{
    internal static class ConsoleUtilities
    {
        internal static bool commandLineArgumentOn(string option)
        {
            foreach (var argument in Environment.GetCommandLineArgs())
            { 
                if (String.Compare(argument, "/"+option, true)==0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
