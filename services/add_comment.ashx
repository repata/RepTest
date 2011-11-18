<%@ WebHandler Language="C#" Class="add_comment" %>

using System;
using System.Web;

public class add_comment : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        HttpResponse Response = context.Response;
        HttpRequest Request = context.Request;
        Response.ContentType = "text/plain";
        Response.Expires = -100;

        //System.Threading.Thread.Sleep(1000);
        
        if (Request.Form["u"] != null)
        {
            string urlId = Request.Form["u"].ToString();
            string usersName = Request.Form["un"].ToString();
            string userId = context.Profile.GetPropertyValue("AnonUserId").ToString();
            string comment = Request.Form["cmt"].ToString();
            int lastCommentKey = 0;
            
            if (Request.Form["lk"] != null)
                Int32.TryParse(Request.Form["lk"].ToString(), out lastCommentKey);
            
            int commentKey = unCCed.DAL.PageCommentDb.Add(urlId, userId, usersName, comment);

            if (commentKey > 0)
            {
                context.Profile.SetPropertyValue("Name", usersName);
                unCCed.AnonUser au = new unCCed.AnonUser(userId);
                if (au.Name != usersName)
                {
                    au.Name = usersName;
                    au.Update();
                }
                int maxKey = 0;
                string comments = unCCed.PageComment.GetNewCommentsJsonFormatted(urlId, lastCommentKey, userId, out maxKey);
                string ret = "{\"success\":true, \"msg\":\"\", \"lastcommentid\":" + maxKey.ToString() + ", \"comments\":[" + comments + "]}";
                Response.Write(ret);
            }
        }
        else
        {
            Response.Write("{\"success\":false, \"msg\":\"Invalid Request.\"}");    
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}