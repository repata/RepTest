using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.ServiceModel.Syndication;

namespace unCCed
{
    /// <summary>
    /// Summary description for unCCed
    /// </summary>
    public class Page
    {
        public Page()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public Page(int pageKey)
        {
            DataTable dt = unCCed.DAL.PageDb.Get(pageKey);
            if (dt.Rows.Count == 1)
            {
                setProps(dt.Rows[0]);
            }
        }
        
        public Page(string urlId)
        {
            DataTable dt = unCCed.DAL.PageDb.Get(urlId);
            if (dt.Rows.Count == 1)
            {
                setProps(dt.Rows[0]);
                
            }
        }
        public Page(DataRow dr)
        {
            setProps(dr);
        }

        private void setProps(DataRow dr)
        {
            _id = (int)dr["PageKey"];
            _urlId = dr["UrlId"].ToString();
            _title = dr["Title"].ToString();
            _ownerUserId = dr["OwnerUserId"].ToString();
            _pagePassword = dr["PagePassword"].ToString();
            _dateExpires = Convert.ToDateTime(dr["DateExpires"].ToString());
            _owneremail = dr["OwnerEmail"].ToString();
            _ownername = dr["OwnerName"].ToString();
            _showCalendar = Convert.ToBoolean(dr["ShowCalendar"].ToString());
            _userCount = Convert.ToInt32(dr["UserCount"].ToString());
            _categoryCode = dr["CategoryCode"].ToString();
            if (!dr.IsNull("Content"))
            {
                _origContent = dr["Content"].ToString();
            }
        }

        public static bool AddOrigContent(int pageKey, string content)
        {
            return unCCed.DAL.PageDb.AddOrigContent(pageKey, content);
        }

        public static bool AddPageUser(int pageKey, string email)
        {
            return false;
        }

        private int _id = 0;
        private string _ownerUserId  = String.Empty;
        private string _title = String.Empty;
        private string _urlId = String.Empty;
        private string _pagePassword = String.Empty;
        private string _ownername = String.Empty;
        private string _owneremail = String.Empty;
        private bool _showCalendar = false;
        private DateTime _dateExpires = DateTime.Now;
        private int _userCount = 0;
        private string _origContent = String.Empty;
        private int _lastCommentId = 0;
        private string _categoryCode = "X";

        public int ID
        {
            get { return _id; }
        }

        public string OwnerUserId
        {
            get { return _ownerUserId; }
        }
        public string Title
        {
            get { return _title; }
        }
        public string UrlId
        {
            get { return _urlId; }
        }
        public string PagePassword
        {
            get { return _pagePassword; }
        }

        public string OwnerName
        {
            get { return _ownername; }
        }

        public string OwnerEmail
        {
            get { return _owneremail; }
        }

        public DateTime DateExpires
        {
            get { return _dateExpires; }
        }

        public bool isPrivate
        {
            get {
                return _pagePassword.Length > 0;
            }
        }

        public string DisplayName {
            get {
                if (String.IsNullOrEmpty(_ownername))
                    return _owneremail;
                else
                    return _ownername;
            }
        }
        public int UserCount
        {
            get { return _userCount; }
        }

        public string OriginalContent
        {
            get { return _origContent; }
        }

        public int LastCommentID
        {
            get { return _lastCommentId; }
        }
        public string CategoryCode
        {
            get { return _categoryCode; }
        }

        public List<SyndicationItem> GetCommentItems()
        {
            if (_id > 0)
            { 
                List<SyndicationItem> list = new List<SyndicationItem>();
                DataTable dt = DAL.PageCommentDb.GetNew(_id, _lastCommentId);
                foreach (DataRow dr in dt.Rows)
                {
                    string comment = dr["Comment"].ToString();
                    var title = (comment.Length > 40 ? comment.Substring(0,40) : comment);
                    list.Add(new SyndicationItem(title, null, null) { Title = new TextSyndicationContent(comment) });
                }
                return list;
            }
            else
                return null;
        }
        public DataTable GetComments()
        {
            if (_id > 0)
                return DAL.PageCommentDb.GetNew(_id, _lastCommentId);
            else
                return null;
        }

        public DataTable GetComments(int lastCommentKey)
        {
            if (_id > 0)
                return DAL.PageCommentDb.GetNew(_id, lastCommentKey);
            else
                return null;
        }

        public List<AnonUser> GetUnccers() {
            List<AnonUser> list = new List<AnonUser>();

            if (_id > 0)
            {
                DataTable dt = DAL.AnonUserDb.GetAllForPage(_id);
                foreach (DataRow dr in dt.Rows)
                {
                    AnonUser au = new AnonUser();
                    au.Name = dr["Name"].ToString();
                    au.Email = dr["Email"].ToString();
                    au.ID = dr["AnonUserId"].ToString();
                    if (!dr.IsNull("DateLastComment"))
                        au.LastComment = Convert.ToDateTime(dr["DateLastComment"].ToString());
                    if (!dr.IsNull("DateLastViewed"))
                        au.LastViewed = Convert.ToDateTime(dr["DateLastViewed"].ToString());

                    list.Add(au);
                }
            }
            return list;
        }

        public static List<Page> GetActiveForUser(string userId)
        {
            List<Page> list = new List<Page>();

            DataTable dt = DAL.PageDb.GetActiveForUser(userId);
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new Page(dr));
            }

            return list;
        }

        

        public bool MarkUserViewed(string userId)
        {
            if (_id > 0)
            {
                return DAL.PageDb.AddUserViewed(_id, userId);
            }
            else
            {
                return false;
            }
        }
    }
}