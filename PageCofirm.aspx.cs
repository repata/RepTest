using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PageCofirm : System.Web.UI.Page
{
    protected  string urlId = String.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["u"] != null)
        {
            urlId = Request.QueryString["u"].ToString();
            //TODO check exists

            //lbl_newlink.Text = "<a href=\"http://unCCed.com/" + urlId + "\">unCCed.com/" + urlId + "</a>";
            lbl_newlink.Text = "unCCed.com/" + urlId;
        }
        else
        {
            Response.Redirect("~/Default.aspx");
        }
    }
    protected void btn_continue_Click(object sender, EventArgs e)
    {
        string catCode = rbl_purpose.SelectedValue;

        if (catCode.Length == 2)
        {
            unCCed.DAL.DbCommon.executeNonSql("UPDATE dbo.Page SET CategoryCode = '" + catCode + "' WHERE urlId = '" + urlId + "'");

            Response.Redirect("~/" + urlId);
        }
    }
}
