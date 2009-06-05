using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzMan.Cache
{
    /// <summary>
    /// Authorized Item Class.
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class AuthorizedItem
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [DataMember()]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [DataMember()]
        public ItemType Type { get; set; }
        /// <summary>
        /// Gets or sets the authorization.
        /// </summary>
        /// <value>The authorization.</value>
        [DataMember()]
        public AuthorizationType Authorization { get; set; }
        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>The attributes.</value>
        [DataMember()]
        public List<KeyValuePair<string, string>> Attributes { get; set; }
    }
}
