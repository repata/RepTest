<%@ WebHandler Language="C#" Class="do_answer" %>

using System;
using System.Web;

public class do_answer : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {

        int pollKey = 0;
        string userId = context.Profile.GetPropertyValue("AnonUserId").ToString();
        int answer = 0;
        bool userIsAllowed = true;
        bool success = false;

        if (context.Request.Form["k"] != null)
            Int32.TryParse(context.Request.Form["k"].ToString(), out pollKey);

        if (context.Request.Form["a"] != null)
            Int32.TryParse(context.Request.Form["a"].ToString(), out answer);
        
        //TODO check if user is allowed to vote

        // return results
        string results = String.Empty;
        string totalVotes = String.Empty;
        if (pollKey > 0 && userIsAllowed && answer > 0)
        {
            success = unCCed.DAL.PollDb.SubmitAnswer(pollKey, userId, answer);

            unCCed.Poll p = new unCCed.Poll(pollKey);
            totalVotes = p.TotalVotes.ToString();
            unCCed.PollResultItem[] pr = p.Results;
            string fmt = "\"ix\":{0},\"answer\":\"{1}\",\"pct\":{2},\"votes\":{3}";
            int cnt = pr.Length;
            for (int i = 0; i < cnt; i++)
            {
                results += String.Concat("{", String.Format(fmt, pr[i].AnswerIndex, pr[i].Answer, Math.Round(pr[i].Percent,2), pr[i].Votes), "}");
                if (i < cnt - 1)
                    results += ",";
            }
        }
        

        context.Response.Write(String.Concat("{\"success\":", success.ToString().ToLower() , ", \"totalVotes\":",totalVotes,", \"results\":[" ,results, "]}"));
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}