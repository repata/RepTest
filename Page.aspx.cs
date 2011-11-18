using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class Page : System.Web.UI.Page
{
    protected string lastCommentKey = "0";
    protected string urlId = String.Empty;
    protected string usersName = String.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        //string urlId = String.Empty;
        bool requiresPassword = false;
        int pageKey = 0;
        string userId = String.Empty;
        if (Request.QueryString["id"] != null)
            urlId = Request.QueryString["id"].ToString();

        //TODO security check

        lbl_test.Text = Profile.AnonUserId + " " + Profile.UserName;
        
        
        if (urlId.Length > 8)
        {
            unCCed.Page p = new unCCed.Page(urlId);
            if (p.ID > 0)
            {
                if (p.PagePassword.Length > 0)
                    requiresPassword = true;

                lbl_title.Text = p.Title;
                TimeSpan ts = p.DateExpires - DateTime.Now;
                int expireIn = (int)Math.Floor(ts.TotalDays);
                
                // check for expiration
                if (p.DateExpires < DateTime.Now)
                {
                    Response.Redirect("~/Expired.aspx?u=" + urlId);
                }
                else
                {
                    string expires = String.Empty;

                    if (expireIn == 1)
                    {
                        expires = ", <span style=\"color:red\">Page Expires Tomorrow</span>";
                    }
                    else
                    {
                        expires = ", Page Expires in " + expireIn.ToString() + " days";
                    }

                    //lbl_host.Text = "Host: " + p.DisplayName + ", Page Expires in " + p.DateExpires.ToString("ddd, M/d/yyyy");
                    lbl_host.Text = "Host: " + p.DisplayName + expires;
                    Page.Title = "unCCed - " + p.Title;
                    usersName = Profile.Name;

                    if (!requiresPassword)
                    {
                        if (String.IsNullOrEmpty(Profile.AnonUserId))
                        {
                            userId = unCCed.DAL.PageDb.CreateBlankUser(p.ID);
                            Profile.AnonUserId = userId;
                            lbl_test.Text = Profile.AnonUserId;
                        }
                        else
                        {
                            userId = Profile.AnonUserId;
                        }

                        p.MarkUserViewed(userId);

                        int maxKey = 0;
                        string html = unCCed.PageComment.GetNewCommentsHtmlFormatted(p.ID, 0, userId, out maxKey);

                        lastCommentKey = maxKey.ToString();
                        lit_msgs.Text = html;
                        lbl_usercount.Text = p.UserCount.ToString() + " unCCers (commenters)";
                        if (p.OriginalContent.Length > 0)
                        {
                            string htmlOrig = "<div id=\"origmsgs\" style=\"margin-top:18px;padding:12px;border:solid 2px #ADD8E6;background-color:#e1f1fa;clear:left;\"><div style=\"border:solid 1px #ADD8E6;padding:1px;margin-bottom:12px;\">";
                            htmlOrig += "<div style=\"padding:4px;font-weight:bold;font-size:12pt;background-color:#87CEFA\">Original Content from Email Chain posted by " + p.DisplayName + "</div></div>";
                            htmlOrig += p.OriginalContent.Replace("\n", "<br/>") + "</div>";

                            lit_origmsgs.Text = htmlOrig;
                        }

                        // do rss urls
                        string rssUrl = unCCed.Common.SiteRootUrl + "rss.ashx?u=" + urlId;
                        fhl_xml.HRef = rssUrl;
                        fhl_google.HRef = "http://fusion.google.com/add?source=atgs&feedurl=" + Server.UrlEncode(rssUrl);
                        fhl_yahoo.HRef = "http://add.my.yahoo.com/rss?url=" + Server.UrlEncode(rssUrl);
                        fhl_aol.HRef = "http://feeds.my.aol.com/add.jsp?url=" + Server.UrlEncode(rssUrl);
                        fhl_msn.HRef = "http://my.msn.com/addtomymsn.armx?id=rss&amp;ut=" + Server.UrlEncode(rssUrl);

                        System.Web.UI.HtmlControls.HtmlLink rssLink = new System.Web.UI.HtmlControls.HtmlLink();
                        rssLink.Attributes.Add("type", "application/rss+xml");
                        rssLink.Attributes.Add("rel", "alternate");
                        rssLink.Attributes.Add("title", p.Title);
                        rssLink.Attributes.Add("href", rssUrl);
                        Page.Header.Controls.Add(rssLink);


                        // get all unccers for the page
                        string unccersFmt = "<div style=\"margin-bottom:2px;\">{2}<a href=\"javascript:void(0);\" onclick=\"hilitUser('{1}');\">{0}</a></div>";
                        string unccers = String.Empty;
                        foreach (unCCed.AnonUser au in p.GetUnccers())
                        {
                            string img = "";
                            if (au.hasCommented)
                                img = "<img src=\"media/icons/user_comment.png\" align=\"absmiddle\" style=\"margin-right:4px;\" alt=\"This unCCer has commented.\" />";
                            else if (au.hasViewed)
                                img = "<img src=\"media/icons/eye.png\" align=\"absmiddle\" style=\"margin-right:4px;\" alt=\"This unCCer has only viewed this page.\" />";

                            unccers += String.Format(unccersFmt, au.DisplayName, au.ID, img);
                        }
                        lit_unccers.Text = unccers;

                        doPoll(p.ID, userId);
                    }
                }
            }


        }



    }

    protected void doPoll(int pageKey, string userId)
    {
        //int pollKey = 10001;

        DataTable dt = unCCed.DAL.PollDb.GetAllForPage(pageKey, userId);

        string html = String.Empty;
        foreach (DataRow dr in dt.Rows)
        {
            string[] answers = Regex.Split(dr["Answers"].ToString(), "\n");
            string ans = String.Empty;
            string pollKey = dr["PollKey"].ToString();
            html += "<div id=\"poll-" + pollKey + "\" class=\"poll\"><p>" + dr["Question"].ToString() + "</p><div id=\"poll-" + pollKey + "-a\">";
            if ((int)dr["UserAnswered"] == 0)
            {
                html += "<ul>";
                for (int i = 0; i < answers.Length; i++)
                {
                    html += "<li><input name=\"polla-" + dr["PollKey"].ToString() + "\" type=\"radio\" value=\"" + (i + 1).ToString() + "\"/>" + answers[i] + "</li>";
                }
                html += "</ul>";
                html += "</div><div class=\"btns\"><button type=\"button\" onclick=\"answerPoll(" + pollKey + ")\" value=\"Submit\">Submit</button></div></div>";
            }
            else
            {
                unCCed.Poll p = new unCCed.Poll((int)dr["PollKey"]);
                int totalVotes = p.TotalVotes;
                unCCed.PollResultItem[] pr = p.Results;
                html += "<table width=\"100%\"><tr><td colspan=\"2\">Total Votes: " + totalVotes.ToString() + "</td></tr>";
                int cnt = pr.Length;
                for (int i = 0; i < cnt; i++)
                {
                    double pct = Math.Round(pr[i].Votes / (double)p.TotalVotes,2) * 100;
                    html += "<tr><td>" + pr[i].Answer +"</td><td rowspan=\"2\">" + pct.ToString() + "% (" + pr[i].Votes.ToString() + ")</td></tr>";
                    html += "<tr><td><div style=\"background-color:silver;width:" + pct.ToString() + "%;height:4px;\"></div></td></tr>";
                }
                html += "</table>";
                html += "</div><div class=\"btns\"></div></div>";
            }
            
            //context.Response.Write(String.Concat("{", String.Format(fmt, dt.Rows[0]["Question"], ans), "}"));
        }
        lit_poll.Text = html;
    }

    protected void Invalid()
    {
        Response.Redirect("~/Default.aspx");
    }
}
