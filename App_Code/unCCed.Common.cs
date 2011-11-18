using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace unCCed
{
    /// <summary>
    /// Summary description for unCCed
    /// </summary>
    public class Common
    {
        public Common()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString; }
        }


        public static string SiteRootUrl
        {
            get
            {

                if (System.Configuration.ConfigurationSettings.AppSettings["SiteRootUrl"] != null)
                {
                    return System.Configuration.ConfigurationSettings.AppSettings["SiteRootUrl"].ToString();
                }
                else
                    return "http://unCCed.com/";

                
            }
        }
    }
}