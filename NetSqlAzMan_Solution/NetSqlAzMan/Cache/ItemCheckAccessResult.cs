using System;
using System.Collections.Generic;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzMan.Cache
{
    /// <summary>
    /// ItemCheckAccessResult cache result.
    /// </summary>
    [Serializable()]
    public sealed class ItemCheckAccessResult
    {
        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        /// <value>The name of the item.</value>
        public string ItemName { get; set; }
        /// <summary>
        /// Gets or sets the type of the authorization.
        /// </summary>
        /// <value>The type of the authorization.</value>
        public AuthorizationType AuthorizationType { get; set; }
        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>The attributes.</value>
        public List<KeyValuePair<string, string>> Attributes { get; set; }
        /// <summary>
        /// Gets or sets the valid from.
        /// </summary>
        /// <value>The valid from.</value>
        public DateTime ValidFrom { get; set; }
        /// <summary>
        /// Gets or sets the valid to.
        /// </summary>
        /// <value>The valid to.</value>
        public DateTime ValidTo { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ItemCheckAccessResult"/> is inherited.
        /// </summary>
        /// <value><c>true</c> if inherited; otherwise, <c>false</c>.</value>
        public bool Inherited { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemCheckAccessResult"/> class.
        /// </summary>
        /// <param name="itemName">Name of the item.</param>
        public ItemCheckAccessResult(string itemName)
        {
            this.ItemName = itemName;
            this.Inherited = false;
        }

        /// <summary>
        /// Cloneds for item.
        /// </summary>
        /// <param name="itemName">Name of the item.</param>
        /// <returns></returns>
        public ItemCheckAccessResult ClonedForItem(string itemName)
        {
            ItemCheckAccessResult result = new ItemCheckAccessResult(itemName);
            result.Attributes = this.Attributes;
            result.AuthorizationType = this.AuthorizationType == AuthorizationType.AllowWithDelegation ? AuthorizationType.Allow : this.AuthorizationType;
            result.ValidFrom = this.ValidFrom;
            result.ValidTo = this.ValidTo;
            result.Inherited = true;
            return result;
        }
    }
}
