
namespace CubicOrange.Windows.Forms.ActiveDirectory
{
    /// <summary>
    /// Details of a directory object selected in the DirectoryObjectPickerDialog.
    /// </summary>
    public class DirectoryObject
    {
        private string adsPath;
        private string className;
        private string name;
        private string upn;

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryObject"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="path">The path.</param>
        /// <param name="schemaClass">The schema class.</param>
        /// <param name="upn">The upn.</param>
        public DirectoryObject(string name, string path, string schemaClass, string upn)
        {
            this.name = name;
            this.adsPath = path;
            this.className = schemaClass;
            this.upn = upn;
        }

        /// <summary>
        /// Gets the Active Directory path for this directory object.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The format of this string depends on the options specified in the DirectoryObjectPickerDialog
        /// from which this object was selected.
        /// </para>
        /// </remarks>
        public string Path
        {
            get
            {
                return adsPath;
            }
        }

        /// <summary>
        /// Gets the name of the schema class for this directory object (objectClass attribute).
        /// </summary>
        public string SchemaClassName
        {
            get
            {
                return className;
            }
        }

        /// <summary>
        /// Gets the directory object's relative distinguished name (RDN).
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Gets the objects user principal name (userPrincipalName attribute).
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the object does not have a userPrincipalName value, this property is an empty string. 
        /// </para>
        /// </remarks>
        public string Upn
        {
            get
            {
                return upn;
            }
        }
    }
}
