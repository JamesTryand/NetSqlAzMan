using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using System.Collections;

namespace NetSqlAzMan.SnapIn.Globalization
{
    /// <summary>
    /// Class for Multilingual Management
    /// </summary>
    internal sealed class MultilanguageResource
    {
        internal static CultureInfo cultureInfo = null;
        internal static global::System.Resources.ResourceManager resourceManager = null;
        internal static Hashtable currentResources = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private static string GetStringInternal(string key)
        {
            if (MultilanguageResource.currentResources.ContainsKey(key))
                return (String)MultilanguageResource.currentResources[key];

            string culture = MultilanguageResource.GetCurrentCulture();
            if (cultureInfo == null)
            {
                MultilanguageResource.SetCulture(culture);
            }
            if (resourceManager == null)
                resourceManager = new ResourceManager(String.Format("NetSqlAzMan.SnapIn.Globalization.Resource_{0}", culture), Assembly.GetExecutingAssembly());
            string result;
            try
            {
                result = MultilanguageResource.resourceManager.GetString(key);
                if (result != null)
                {
                    if (result.IndexOf("\\r\\n") != -1)
                    {
                        result = result.Replace("\\r\\n", "\r\n");
                    }
                }
                else
                {
                    result = String.Format("Globalization Error: Key '{0}' NOT FOUND in .resx file !!!", key);
                }
            }
            catch
            {
                result = String.Format("Globalization Error: Key '{0}' NOT FOUND in .resx file !!!", key);
            }
            lock (MultilanguageResource.currentResources.SyncRoot)
            {
                MultilanguageResource.currentResources.Add(key, result);
            }
            return result;
        }

        /// <summary>
        /// Sets the culture.
        /// </summary>
        /// <param name="culture">The culture.</param>
        internal static void SetCulture(string culture)
        {
            MultilanguageResource.currentResources = Hashtable.Synchronized(new Hashtable());
            MultilanguageResource.resourceManager = null;
            MultilanguageResource.cultureInfo = new CultureInfo(MultilanguageResource.cultureSuffix(culture));
            Thread.CurrentThread.CurrentUICulture = MultilanguageResource.cultureInfo;
        }

        /// <summary>
        /// Cultures the suffix.
        /// </summary>
        /// <param name="cultureName">Name of the culture.</param>
        /// <returns></returns>
        private static string cultureSuffix(string cultureName)
        {
            string cultureString = "en";
            string culture = cultureName.ToLower();
            if (culture.StartsWith("english")) cultureString = "en";
            if (culture.StartsWith("italian")) cultureString = "it";
            if (culture.StartsWith("spanish")) cultureString = "es";
            if (culture.StartsWith("albanian")) cultureString = "sq";
            if (culture.StartsWith("russian")) cultureString = "ru";
            return cultureString;
        }

        /// <summary>
        /// Cultures the suffix.
        /// </summary>
        /// <param name="cultureName">Name of the culture.</param>
        /// <returns></returns>
        internal static string cultureName(string cultureSuffix)
        {
            string cultureString = "english";
            string culture = cultureSuffix.ToLower();
            if (culture.StartsWith("en")) cultureString = "English";
            if (culture.StartsWith("it")) cultureString = "Italian";
            if (culture.StartsWith("es")) cultureString = "Spanish";
            if (culture.StartsWith("sq")) cultureString = "Albanian";
            if (culture.StartsWith("ru")) cultureString = "Russian";
            return cultureString;
        }

        /// <summary>
        /// Gets the current culture.
        /// </summary>
        internal static string GetCurrentCulture()
        {
            string culture = Thread.CurrentThread.CurrentUICulture.ToString();
            string cultureString = "en";
            if (culture.StartsWith("en")) cultureString = "en";
            if (culture.StartsWith("it")) cultureString = "it";
            if (culture.StartsWith("es")) cultureString = "es";
            if (culture.StartsWith("sq")) cultureString = "sq";
            if (culture.StartsWith("ru")) cultureString = "ru";
            return cultureString;
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns></returns>
        internal static string GetString(Form form)
        {
            return MultilanguageResource.GetStringInternal(String.Format("{0}.Text",form.Name));
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="className">Name of the class.</param>
        /// <param name="messageName">Name of the message.</param>
        /// <returns></returns>
        internal static string GetString(string className, string messageName)
        {
            return MultilanguageResource.GetStringInternal(String.Format("{0}_{1}", className, messageName));
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        internal static string GetString(string key)
        {
            return MultilanguageResource.GetStringInternal(key);
        }
    }
}
