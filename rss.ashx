<%@ WebHandler Language="C#" Class="rss" %>

using System;
using System.Web;
using System.ServiceModel.Syndication;
using System.Data;
using System.Xml;
using System.IO;

public class rss : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        string urlId = String.Empty;
        
        if (context.Request.QueryString["u"] != null)
        {
            urlId = context.Request.QueryString["u"].ToString();
            
            //check if exists
            unCCed.Page p = new unCCed.Page(urlId);
            if (p.ID > 0)
            {
                
                // check for only public feeds 
                //TODO or create unique security key for user
                if (!p.isPrivate)
                {
                    if (p.DateExpires >= DateTime.Now)
                    {
                        SyndicationFeed f = new SyndicationFeed();
                        f.Title = new TextSyndicationContent(p.Title);
                        f.Description = new TextSyndicationContent("An unCCed.com micro forum");
                        f.ImageUrl = new Uri(unCCed.Common.SiteRootUrl + "media/images/logo1a.png");

                        f.Items = p.GetCommentItems();

                        var output = new StringWriter();
                        var writer = new XmlTextWriter(output);

                        new Rss20FeedFormatter(f).WriteTo(writer);

                        context.Response.ContentType = "application/rss+xml";
                        context.Response.Write(output.ToString());

                    }
                    else
                    {
                        doExpires(context);
                    }
                }
                else
                {
                    doPrivate(context);
                }
                
            }
            else
                doInvalid(context);
            
            
            
        }
        else
            doInvalid(context);
        
    }
    public void doExpires(HttpContext context)
    { 
        
        context.Response.StatusCode = 402;
        context.Response.Status = "402 Payment Required";
        context.Response.Write("The page has expired.  You can reinstate it by...");
        context.Response.End();
    }
    public void doPrivate(HttpContext context)
    {
        context.Response.StatusCode = 401;
        context.Response.Status = "401 Unauthorized";
        context.Response.Write("Login or Key Required");
        context.Response.End();
    }
    
    public void doInvalid(HttpContext context)
    {
        context.Response.StatusCode = 404;
        context.Response.Status = "404 Not Found";
        context.Response.Write("Resource Not Found");
        context.Response.End();
    }

    //private SyndicationFeed CreateFeed(string urlId) { 
        
    //}
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}