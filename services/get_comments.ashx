<%@ WebHandler Language="C#" Class="get_comments" %>

using System;
using System.Web;
using System.Data;
using System.Text;

public class get_comments : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        HttpResponse Response = context.Response;
        HttpRequest Request = context.Request;
        Response.ContentType = "text/plain";
        Response.Expires = -100;

        int pageKey = 0;
        int lastCommentKey = 0;
        string urlId = String.Empty;
        string userId = context.Profile.GetPropertyValue("AnonUserId").ToString();
        int maxKey = 0;

        if (Request.Form["u"] != null)
            urlId = Request.Form["u"].ToString();


        if (Request.Form["lk"] != null)
            Int32.TryParse(Request.Form["lk"].ToString(), out lastCommentKey);
        
        string fmt = "\"id\":{0}, \"name\":\"{1}\", \"isUser\":{2}, \"time\":\"{3}\",\"comment\":\"{4}\"";
        //DataTable dt = unCCed.DAL.PageCommentDb.GetNew(urlId, lastCommentKey);

        StringBuilder sb = new StringBuilder();

        
        //foreach (DataRow dr in dt.Rows)
        //{
        //    sb.Append("{");
        //    sb.AppendFormat(fmt, dr["CommentKey"], dr["UsersName"], "true", getTime(DateTime.Parse(dr["DateCreated"].ToString())), System.Security.SecurityElement.Escape(dr["Comment"].ToString()));
        //    sb.Append("}");
        //}
        string comments = unCCed.PageComment.GetNewCommentsJsonFormatted(urlId, lastCommentKey, userId, out maxKey);
        sb.Append("{\"success\":true, \"lastcommentid\":" + maxKey.ToString() + "");
        sb.Append(", \"comments\":[");
        sb.Append(comments);
        sb.Append("]}");

        Response.Write(sb.ToString());
        
        
    }

    public string getTime(DateTime date)
    {
        string r = String.Empty;

        TimeSpan ts = DateTime.Now - date;

        if (ts.TotalSeconds < 60)
            return ts.TotalSeconds.ToString() + " secs ago";
        else if (ts.TotalMinutes < 60)
            return ts.TotalMinutes.ToString() + " mins ago";
        else if (ts.TotalHours < 24)
            return ts.TotalHours.ToString() + " hours ago";
        else
            return ts.TotalDays.ToString() + " days ago";
        
       
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}