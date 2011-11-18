<%@ WebHandler Language="C#" Class="add_poll" %>

using System;
using System.Web;

public class add_poll : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {

        context.Response.ContentType = "text/plain";
        
        string question = String.Empty;
        string answers = String.Empty;
        string urlId = String.Empty;
        string userId = context.Profile.GetPropertyValue("AnonUserId").ToString();
        int pageKey = 1;
        bool success = false;
        int pollKey = 0;
        //test

        if (context.Request.Form["q"] != null)
            question = context.Request.Form["q"].ToString().Trim().Replace("\r\n", " ").Replace("  ", " ");

        if (context.Request.Form["a"] != null)
            answers = context.Request.Form["a"].ToString().Trim();

        if (context.Request.Form["u"] != null)
            urlId = context.Request.Form["u"].ToString().Trim();

        unCCed.Page p = new unCCed.Page(urlId);


        // TODO create if user has access
        if (p.ID > 0)
        {
            pollKey = unCCed.DAL.PollDb.Add(question, answers, userId, p.ID);
            success = (pollKey > 0);
            
        }

        p = null;
        
        
        //TODO check for proper answers cnt > 1
        
        //TODO create user or fail ?


        context.Response.Write("{\"success\":" + success.ToString().ToLower() + ", \"pollId\":" + pollKey.ToString() + "}");
        
        
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}