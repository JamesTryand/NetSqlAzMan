using System;
using System.Collections.Generic;

namespace NetSqlAzManWebConsole.Objects
{
    [Serializable()]
    public class ListView
    {
        public ListView()
        {
            this.Items = new ListViewItemCollection();
            this.Enabled = true;
        }

        public ListViewItemCollection Items { get; set; }
        public bool Enabled { get; set; }
        public static void Remove(ListView listView, List<ListViewItem> itemsToRemove)
        {
            foreach (var item in itemsToRemove)
            {
                if (listView.Items.Contains(item))
                    listView.Items.Remove(item);
            }
        }
    }

    [Serializable()]
    public class ListViewItem
    {
        public ListViewItem()
        {
            this.Text = String.Empty;
            this.Tag = null;
            this.SubItems = new ListViewItemCollection();
            this.Selected = false;
        }
        public ListViewItem(string text)
        {
            this.Text = text;
        }
        public string Text { get; set; }
        public object Tag { get; set; }
        public ListViewItemCollection SubItems { get; set; }
        public bool Selected { get; set; }
        public static implicit operator ListViewItem(string itemText)
        {
            return new ListViewItem(itemText);
        }
    }

    [Serializable()]
    public class ListViewItemCollection : System.Collections.ObjectModel.Collection<ListViewItem>
    {
        public ListViewItemCollection()
        {

        }
    }
}
