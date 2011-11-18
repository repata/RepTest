using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
namespace unCCed
{

    /// <summary>
    /// Summary description for unCCed
    /// </summary>
    public class PageComment
    {
        public PageComment()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static string GetNewCommentsHtmlFormatted(int pageKey, int lastCommentKey, string requestingUserId, out int maxKey)
        {
            maxKey = 0;
            DataTable dt = unCCed.DAL.PageCommentDb.GetNew(pageKey, lastCommentKey, requestingUserId); 
            int rows = dt.Rows.Count;
            int cnt = 0;
            StringBuilder sb = new StringBuilder();

            string fmt = "<div id=\"usr-{6}\"><div style=\"float:left;width:120px;\"><div class=\"{3}\">{0}</div><div id=\"scs-{4}\" class=\"cmt-time\">{1}</div></div><div class=\"{5} cmt-shell-lf\">{2}</div></div>";
            foreach (DataRow dr in dt.Rows)
            {
                cnt++;
                string cmt_shell = "cmt-shell";
                if ((int)dr["CommentKey"] > maxKey)
                    maxKey = (int)dr["CommentKey"];
                string userCls = "un-anon";
                if ((int)dr["isRegistered"] == 1)
                    userCls = "un-star";


                if ((int)dr["isMine"] == 1)
                    cmt_shell = "cmt-shell-mine";    
                //sb.AppendFormat(fmt, dr["UsersName"], unCCed.Utilities.getTime(DateTime.Parse(dr["DateCreated"].ToString())), System.Security.SecurityElement.Escape(dr["Comment"].ToString()));
                sb.AppendFormat(fmt, dr["UsersName"], unCCed.Utilities.getTime(Convert.ToInt32(dr["Secs"].ToString())), System.Security.SecurityElement.Escape(dr["Comment"].ToString()), userCls, dr["Secs"].ToString(), cmt_shell, dr["UserId"]);
            }
            return sb.ToString();
        }


        public static string GetNewCommentsJsonFormatted(int pageKey, int lastCommentKey, string requestingUserId, out int maxKey)
        {
            maxKey = 0;
            DataTable dt = unCCed.DAL.PageCommentDb.GetNew(pageKey, lastCommentKey, requestingUserId);
            return formatCommentsJson(dt, out maxKey);
        }


        public static string GetNewCommentsJsonFormatted(string urlId, int lastCommentKey, string requestingUserId, out int maxKey)
        {
            maxKey = 0;
            DataTable dt = unCCed.DAL.PageCommentDb.GetNew(urlId, lastCommentKey, requestingUserId);
            return formatCommentsJson(dt, out maxKey);
        }

        private static string formatCommentsJson(DataTable dt, out int maxKey)
        {

            maxKey = 0;
            StringBuilder sb = new StringBuilder();
            string fmt = "\"id\":{0}, \"isMine\":{6}, \"name\":\"{1}\", \"isUser\":{2}, \"time\":\"{3}\",\"comment\":\"{4}\", \"date\":\"{5}\"";
            int rows = dt.Rows.Count;
            int cnt = 0;
            foreach (DataRow dr in dt.Rows)
            {
                cnt++;
                if ((int)dr["CommentKey"] > maxKey)
                    maxKey = (int)dr["CommentKey"];
                bool isRegistered = false;
                if ((int)dr["isRegistered"] == 1)
                    isRegistered = true;

                bool isMine = false;
                if ((int)dr["isMine"] == 1)
                    isMine = true;


                sb.Append("{");
                //sb.AppendFormat(fmt, dr["CommentKey"], dr["UsersName"], "true", Utilities.getTime(DateTime.Parse(dr["DateCreated"].ToString())), System.Security.SecurityElement.Escape(dr["Comment"].ToString().Replace("\n"," ")));
                sb.AppendFormat(fmt, dr["CommentKey"], dr["UsersName"], isRegistered.ToString().ToLower(), dr["Secs"].ToString(), System.Security.SecurityElement.Escape(dr["Comment"].ToString().Replace("\n", " ")), dr["DateCreated"].ToString(), isMine.ToString().ToLower());
                sb.Append("}");

                if (cnt < rows)
                    sb.Append(",");
            }
            return sb.ToString();
        }
    }
}