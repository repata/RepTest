using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace unCCed.DAL
{
    public class DbCommon
    {
        public static string lastMessage = String.Empty;

        public static DataTable executeSql(string tsql)
        {
            DataTable dt = new DataTable();
            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand(tsql, cn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            try
            {
                cn.Open();
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                //Audit.Event(0, "DAL.executeSql", ex.Message + "; " + tsql);
                //errorCount++;
                lastMessage = ex.Message;
            }
            finally
            {
                cmd.Dispose();
                cn.Dispose();
            }

            return dt;
        }

        public static bool executeNonSql(string tsql)
        {
            bool ret = false;
            SqlConnection cn = new SqlConnection(Common.ConnectionString);

            SqlCommand cmd = new SqlCommand(tsql, cn);
            cmd.CommandTimeout = 600;
            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                ret = true;
            }
            catch (Exception ex)
            {
                //Audit.Event(0, "DAL.executeNonSql", ex.Message + "; " + tsql);
                //errorCount++;
                ret = false;
                lastMessage = ex.Message;
            }
            finally
            {
                cmd.Dispose();
                cn.Dispose();
            }

            return ret;
        }
    }
    public class AnonUserDb
    {
        public static DataTable Get(string userId)
        {   
            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.AnonUser WHERE AnonUserId = @UserId", cn);
            //cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@UserId", userId));
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            cn.Open();
            da.Fill(dt);
            cmd.Dispose();
            cn.Dispose();
            return dt;
        }

        public static DataTable GetAllForPage(int pageKey)
        {
            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT au.*, CASE WHEN LEN(au.Email) + LEN(au.Name) = 0 THEN 1 ELSE 0 END as isAnonymous, pu.DateLastComment, pu.DateLastViewed FROM AnonUser au INNER JOIN dbo.PageUser pu ON au.AnonUserId = pu.AnonUserId WHERE pu.PageKey = @PageKey ORDER BY pu.DateLastComment DESC, pu.DateLastViewed DESC", cn);
            //cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@PageKey", pageKey));
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            cn.Open();
            da.Fill(dt);
            cmd.Dispose();
            cn.Dispose();
            return dt;
        }



        public static bool DoesExist(string email)
        {
            bool ret = false;

            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.rsp_AnonUser_Exists", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Email", email));

            SqlParameter prm = cmd.Parameters.Add("@DoesExist", SqlDbType.Bit);
            prm.Direction = ParameterDirection.Output;
            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                ret = (bool)prm.Value;
            }
            catch (Exception ex)
            {
                //TODO audit
                //Audit.Event(0, "DAL.PageDb.AddPage", ex.Message);
                ret = false;
            }
            finally
            {
                cmd.Dispose();
                cn.Dispose();
            }

            return ret;
        }
        public static string GetAnonUserIdFromEmail(string email)
        {
            string ret = String.Empty;

            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.rsp_AnonUser_GetIdFromEmail", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Email", email));


            SqlParameter prm = cmd.Parameters.Add("@AnonUserId", SqlDbType.UniqueIdentifier);
            prm.Direction = ParameterDirection.Output;

            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                ret = prm.Value.ToString();

            }
            catch (Exception ex)
            {
                //TODO audit
                //Audit.Event(0, "DAL.PageDb.AddPage", ex.Message);
                ret = String.Empty;
            }
            finally
            {
                cmd.Dispose();
                cn.Dispose();
            }

            return ret;
        }


        public static bool Update(string userId, string name, string email)
        {
            bool _ret = false;
            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("UPDATE dbo.AnonUser SET Name = @Name, Email = @Email WHERE AnonUserId = @UserId", cn);
            //cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@UserId", userId));
            cmd.Parameters.Add(new SqlParameter("@Name", name));
            cmd.Parameters.Add(new SqlParameter("@Email", email));



            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                //ret = prm.Value.ToString();
                _ret = true;

            }
            catch (Exception ex)
            {
                //TODO audit
                //Audit.Event(0, "DAL.PageDb.AddPage", ex.Message);
                //ret = String.Empty;
            }
            finally
            {
                cmd.Dispose();
                cn.Dispose();
            }

            return _ret;
        }
        public static string Add(string name, string email)
        {
            string _ret = String.Empty;
            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("rsp_AnonUser_Add", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Name", name));
            cmd.Parameters.Add(new SqlParameter("@Email", email));


            SqlParameter prm = cmd.Parameters.Add("@AnonUserId", SqlDbType.UniqueIdentifier);
            prm.Direction = ParameterDirection.Output;

            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                _ret = prm.Value.ToString();
               

            }
            catch (Exception ex)
            {
                //TODO audit
                //Audit.Event(0, "DAL.PageDb.AddPage", ex.Message);
                _ret = String.Empty;
            }
            finally
            {
                cmd.Dispose();
                cn.Dispose();
            }

            return _ret;
        }
    }
    /// <summary>
    /// Summary description for unCCed
    /// </summary>
    public class PageDb
    {
        public PageDb()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public static bool doesTitleExist(string title)
        {
            bool ret = false;

            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.rsp_Page_TitleExists", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Title", title));

            SqlParameter prm = cmd.Parameters.Add("@DoesExist", SqlDbType.Bit);
            prm.Direction = ParameterDirection.Output;
            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                ret = (bool)prm.Value;
            }
            catch (Exception ex)
            {
                //TODO audit
                //Audit.Event(0, "DAL.PageDb.AddPage", ex.Message);
                ret = false;
            }
            finally
            {
                cmd.Dispose();
                cn.Dispose();
            }

            return ret;
        }
        public static bool doesTitleExist(string title, string email)
        {
            bool ret = false;

            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.rsp_Page_TitleEmailExists", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Title", title));
            cmd.Parameters.Add(new SqlParameter("@Email", email));

            SqlParameter prm = cmd.Parameters.Add("@DoesExist", SqlDbType.Bit);
            prm.Direction = ParameterDirection.Output;
            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                ret = (bool)prm.Value;
            }
            catch (Exception ex)
            {
                //TODO audit
                //Audit.Event(0, "DAL.PageDb.AddPage", ex.Message);
                ret = false;
            }
            finally
            {
                cmd.Dispose();
                cn.Dispose();
            }

            return ret;
        }
        public static bool doesUrlIdExist(string urlId)
        {
            bool ret = false;

            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.rsp_Page_UrlIdExists", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@UrlId", urlId));

            SqlParameter prm = cmd.Parameters.Add("@DoesExist", SqlDbType.Bit);
            prm.Direction = ParameterDirection.Output;
            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                ret = (bool)prm.Value;
            }
            catch (Exception ex)
            {
                //TODO audit
                //Audit.Event(0, "DAL.PageDb.AddPage", ex.Message);
                ret = false;
            }
            finally
            {
                cmd.Dispose();
                cn.Dispose();
            }

            return ret;
        }
        public static bool doesUidlExist(string uidl)
        {
            bool ret = false;

            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.rsp_ConvertedEmail_Exists", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Uidl", uidl));

            SqlParameter prm = cmd.Parameters.Add("@DoesExist", SqlDbType.Bit);
            prm.Direction = ParameterDirection.Output;
            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                ret = (bool)prm.Value;
            }
            catch (Exception ex)
            {
                //TODO audit
                //Audit.Event(0, "DAL.PageDb.AddPage", ex.Message);
                ret = false;
            }
            finally
            {
                cmd.Dispose();
                cn.Dispose();
            }

            return ret;
        }
        public static bool AddOrigContent(int pageKey, string content)
        {
            bool ret = false;

            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.rsp_Page_OrigContent_Save ", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@PageKey", pageKey));
            cmd.Parameters.Add(new SqlParameter("@Content", content));

            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //TODO audit
                //Audit.Event(0, "DAL.PageDb.AddPage", ex.Message);
                ret = false;
            }
            finally
            {
                cmd.Dispose();
                cn.Dispose();
            }

            return ret;
        }

        public static bool AddUser(int pageKey, string userId) {
            bool ret = false;

            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.rsp_Page_AddUser", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@PageKey", pageKey));
            cmd.Parameters.Add(new SqlParameter("@AnonUserId", userId));

            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                ret = true;

            }
            catch (Exception ex)
            {
                //TODO audit
                //Audit.Event(0, "DAL.PageDb.AddPage", ex.Message);
                ret = false;
            }
            finally
            {
                cmd.Dispose();
                cn.Dispose();
            }

            return ret;
        }

        public static bool AddUserViewed(int pageKey, string userId)
        {
            bool ret = false;

            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.rsp_Page_UserViewed", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@PageKey", pageKey));
            cmd.Parameters.Add(new SqlParameter("@AnonUserId", userId));

            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                ret = true;

            }
            catch (Exception ex)
            {
                //TODO audit
                //Audit.Event(0, "DAL.PageDb.AddPage", ex.Message);
                ret = false;
            }
            finally
            {
                cmd.Dispose();
                cn.Dispose();
            }

            return ret;
        }


        public static string CreateBlankUser(int pageKey)
        {
            string ret = String.Empty;

            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.rsp_PageUser_CreateBlank", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@PageKey", pageKey));


            SqlParameter prm = cmd.Parameters.Add("@AnonUserId", SqlDbType.UniqueIdentifier);
            prm.Direction = ParameterDirection.Output;

            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                ret = prm.Value.ToString();

            }
            catch (Exception ex)
            {
                //TODO audit
                //Audit.Event(0, "DAL.PageDb.AddPage", ex.Message);
                ret = String.Empty;
            }
            finally
            {
                cmd.Dispose();
                cn.Dispose();
            }

            return ret;
        }

        public static int Add(string title, string urlId, string ownerUserId, string pagePassword)
        {
            int tReturn = 0;

            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.rsp_Page_Save", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@title", title));
            cmd.Parameters.Add(new SqlParameter("@urlId", urlId));
            cmd.Parameters.Add(new SqlParameter("@ownerUserId", ownerUserId));
            if (!String.IsNullOrEmpty(pagePassword))
                cmd.Parameters.Add(new SqlParameter("@pagePassword", pagePassword));


            SqlParameter prm = cmd.Parameters.Add("@PageKey", SqlDbType.Int);
            prm.Direction = ParameterDirection.Output;

            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                tReturn = (int)prm.Value;

            }
            catch (Exception ex)
            {
                //TODO audit
                //Audit.Event(0, "DAL.PageDb.AddPage", ex.Message);
                tReturn = -1;
            }
            finally
            {
                cmd.Dispose();
                cn.Dispose();
            }

            return tReturn;
        }

        public static int AddViaEmail(string title, string urlId, string email, string pagePassword)
        {
            int tReturn = 0;

            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.rsp_Page_Create", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@title", title));
            cmd.Parameters.Add(new SqlParameter("@urlId", urlId));
            cmd.Parameters.Add(new SqlParameter("@email", email));
            if (!String.IsNullOrEmpty(pagePassword))
                cmd.Parameters.Add(new SqlParameter("@pagePassword", pagePassword));


            SqlParameter prm = cmd.Parameters.Add("@PageKey", SqlDbType.Int);
            prm.Direction = ParameterDirection.Output;

            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                tReturn = (int)prm.Value;

            }
            catch (Exception ex)
            {
                //TODO audit
                //Audit.Event(0, "DAL.PageDb.AddPage", ex.Message);
                tReturn = -1;
            }
            finally
            {
                cmd.Dispose();
                cn.Dispose();
            }

            return tReturn;
        }
        public static int AddPageInvite(int pageKey, string userId, string email)
        {
            int tReturn = 0;

            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.rsp_Page_InviteUser", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@PageKey", pageKey));
            cmd.Parameters.Add(new SqlParameter("@AnonUserId", userId));
            cmd.Parameters.Add(new SqlParameter("@Email", email));

            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //TODO audit
                //Audit.Event(0, "DAL.PageDb.AddPage", ex.Message);
                tReturn = -1;
            }
            finally
            {
                cmd.Dispose();
                cn.Dispose();
            }

            return tReturn;
        }

        public static int AddPageInvite(int pageKey, string userId, string email, string name)
        {
            int tReturn = 0;

            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.rsp_Page_InviteUser", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@PageKey", pageKey));
            cmd.Parameters.Add(new SqlParameter("@AnonUserId", userId));
            cmd.Parameters.Add(new SqlParameter("@Email", email));
            cmd.Parameters.Add(new SqlParameter("@Name", name));

            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //TODO audit
                //Audit.Event(0, "DAL.PageDb.AddPage", ex.Message);
                tReturn = -1;
            }
            finally
            {
                cmd.Dispose();
                cn.Dispose();
            }

            return tReturn;
        }
        public static DataTable Get(string urlId)
        {
            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.rsp_Page_GetFromUrlId", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@UrlId", urlId));
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            cn.Open();
            da.Fill(dt);
            cmd.Dispose();
            cn.Dispose();
            return dt;
        }
        public static DataTable Get(int pageKey)
        {
            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.rsp_Page_GetFromId", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@pageKey", pageKey));
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            cn.Open();
            da.Fill(dt);
            cmd.Dispose();
            cn.Dispose();
            return dt;
        }

        public static DataTable GetActiveForUser(string userId)
        {
            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.rsp_Page_GetActiveForUser", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@AnonUserId", userId));
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            cn.Open();
            da.Fill(dt);
            cmd.Dispose();
            cn.Dispose();
            return dt;
        }
    }

    public class PageCommentDb
    {
        public PageCommentDb() { }

        public static int Add(int pageKey, string userId, string usersName, string comment)
        {
            int tReturn = 0;

            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.rsp_PageComment_Add", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@PageKey", pageKey));
            cmd.Parameters.Add(new SqlParameter("@UserId", userId));
            cmd.Parameters.Add(new SqlParameter("@usersName", usersName));
            cmd.Parameters.Add(new SqlParameter("@comment", comment));


            SqlParameter prm = cmd.Parameters.Add("@CommentKey", SqlDbType.Int);
            prm.Direction = ParameterDirection.Output;

            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                tReturn = (int)prm.Value;

            }
            catch (Exception ex)
            {
                //TODO audit
                //Audit.Event(0, "DAL.PageDb.AddPage", ex.Message);
                tReturn = -1;
            }
            finally
            {
                cmd.Dispose();
                cn.Dispose();
            }

            return tReturn;
        }
        public static int Add(string urlId, string userId, string usersName, string comment)
        {
            int tReturn = 0;

            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.rsp_PageComment_AddFromUrlId", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@urlId", urlId));
            cmd.Parameters.Add(new SqlParameter("@UserId", userId));
            cmd.Parameters.Add(new SqlParameter("@usersName", usersName));
            cmd.Parameters.Add(new SqlParameter("@comment", comment));


            SqlParameter prm = cmd.Parameters.Add("@CommentKey", SqlDbType.Int);
            prm.Direction = ParameterDirection.Output;

            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                tReturn = (int)prm.Value;

            }
            catch (Exception ex)
            {
                //TODO audit
                //Audit.Event(0, "DAL.PageDb.AddPage", ex.Message);
                tReturn = -1;
            }
            finally
            {
                cmd.Dispose();
                cn.Dispose();
            }

            return tReturn;
        }

        public static DataTable GetNew(int pageKey, int lastCommentKey)
        {

            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.rsp_PageComment_GetNew", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@PageKey", pageKey));
            cmd.Parameters.Add(new SqlParameter("@LastCommentKey", lastCommentKey));
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            cn.Open();
            da.Fill(dt);
            cmd.Dispose();
            cn.Dispose();
            return dt;
        }
        public static DataTable GetNew(int pageKey, int lastCommentKey, string requestingUserId)
        {

            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.rsp_PageComment_GetNew", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@PageKey", pageKey));
            cmd.Parameters.Add(new SqlParameter("@LastCommentKey", lastCommentKey));
            if (requestingUserId.Length > 0)
                cmd.Parameters.Add(new SqlParameter("@RequestingAnonUserId", requestingUserId));
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            cn.Open();
            da.Fill(dt);
            cmd.Dispose();
            cn.Dispose();
            return dt;
        }

        public static DataTable GetNew(string urlId, int lastCommentKey, string requestingUserId)
        {

            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.rsp_PageComment_GetNewFromUrlId", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@UrlId", urlId));
            cmd.Parameters.Add(new SqlParameter("@LastCommentKey", lastCommentKey));
            if (requestingUserId.Length > 0)
                cmd.Parameters.Add(new SqlParameter("@RequestingAnonUserId", requestingUserId));    
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            cn.Open();
            da.Fill(dt);
            cmd.Dispose();
            cn.Dispose();
            return dt;
        }

        public static DataTable GetNew(string urlId, int lastCommentKey)
        {

            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.rsp_PageComment_GetNewFromUrlId", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@UrlId", urlId));
            cmd.Parameters.Add(new SqlParameter("@LastCommentKey", lastCommentKey));
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            cn.Open();
            da.Fill(dt);
            cmd.Dispose();
            cn.Dispose();
            return dt;
        }
    }

}