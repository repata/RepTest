using System;
using System.Collections.Generic;
using System.Data;

namespace unCCed
{
    /// <summary>
    /// Summary description for unCCed
    /// </summary>
    public class AnonUser
    {
        public AnonUser()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public AnonUser(string userId)
        {
            DataTable dt = DAL.AnonUserDb.Get(userId);
            if (dt.Rows.Count == 1) {
                _id = userId;
                _name = dt.Rows[0]["Name"].ToString();
                _email = dt.Rows[0]["Email"].ToString();
            }
        }

        public static string getUserIdFromEmail(string email) {
            return DAL.AnonUserDb.GetAnonUserIdFromEmail(email);
        }

        private string _id = String.Empty;
        private string _name = String.Empty;
        private string _email = String.Empty;
        DateTime _lastComment = new DateTime(1973,1,1);
        DateTime _lastViewed = new DateTime(1973, 1, 1);

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public bool isAnonymous
        {
            get {
                return (String.IsNullOrEmpty(_name) && String.IsNullOrEmpty(_email));
            }
        }
        public string DisplayName
        {
            get {
                if (isAnonymous)
                    return "[Anonymous]";
                else if (String.IsNullOrEmpty(_name))
                    return _email;
                else
                    return _name;
            }
        }
        public bool hasCommented
        {
            get {
                return _lastComment > new DateTime(1973, 1, 1);
            }
        }

        public bool hasViewed
        {
            get
            {
                return _lastViewed > new DateTime(1973, 1, 1);
            }
        }
        public DateTime LastComment
        {
            get { return _lastComment; }
            set { _lastComment = value; }
        }
        public DateTime LastViewed
        {
            get { return _lastViewed; }
            set { _lastViewed = value; }
        }

        public bool Add()
        {
            bool _ret = false;
            if (_email.Length > 0 || _name.Length > 0)
            {
                string id = DAL.AnonUserDb.Add(_name, _email);
                if (!String.IsNullOrEmpty(id))
                {
                    _id = id;
                    _ret = true;
                }

            }
            return _ret;
        }

        public bool Update()
        {
            return DAL.AnonUserDb.Update(_id, _name, _email);
        }
    }
}