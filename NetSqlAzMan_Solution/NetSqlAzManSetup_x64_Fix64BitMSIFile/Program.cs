using System;
using System.Configuration;

namespace NetSqlAzManSetup_x64_Fix64BitMSIFile
{
    /// <summary>
    /// http://blog.crowe.co.nz/archive/2008/11/13/64-bit-Managed-Custom-DLL-Actions-with-Visual-Studio-do-not.aspx
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            bool ChangesMade = false;

            Console.WriteLine("Update an MSI File with a 64bit version of InstallUtilLib");
            Console.WriteLine();

            string InstallUtil64BitFileSpec = ConfigurationManager.AppSettings["InstallUtil64BitFileSpec"];
            string MSIFileSpec = ConfigurationManager.AppSettings["MSIFileSpec"];

            if (System.IO.File.Exists(InstallUtil64BitFileSpec) == false)
            {
                Console.WriteLine("File Not Found : " + InstallUtil64BitFileSpec);
                return;
            }
            if (System.IO.File.Exists(MSIFileSpec) == false)
            {
                Console.WriteLine("File Not Found : " + MSIFileSpec);
                return;
            }

            Console.WriteLine("MSIFile:" + Environment.NewLine + MSIFileSpec);
            Console.WriteLine();
            Console.WriteLine("InstallUtil: " + Environment.NewLine + InstallUtil64BitFileSpec);
            Console.WriteLine();
            Console.WriteLine("Opening the MSI File");
            //WindowsInstaller.Installer i = (WindowsInstaller.Installer)new Fix64BitMSIFile.Installer();

            Type classType = Type.GetTypeFromProgID("WindowsInstaller.Installer");
            Object installerClassObject = Activator.CreateInstance(classType);
            WindowsInstaller.Installer i = (WindowsInstaller.Installer)installerClassObject; 

            WindowsInstaller.Database db = i.OpenDatabase(MSIFileSpec, WindowsInstaller.MsiOpenDatabaseMode.msiOpenDatabaseModeTransact);
            Console.WriteLine("Running SQL Query for InstallUtil in the Binary MSI table");

            // NOTE: The ` is correct in the SQL statement below - it is not " or ' 

            WindowsInstaller.View v = db.OpenView("SELECT `Name`,`Data` FROM `Binary` where `Binary`.`Name` = 'InstallUtil'");
            v.Execute(null);
            WindowsInstaller.Record Record = v.Fetch();
            if (Record != null)
            {
                Console.WriteLine("Updating the Binary Data for InstallUtil");
                Record.SetStream(2, InstallUtil64BitFileSpec);
                v.Modify(WindowsInstaller.MsiViewModify.msiViewModifyUpdate, Record);
                ChangesMade = true;
            }
            else
            {
                Console.WriteLine("Error : InstallUtil not found in the Binary MSI Table");
            }
            v.Close();
            if (ChangesMade)
            {
                Console.WriteLine("Commiting the changes to the database");
                db.Commit();
            }

            Console.WriteLine();
            Console.WriteLine("Completed.....");
            Console.ReadLine();
        }
    }
}
