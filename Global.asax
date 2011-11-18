<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

    void Application_BeginRequest(object sender, EventArgs e)
    {

        string fullOrigionalpath = Request.Url.ToString();
        int x = fullOrigionalpath.LastIndexOf("/");
        if (fullOrigionalpath.IndexOf(".",x) == -1)
        {
            
            string id = fullOrigionalpath.Substring(x+1, fullOrigionalpath.Length - x -1);
            if (id.Length > 8)
                Context.RewritePath("~/Page.aspx?id=" + id);
        }
        //// proper url = /Truck-{TruckKey}-{SEName}.aspx
        //if (fullOrigionalpath.ToLower().Contains("/truck-for-sale-"))
        //{
        //    string sTruckKey = "";
        //    int TruckKey = 0;
        //    int start = fullOrigionalpath.ToLower().IndexOf("/truck-for-sale-") + 16;
        //    int start2 = fullOrigionalpath.IndexOf("-", start);

        //    if (start2.Equals(-1))
        //    {
        //        start2 = fullOrigionalpath.IndexOf(".aspx", start);
        //    }

        //    sTruckKey = fullOrigionalpath.Substring(start, start2 - start);

        //    if (int.TryParse(sTruckKey, out TruckKey))
        //        Context.RewritePath(string.Format("~/Truck.aspx?id={0}", TruckKey.ToString()));
        //}
        //else if (fullOrigionalpath.ToLower().Contains("/content-"))
        //{
        //    string sKey = "";
        //    int Key = 0;
        //    int start = fullOrigionalpath.IndexOf("/content-") + 9;
        //    int start2 = fullOrigionalpath.IndexOf("-", start);

        //    if (start2.Equals(-1))
        //    {
        //        start2 = fullOrigionalpath.IndexOf(".aspx", start);
        //    }

        //    sKey = fullOrigionalpath.Substring(start, start2 - start);

        //    if (int.TryParse(sKey, out Key))
        //        Context.RewritePath(string.Format("~/Contact.aspx?id={0}", Key.ToString()));
        //}
        //else if (fullOrigionalpath.ToLower().Contains("/truck-part-"))
        //{
        //    string sTruckKey = "";
        //    int TruckKey = 0;
        //    int start = fullOrigionalpath.ToLower().IndexOf("/truck-part-") + 12;
        //    int start2 = fullOrigionalpath.IndexOf("-", start);

        //    if (start2.Equals(-1))
        //    {
        //        start2 = fullOrigionalpath.IndexOf(".aspx", start);
        //    }

        //    sTruckKey = fullOrigionalpath.Substring(start, start2 - start);

        //    if (int.TryParse(sTruckKey, out TruckKey))
        //        Context.RewritePath(string.Format("~/Part.aspx?id={0}", TruckKey.ToString()));
        //}
        //else if (fullOrigionalpath.ToLower().Contains("sitemap.xml"))
        //{
        //    Context.RewritePath("~/SiteMap.aspx");
        //}
    } 
       
</script>
