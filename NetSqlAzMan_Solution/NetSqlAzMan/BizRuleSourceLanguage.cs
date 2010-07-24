using System.Runtime.Serialization;

namespace NetSqlAzMan
{
    /// <summary>
    /// Source Code Language for Biz Rules
    /// </summary>
    [DataContract(Namespace = "http://NetSqlAzMan/ServiceModel", IsReference = true)]
    public enum BizRuleSourceLanguage : byte
    {
        /// <summary>
        /// JavaScript
        /// </summary>
        [EnumMember]
        CSharp,
        /// <summary>
        /// Visual Basic Script
        /// </summary>
        [EnumMember]
        VBNet
    }
}
