<%@ WebHandler Language="C#" Class="send_invites" %>

using System;
using System.Web;
using System.Net.Mail;

public class send_invites : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        HttpResponse Response = context.Response;
        HttpRequest Request = context.Request;
        Response.ContentType = "text/plain";
        Response.Expires = -100;

        string urlId = Request.Form["u"].ToString();
        
        string emails = String.Empty;
        string usersName = String.Empty;
        string usersEmail = String.Empty;
        string userId = context.Profile.GetPropertyValue("AnonUserId").ToString();
        string title = String.Empty;
        string link = String.Empty;
        string pwstr = String.Empty;
        string msg = String.Empty;
        int pageKey = 0;
        bool success = false;
        int cnt = 0;
        string subject = "You have been unCCed by ";
        
        unCCed.Page p = new unCCed.Page(urlId);
        if (p.ID > 0)
        {
            pageKey = p.ID;
            title = p.Title;
            link = unCCed.Common.SiteRootUrl + p.UrlId;
            if (!String.IsNullOrEmpty(p.PagePassword))
            {
                pwstr = "<br/>Password: " + p.PagePassword + "\n";
            }
            emails = Request.Form["emls"].ToString();
            subject += context.Profile.GetPropertyValue("Name");

            string msgFmt = "<p>Join the conversation on the \"{0}\" page available at:</p><p>Link: <a href=\"{1}\">{1}</a>{2}</p><p>unCCed.com couldn't be easier to use.  Simply click on the link above (or copy and paste for you scaredy cats).</p>";

            string[] emailAr = emails.Split(',');
            
            foreach (string email in emailAr)
            {
                if (unCCed.Utilities.checkEmail(email))
                {
                    //TODO check if already a page user

                    MailMessage m = new MailMessage("ctrailer@repata.com", email);
                    m.Subject = subject;
                    m.Body = String.Format(msgFmt, title, link, pwstr);
                    m.IsBodyHtml = true;

                    SmtpClient s = new SmtpClient();
                    try
                    {
                        s.Send(m);
                        unCCed.DAL.PageDb.AddPageInvite(pageKey, userId, email);
                        cnt++;
                    }
                    catch
                    {
                        //todo error handle
                    }
                }
            }
            if (cnt == emailAr.Length)
                success = true;
            else
            {
                msg = "Some emails were not sent.";  
            }
        }
        Response.Write("{\"success\":" + success.ToString().ToLower() + ", \"sent\":" + cnt.ToString() + ", \"msg\":\"" + msg + "\"}");
        
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}