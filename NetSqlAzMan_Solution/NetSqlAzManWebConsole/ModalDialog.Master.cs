using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace NetSqlAzManWebConsole
{
    public partial class ModalDialog : System.Web.UI.MasterPage
    {
        /// <summary>
        /// internalBtnApply control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected internal global::System.Web.UI.WebControls.Button internalBtnApply;

        /// <summary>
        /// litEnd control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected internal global::System.Web.UI.WebControls.Literal litEnd;

        /// <summary>
        /// internalBtnCancel control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected internal global::System.Web.UI.WebControls.Button internalBtnCancel;

        /// <summary>
        /// imgLogo control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected internal global::System.Web.UI.WebControls.Image imgLogo;

        /// <summary>
        /// imgIcon control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected internal global::System.Web.UI.WebControls.Image imgIcon;

        /// <summary>
        /// internalBtnOk control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected internal global::System.Web.UI.WebControls.Button internalBtnOk;

        /// <summary>
        /// lblDescription control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected internal global::System.Web.UI.WebControls.Label lblDescription;

        /// <summary>
        /// Overridden Render method solely to do registration for event validation
        /// </summary>
        /// <param name="writer">HtmlTextWriter object</param>
        protected override void Render(HtmlTextWriter writer)
        {
            PostBackOptions options = new PostBackOptions(this.btnDummyToPostBack);
            if (options != null)
            {
                options.PerformValidation = true;
                Page.ClientScript.RegisterForEventValidation(options);
                this.litPostBack.Text = String.Format("{0}{1}{2}", "<script type=\"text/javascript\" language=\"javascript\">function doPostBack() { ", Page.ClientScript.GetPostBackEventReference(options), "}</script>"); ;
            }
            base.Render(writer);
        }
    }
}
