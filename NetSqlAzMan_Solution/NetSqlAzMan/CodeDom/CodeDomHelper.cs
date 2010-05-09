using System;
using System.CodeDom;

namespace NetSqlAzMan.CodeDom
{
    internal static class CodeDomHelper
    {
        internal static void AddNamespaceHeaderComment(CodeNamespace nameSpace, params string[] comments)
        {
            foreach (string comment in comments)
            {
                nameSpace.Comments.Add(new CodeCommentStatement(comment));
            }
        }

        internal static void AddStandardNamespaces(CodeNamespace nameSpace)
        {
            nameSpace.Imports.Add(new CodeNamespaceImport("System"));
            nameSpace.Imports.Add(new CodeNamespaceImport("System.Security.Principal"));
            nameSpace.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            nameSpace.Imports.Add(new CodeNamespaceImport("System.Text"));
        }

        internal static void AddNetSqlAzManNamespaces(CodeNamespace nameSpace)
        {
            string globalPrefix = String.Empty;
            nameSpace.Imports.Add(new CodeNamespaceImport("NetSqlAzMan"));
            nameSpace.Imports.Add(new CodeNamespaceImport("NetSqlAzMan.Interfaces"));
        }

        internal static void AddCustomAttribute(CodeTypeDeclaration type, string AttributeName)
        {
            type.CustomAttributes.Add(new CodeAttributeDeclaration(AttributeName));
        }

        internal static void AddCustomAttribute(CodeMemberProperty type, string AttributeName)
        {
            type.CustomAttributes.Add(new CodeAttributeDeclaration(AttributeName));
        }

        internal static void AddCustomAttribute(CodeTypeDeclaration type, string AttributeName, string ArgumentName, object ArgumentValue)
        {
            CodeAttributeDeclaration cad = new CodeAttributeDeclaration(AttributeName);
            CodeAttributeArgument cadarg = new CodeAttributeArgument();
            if (ArgumentName != String.Empty)
                cadarg.Name = ArgumentName;
            cadarg.Value = new CodePrimitiveExpression(ArgumentValue);
            cad.Arguments.Add(cadarg);
            type.CustomAttributes.Add(cad);
        }

        internal static void AddXmlSummaryComment(CodeTypeDeclaration type, string summaryComment)
        {
            type.Comments.Add(new CodeCommentStatement("<summary>", true));
            type.Comments.Add(new CodeCommentStatement(summaryComment, true));
            type.Comments.Add(new CodeCommentStatement("</summary>", true));
        }
        internal static void AddXmlSummaryComment(CodeConstructor type, string summaryComment)
        {
            type.Comments.Add(new CodeCommentStatement("<summary>", true));
            type.Comments.Add(new CodeCommentStatement(summaryComment, true));
            type.Comments.Add(new CodeCommentStatement("</summary>", true));
        }
        internal static void AddXmlSummaryComment(CodeMemberField field, string summaryComment)
        {
            field.Comments.Add(new CodeCommentStatement("<summary>", true));
            field.Comments.Add(new CodeCommentStatement(summaryComment, true));
            field.Comments.Add(new CodeCommentStatement("</summary>", true));
        }

        internal static void AddXmlSummaryComment(CodeMemberProperty property, string summaryComment)
        {
            property.Comments.Add(new CodeCommentStatement("<summary>", true));
            property.Comments.Add(new CodeCommentStatement(summaryComment, true));
            property.Comments.Add(new CodeCommentStatement("</summary>", true));
        }

        internal static void AddXmlSummaryComment(CodeMemberMethod method, string summaryComment)
        {
            method.Comments.Add(new CodeCommentStatement("<summary>", true));
            method.Comments.Add(new CodeCommentStatement(summaryComment, true));
            method.Comments.Add(new CodeCommentStatement("</summary>", true));
        }

        internal static CodeMemberField AddFieldDeclaration(CodeTypeDeclaration type, MemberAttributes memberAttribute, string fieldType, string fieldName)
        {
            CodeMemberField cmf = new CodeMemberField(fieldType, fieldName);
            cmf.Attributes = memberAttribute;
            type.Members.Add(cmf);
            return cmf;
        }
        internal static string TransformToVariable(string prefix, string name, bool toupper)
        {
            string ris = "";
            if (String.IsNullOrEmpty(name)) return String.Empty;
            char[] nc = name.ToCharArray();
            for (int i = 0; i < nc.Length; i++)
            {
                if (!char.IsLetterOrDigit(nc[i]) && (nc[i] != '_'))
                {
                    if (nc[i] != '[')
                    {
                        ris += "_";
                    }
                }
                else
                {
                    ris += nc[i].ToString();
                }
            }
            if (toupper)
            {
                ris = ris.ToUpper();
            }
            if (!char.IsLetter(ris.ToCharArray()[0]))
            {
                ris = "_" + ris;

            }
            ris = ris.Trim('_');
            ris = prefix + ris;
            return ris;
        }
    }
}
