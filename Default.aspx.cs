using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Data;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!String.IsNullOrEmpty(Profile.AnonUserId))
            {
                unCCed.AnonUser au = new unCCed.AnonUser(Profile.AnonUserId);
                if (!String.IsNullOrEmpty(au.ID))
                    tb_email.Text = au.Email;
            }

            DataTable dt = unCCed.DAL.DbCommon.executeSql("SELECT p.UrlId, p.Title FROM dbo.Page p ORDER BY PageKey DESC");
            string html = String.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                html += "<a href=\"" + dr["UrlId"].ToString() + "\">" + dr["Title"].ToString() + "</a><br/>";
            }
            lit_open.Text = html;

        }
        lbl_debug.Text = "AnonUserId: " + Profile.AnonUserId;
    }
    
    protected void btn_findem_Click(object sender, EventArgs e)
    {
        string email = tb_findem.Text.Trim();
        email = email.Replace("--","").Replace("/*","");
        
        // check if user exists

        if (unCCed.DAL.AnonUserDb.DoesExist(email))
        {

            DataTable dt = unCCed.DAL.DbCommon.executeSql("SELECT DISTINCT p.UrlId, p.Title, au.Email, p.DateExpires FROM dbo.Page p INNER JOIN dbo.AnonUser au ON p.OwnerUserId = au.AnonUserId INNER JOIN dbo.PageUser pu ON p.PageKey = pu.PageKey INNER JOIN dbo.AnonUser pau ON pu.AnonUserId = pau.AnonUserId WHERE pau.Email = '" + email + "' AND GETDATE() < p.DateExpires ORDER BY p.DateExpires ASC");
            string html = String.Empty;
            bool sent = false;
            if (dt.Rows.Count > 0)
            {
                html = "Thanks for using unCCed.com!  Here are your requested unCCed links.  If you did not request this, please let us know.<table><tr><td>Title</td><td>Host</td><td>Expires On</td></tr>";
                string fmt = "<tr><td><a href=\"{0}{1}\">{2}</a></td><td>{3}</td><td>{4}</tr>";
                foreach (DataRow dr in dt.Rows)
                {
                    html += String.Format(fmt, unCCed.Common.SiteRootUrl, dr["UrlId"], dr["Title"], dr["Email"], Convert.ToDateTime(dr["DateExpires"].ToString()).ToShortDateString());
                }
                html += "</table>";
                MailMessage m = new MailMessage("ctrailer@repata.com", email);
                m.Subject = "Your unCCed pages on " + DateTime.Today.ToShortDateString();
                m.Body = html;
                m.IsBodyHtml = true;

                SmtpClient s = new SmtpClient();
                try
                {
                    s.Send(m);
                    sent = true;
                }
                catch
                {
                    //todo error handle
                }
                if (sent)
                    lbl_finem_msg.Text = "We sent an email containing links to all your pages.  Remember to add uncced@repata.com to your Safe Sender's list.";
                else
                    lbl_finem_msg.Text = "Unfortunately, we had an issue sending your email.  Please try again.";

            }
            else
            {
                lbl_finem_msg.Text = "There are no active pages for that email address.";
            }
            
        }
        else
        {
            lbl_finem_msg.Text = "That email does not exist.";
        }
    }
    
    protected void btn_create_Click(object sender, EventArgs e)
    {
        string title = tb_title.Text.Trim();
        string pw = tb_pw.Text.Trim();
        string userId = Guid.Empty.ToString();
        string email = tb_email.Text.Trim();
        
        string urlId = unCCed.Utilities.GenerateRandomString(10);

        string eUserId = unCCed.AnonUser.getUserIdFromEmail(email);

        if (!String.IsNullOrEmpty(Profile.AnonUserId))
        {
            if (eUserId == Profile.AnonUserId)
            {
                unCCed.AnonUser au = new unCCed.AnonUser(Profile.AnonUserId);
                if (!String.IsNullOrEmpty(au.ID))
                {
                    if (au.Email.ToLower() != email.ToLower())
                    {
                        // update profile's email
                        au.Email = email;
                        au.Update();
                    }
                }
            }
            else
            {
                Profile.AnonUserId = eUserId;
            }
        }
        else
        {
            unCCed.AnonUser au = new unCCed.AnonUser();
            au.Email = email;
            if (au.Add())
            {
                Profile.AnonUserId = au.ID;
            }
        }


        int pageKey = unCCed.DAL.PageDb.AddViaEmail(title, urlId, email, pw);

        if (pageKey > 0)
        {
            string link = unCCed.Common.SiteRootUrl + urlId;
            string linkname = "unCCed.com/" + urlId;
            string pwtr = String.Empty;
            if (pw.Length > 0)
            {
                pwtr = "<tr><td>Password:</td><td>" + pw + "</td></tr>";
            }

            MailMessage m = new MailMessage("ctrailer@repata.com", email);
            m.Subject = "Your new unCCed page link for " + title;
            m.Body = String.Format("<p>Thanks for using unCCed.com!</p>Here is the information about your new page.<table><tr><td>Link:</td><td><a href=\"{0}\">{1}</a></td></tr><tr><td>Title:</td><td>{2}</td></tr>{3}</table>", link, linkname, title, pwtr);
            m.IsBodyHtml = true;

            SmtpClient s = new SmtpClient();
            try
            {
                s.Send(m);
            }
            catch
            {
                //todo error handle
            }

            Response.Redirect("~/PageCofirm.aspx?u=" + urlId);
        }
    }
}
