using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("NetSqlAzMan.SnapIn")]
[assembly: AssemblyDescription(".NET SQL Authorization Manager")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Andrea Ferendeles")]
[assembly: AssemblyProduct("NetSqlAzMan.SnapIn")]
[assembly: AssemblyCopyright("Copyright © Andrea Ferendeles 2006-2010")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("f83a48da-c3f9-4ded-bdb0-759dd0b2099c")]
[assembly: PreEmptive.Attributes.Business("3E35F098-CE43-4F82-9E9D-05C8B1046A45")]
[assembly: PreEmptive.Attributes.Application("5B254108-E61B-4349-8961-1FF56B39F3B5", "NetSqlAzMan", "3.6.0.6", "NetSqlAzMan.SnapIn.dll")]
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("3.6.0.6")]
[assembly: AssemblyFileVersion("3.6.0.6")]
[assembly: AllowPartiallyTrustedCallers()]
[assembly: System.Security.SecurityRules(System.Security.SecurityRuleSet.Level1)]
