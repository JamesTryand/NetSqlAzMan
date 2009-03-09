//#define COLLECTRESOURCES

using System;
using System.Collections;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace NetSqlAzMan.SnapIn.Globalization
{
    internal static class ResourcesManager
    {
        #region COLLECTRESOURCES

        private static string fileName = "c:\\resource.txt";
        private static SortedList<string, string> sl = new SortedList<string, string>();
        static ResourcesManager()
        {
            if (File.Exists(fileName))
            {
                StreamReader sr = File.OpenText(fileName);
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string key = line.Substring(0, line.IndexOf('\t'));
                    string value = line.Substring(line.IndexOf('\t') + 1).TrimEnd('\t');
                    sl.Add(key, value);
                }
                sr.Close();
            }
        }

        internal static void CollectResources(Form form, Control.ControlCollection cc)
        {

            try
            {
                foreach (Control c in cc)
                {
                    bool skip = false;
                    string key = String.Empty;
                    string value = String.Empty;
                    //Exceptions
                    if (
                        c.Name == "rbCSharp" ||
                        c.Name == "rbVBNet"
                        )
                    {
                        continue;
                    }
                    //
                    if (c as Label != null)
                    {
                        key = String.Format("{0}_{1}.Text", form.Name, ((Label)c).Name); value = c.Text;
                    }
                    if (c as Button != null)
                    {
                        key = String.Format("{0}_{1}.Text", form.Name, ((Button)c).Name); value = c.Text;
                    }
                    if (c as RadioButton != null)
                    {
                        key = String.Format("{0}_{1}.Text", form.Name, ((RadioButton)c).Name); value = c.Text;
                    }
                    if (c as CheckBox != null)
                    {
                        key = String.Format("{0}_{1}.Text", form.Name, ((CheckBox)c).Name); value = c.Text;
                    }
                    if (c as GroupBox != null)
                    {
                        key = String.Format("{0}_{1}.Text", form.Name, ((GroupBox)c).Name); value = c.Text;
                    }
                    if (c as TabControl != null)
                    {
                        foreach (TabPage tb in ((TabControl)c).TabPages)
                        {
                            key = String.Format("{0}_{1}.Text", form.Name, tb.Name); value = tb.Text;
                            ResourcesManager.CollectResources(form, tb.Controls);
                        }
                    }
                    if (c as ListView != null)
                    {
                        foreach (ColumnHeader ch in ((ListView)c).Columns)
                        {
                            skip = true;
                            key = String.Format("{0}_{1}_{2}.Text", form.Name, ((ListView)c).Name, ((ColumnHeader)ch).Index); value = ch.Text;
                            ResourcesManager.AddToCollection(sl, key, value);
                        }
                    }
                    if (!skip)
                    {
                        ResourcesManager.AddToCollection(sl, key, value);
                    }
                }
            }
            catch
            {
                ResourcesManager.WriteResources();
            }

        }
        internal static void CollectResources(Form form)
        {
#if !COLLECTRESOURCES
            return;
#endif
#if COLLECTRESOURCES
            try
            {
                ResourcesManager.AddToCollection(sl, String.Format("{0}.Text", form.Name), form.Text);
                foreach (Control c in form.Controls)
                {
                    bool skip = false;
                    string key = String.Empty;
                    string value = String.Empty;
                    //Exceptions
                    if (
                        c.Name == "rbCSharp" ||
                        c.Name == "rbVBNet"
                        )
                    {
                        continue;
                    }
                    //
                    if (c as Label != null)
                    {
                        key = String.Format("{0}_{1}.Text", form.Name, ((Label)c).Name); value = c.Text;
                    }
                    if (c as Button != null)
                    {
                        key = String.Format("{0}_{1}.Text", form.Name, ((Button)c).Name); value = c.Text;
                    }
                    if (c as RadioButton != null)
                    {
                        key = String.Format("{0}_{1}.Text", form.Name, ((RadioButton)c).Name); value = c.Text;
                    }
                    if (c as CheckBox != null)
                    {
                        key = String.Format("{0}_{1}.Text", form.Name, ((CheckBox)c).Name); value = c.Text;
                    }
                    if (c as GroupBox != null)
                    {
                        key = String.Format("{0}_{1}.Text", form.Name, ((GroupBox)c).Name); value = c.Text;
                        ResourcesManager.CollectResources(form, c.Controls);
                    }
                    if (c as TabControl != null)
                    {
                        foreach (TabPage tb in ((TabControl)c).TabPages)
                        {
                            key = String.Format("{0}_{1}.Text", form.Name, tb.Name); value = tb.Text;
                            ResourcesManager.CollectResources(form, tb.Controls);
                        }
                    }
                    if (c as ListView != null)
                    {
                        foreach (ColumnHeader ch in ((ListView)c).Columns)
                        {
                            skip = true;
                            key = String.Format("{0}_{1}_{2}.Text", form.Name, ((ListView)c).Name, ((ColumnHeader)ch).Index); value = ch.Text;
                            ResourcesManager.AddToCollection(sl, key, value);
                        }
                    }
                    if (!skip)
                    {
                        ResourcesManager.AddToCollection(sl, key, value);
                    }
                }
            }
            catch
            {
                ResourcesManager.WriteResources();
            }
#endif
        }
        private static void AddToCollection(SortedList<string, string> coll, string key, string value)
        {
            if (key!=String.Empty && !coll.ContainsKey(key))
            {
                coll.Add(key, value);
            }
        }
        internal static void WriteResources()
        {
#if COLLECTRESOURCES
            StreamWriter sw = File.CreateText(fileName);
            foreach (string key in sl.Keys)
            {
                sw.WriteLine(String.Format("{0}\t{1}\t", key, sl[key]));
            }
            sw.Flush();
            sw.Close();
#endif
        }

        #endregion COLLECTRESOURCES
        #region ManagerResource
        /// <summary>
        /// Manages the resource.
        /// </summary>
        /// <param name="form">The form.</param>
        internal static void ManageResource(Form form)
        {
            form.Text = MultilanguageResource.GetString(form);
            foreach (Control c in form.Controls)
            {
                string key = String.Empty;
                string value = String.Empty;
                //Exceptions
                if (
                    c.Name == "rbCSharp" ||
                    c.Name == "rbVBNet"
                    )
                {
                    continue;
                }
                //
                if (c as Label != null)
                {
                    key = String.Format("{0}_{1}.Text", form.Name, ((Label)c).Name);
                    c.Text = MultilanguageResource.GetString(key);
                }
                if (c as Button != null)
                {
                    key = String.Format("{0}_{1}.Text", form.Name, ((Button)c).Name);
                    c.Text = MultilanguageResource.GetString(key);
                }
                if (c as RadioButton != null)
                {
                    key = String.Format("{0}_{1}.Text", form.Name, ((RadioButton)c).Name);
                    c.Text = MultilanguageResource.GetString(key);
                }
                if (c as CheckBox != null)
                {
                    key = String.Format("{0}_{1}.Text", form.Name, ((CheckBox)c).Name);
                    c.Text = MultilanguageResource.GetString(key);
                }
                if (c as GroupBox != null)
                {
                    key = String.Format("{0}_{1}.Text", form.Name, ((GroupBox)c).Name);
                    c.Text = MultilanguageResource.GetString(key);
                    ResourcesManager.ManageResource(form, c.Controls);
                }
                if (c as TabControl != null)
                {
                    foreach (TabPage tb in ((TabControl)c).TabPages)
                    {
                        key = String.Format("{0}_{1}.Text", form.Name, tb.Name);
                        tb.Text = MultilanguageResource.GetString(key);
                        ResourcesManager.ManageResource(form, tb.Controls);
                    }
                }
                if (c as ListView != null)
                {
                    foreach (ColumnHeader ch in ((ListView)c).Columns)
                    {
                        key = String.Format("{0}_{1}_{2}.Text", form.Name, ((ListView)c).Name, ((ColumnHeader)ch).Index);
                        ch.Text = MultilanguageResource.GetString(key);
                    }
                }
            }
        }
        /// <summary>
        /// Manages the resource.
        /// </summary>
        /// <param name="form">The form.</param>
        internal static void ManageResource(Form form, Control.ControlCollection cc)
        {
            form.Text = MultilanguageResource.GetString(form);
            foreach (Control c in cc)
            {
                string key = String.Empty;
                string value = String.Empty;
                //Exceptions
                if (
                    c.Name == "rbCSharp" ||
                    c.Name == "rbVBNet"
                    )
                {
                    continue;
                }
                //
                if (c as Label != null)
                {
                    key = String.Format("{0}_{1}.Text", form.Name, ((Label)c).Name);
                    c.Text = MultilanguageResource.GetString(key);
                }
                if (c as Button != null)
                {
                    key = String.Format("{0}_{1}.Text", form.Name, ((Button)c).Name);
                    c.Text = MultilanguageResource.GetString(key);
                }
                if (c as RadioButton != null)
                {
                    key = String.Format("{0}_{1}.Text", form.Name, ((RadioButton)c).Name);
                    c.Text = MultilanguageResource.GetString(key);
                }
                if (c as CheckBox != null)
                {
                    key = String.Format("{0}_{1}.Text", form.Name, ((CheckBox)c).Name);
                    c.Text = MultilanguageResource.GetString(key);
                }
                if (c as GroupBox != null)
                {
                    key = String.Format("{0}_{1}.Text", form.Name, ((GroupBox)c).Name);
                    c.Text = MultilanguageResource.GetString(key);
                }
                if (c as TabControl != null)
                {
                    foreach (TabPage tb in ((TabControl)c).TabPages)
                    {
                        key = String.Format("{0}_{1}.Text", form.Name, tb.Name);
                        tb.Text = MultilanguageResource.GetString(key);
                    }
                }
                if (c as ListView != null)
                {
                    foreach (ColumnHeader ch in ((ListView)c).Columns)
                    {
                        key = String.Format("{0}_{1}_{2}.Text", form.Name, ((ListView)c).Name, ((ColumnHeader)ch).Index);
                        ch.Text = MultilanguageResource.GetString(key);
                    }
                }
            }
        }
        #endregion ManagerResource
    }
}
