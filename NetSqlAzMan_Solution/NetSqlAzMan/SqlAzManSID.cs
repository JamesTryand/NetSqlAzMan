using System;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.LINQ;
using System.Linq;

namespace NetSqlAzMan
{
    /// <summary>
    /// Represent a Security IDentifier (SID)
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://NetSqlAzMan/ServiceModel", IsReference = true)]
    public sealed partial class SqlAzManSID : IAzManSid, System.Runtime.Serialization.ISerializable
    {
        internal SecurityIdentifier securityIdentifier = null;
        internal Guid guid = Guid.Empty;
        internal byte[] customSid = null;


        /// <summary>
        /// Initializes a new instance of the <see cref="T:SqlAzManSID"/> class.
        /// </summary>
        /// <param name="sddlForm">The SDDL form.</param>
        public SqlAzManSID(string sddlForm)
        {
            if (sddlForm.StartsWith("S-1"))
                this.securityIdentifier = new SecurityIdentifier(sddlForm);
            else

                guid = new Guid(sddlForm);
        }

        internal bool IsGuid(string candidate, out Guid output)
        {
            Regex isGuid = new Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled);
            bool isValid = false;
            output = Guid.Empty;
            if (candidate != null)
            {
                if (isGuid.IsMatch(candidate))
                {
                    output = new Guid(candidate);
                    isValid = true;
                }
            }
            return isValid;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SqlAzManSID"/> class.
        /// </summary>
        /// <param name="sddlForm">The SDDL form.</param>
        /// <param name="customSid">if set to <c>true</c> [custom sid].</param>
        public SqlAzManSID(string sddlForm, bool customSid)
        {
            Regex isGuid = new Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled);
            if (customSid)
            {
                Guid g;
                if (sddlForm.StartsWith("S-1"))
                {
                    this.securityIdentifier = new SecurityIdentifier(sddlForm);
                }
                else if (IsGuid(sddlForm, out g))
                {
                    this.customSid = g.ToByteArray();
                }
                else
                {
                    int discarded;
                    this.customSid = NetSqlAzMan.Utilities.HexEncoding.GetBytes(sddlForm, out discarded);
                }
            }
            else
            {
                if (sddlForm.StartsWith("S-1"))
                    this.securityIdentifier = new SecurityIdentifier(sddlForm);
                else
                    guid = new Guid(sddlForm);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SqlAzManSID"/> class.
        /// </summary>
        /// <param name="sid">The sid.</param>
        public SqlAzManSID(SecurityIdentifier sid)
        {
            this.securityIdentifier = sid;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SqlAzManSID"/> class.
        /// </summary>
        /// <param name="binaryForm">The binary form.</param>
        /// <remarks>Valid only for SecurityIdentifier(s) and Guid(s)</remarks>
        public SqlAzManSID(byte[] binaryForm)
        {
            if (binaryForm == null)
            {
                this.guid = Guid.Empty;
            }
            else
            {
                if (binaryForm.Length == 16 && binaryForm[0] != 1)
                    this.guid = new Guid(binaryForm);
                else
                    this.securityIdentifier = new SecurityIdentifier(binaryForm, 0);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SqlAzManSID"/> class.
        /// </summary>
        /// <param name="binaryForm">The binary form.</param>
        /// <param name="customSid">if set to <c>true</c> [custom sid].</param>
        /// <remarks>Valid only for custom Sid (DB Users)</remarks>
        public SqlAzManSID(byte[] binaryForm, bool customSid)
        {
            if (customSid)
            {
                if (binaryForm.Length > 3 && binaryForm[0] == 48 && binaryForm[1] == 0 && binaryForm[2] == 120 && binaryForm[3] == 0)
                {
                    //Comes from SqlBinary: 0xFFFFFFFF
                    byte[] sid = new byte[binaryForm.Length - 4];
                    for (int i = 4; i < binaryForm.Length; i++)
                    {
                        sid[i - 4] = binaryForm[i];
                    }
                    this.customSid = sid;
                }
                else
                {
                    this.customSid = binaryForm;
                }
            }
            else
            {
                if (binaryForm.Length == 16 && binaryForm[0] != 1)
                    this.guid = new Guid(binaryForm);
                else
                    this.securityIdentifier = new SecurityIdentifier(binaryForm, 0);
            }
        }

        /// <summary>
        /// Gets the binary value.
        /// </summary>
        /// <value>The binary value.</value>
        public byte[] BinaryValue
        {
            get
            {
                if (this.securityIdentifier != null)
                {
                    byte[] result = new byte[this.securityIdentifier.BinaryLength];
                    this.securityIdentifier.GetBinaryForm(result, 0);
                    return result;
                }
                else if (this.guid != Guid.Empty)
                {
                    return this.guid.ToByteArray();
                }
                else
                {
                    return this.customSid;
                }
            }
        }

        /// <summary>
        /// Gets the string value.
        /// </summary>
        /// <value>The string value.</value>
        public string StringValue
        {
            get
            {
                if (this.securityIdentifier != null)
                    return this.securityIdentifier.Value;
                else if (this.guid != Guid.Empty)
                    return this.guid.ToString();
                else
                    return NetSqlAzMan.Utilities.HexEncoding.ToString(this.customSid);
            }
        }

        /// <summary>
        /// News the SQL az man owner.
        /// </summary>
        /// <returns></returns>
        public static IAzManSid NewSqlAzManSid()
        {
            Guid guid;
            bool isGood=false;
            SqlAzManSID result=null;
            while (!isGood)
            {
                try
                {
                    do
                    {
                        guid = Guid.NewGuid();
                    } while (guid.ToByteArray()[0] == 1);
                    result = new SqlAzManSID(Guid.NewGuid().ToByteArray());
                    isGood = true;
                }
                catch
                {
                    isGood = false;
                } 
            }
            return result;
        }

        /// <summary>
        /// Operator == for SqlAzManSID.
        /// </summary>
        /// <param name="sid">The sid.</param>
        /// <param name="binaryForm">The binary form.</param>
        /// <returns></returns>
        public static bool operator ==(SqlAzManSID sid, byte[] binaryForm)
        {
            byte[] binarySid = sid.BinaryValue;
            if (binaryForm.Length != binarySid.Length)
            {
                return false;
            }
            for (int i=0;i<binaryForm.Length;i++)
            {
                if (binarySid[i] != binaryForm[i])
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Operator != for SqlAzManSID.
        /// </summary>
        /// <param name="sid">The owner.</param>
        /// <param name="binaryForm">The binary form.</param>
        /// <returns></returns>
        public static bool operator !=(SqlAzManSID sid, byte[] binaryForm)
        {
            return !(sid == binaryForm);
        }

        /// <summary>
        /// Implicit operators the specified windows identity.
        /// </summary>
        /// <param name="windowsIdentity">The windows identity.</param>
        /// <returns></returns>
        public static implicit operator SqlAzManSID(WindowsIdentity windowsIdentity)
        {
            return new SqlAzManSID(windowsIdentity.User);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Equalses the specified sid.
        /// </summary>
        /// <param name="sid">The sid.</param>
        /// <returns></returns>
        public bool Equals(SecurityIdentifier sid)
        {
            return this.securityIdentifier.Equals(sid);
        }

        /// <summary>
        /// Equalses the specified GUID.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <returns></returns>
        public bool Equals(Guid guid)
        {
            return this.guid.Equals(guid);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return this.StringValue;
        }

        #region Static Members
        /// <summary>
        /// Gets the bytes from int32.
        /// </summary>
        /// <param name="n">The n.</param>
        /// <returns></returns>
        public static byte[] GetBytesFromInt32(int n)
        {
            byte[] result = BitConverter.GetBytes(n);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(result);
            return result;
        }

        /// <summary>
        /// SIDs to int32.
        /// </summary>
        /// <param name="n">The n.</param>
        /// <returns></returns>
        public static SqlAzManSID SIDToInt32(int n)
        {
            return new SqlAzManSID(SqlAzManSID.GetBytesFromInt32(n), true);
        }
        #endregion Static Members
        #region ISerializable Members

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlAzManSID"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        public SqlAzManSID(SerializationInfo info, StreamingContext context)
        {
            string oSecurityIdentifier = info.GetString("securityIdentifier");
            if (oSecurityIdentifier!=null)
                this.securityIdentifier = new SecurityIdentifier(oSecurityIdentifier);
            string oGuid = info.GetString("guid");
            if (oGuid!=null)
                this.guid = new Guid(info.GetString("guid"));
            this.customSid = (byte[])info.GetValue("customSid", typeof(byte[]));
        }

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with data.</param>
        /// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization.</param>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
        public void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            info.AddValue("securityIdentifier", this.securityIdentifier !=null ? this.securityIdentifier.Value : null);
            info.AddValue("guid", this.guid!=null ? this.guid.ToString() : null);
            info.AddValue("customSid", this.customSid, typeof(byte[]));
        }

        #endregion
    }
}
