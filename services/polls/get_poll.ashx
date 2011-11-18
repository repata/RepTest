<%@ WebHandler Language="C#" Class="get_poll" %>

using System;
using System.Web;
using System.Data;
using System.Text.RegularExpressions;

public class get_poll : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

        int pollKey = 10001;

        DataTable dt = unCCed.DAL.PollDb.Get(pollKey);

        string fmt = "\"question\":\"{0}\", \"answers\":[{1}]";
        
        if (dt.Rows.Count == 1)
        {
            string[] answers = Regex.Split(dt.Rows[0]["Answers"].ToString(), "\n");
            string ans = String.Empty;
            
            for (int i = 0;i<answers.Length;i++)
            {
                ans += "\"" + answers[i] + "\"";
                if (i < answers.Length - 1)
                    ans += ",";
            }

            context.Response.Write(String.Concat("{",String.Format(fmt, dt.Rows[0]["Question"], ans),"}"));
        }
        
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}