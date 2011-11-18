using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;

namespace unCCed.DAL
{
    /// <summary>
    /// Summary description for unCCed
    /// </summary>
    public class PollDb
    {
        public PollDb()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static DataTable Get(int pollKey)
        {
            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT p.*, (SELECT COUNT(*) FROM dbo.PollUserAnswer WHERE PollKey = p.PollKey) as TotalVotes FROM dbo.Poll p WHERE p.PollKey = @PollKey", cn);
           // cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@PollKey", pollKey));
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
            SqlCommand cmd = new SqlCommand("SELECT p.*, (SELECT COUNT(*) FROM dbo.PollUserAnswer WHERE PollKey = p.PollKey) as TotalVotes FROM dbo.Poll p INNER JOIN dbo.PagePoll pp ON p.PollKey = pp.PollKey WHERE pp.PageKey = @PageKey", cn);
            // cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@PageKey", pageKey));
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            cn.Open();
            da.Fill(dt);
            cmd.Dispose();
            cn.Dispose();
            return dt;
        }

        public static DataTable GetAllForPage(int pageKey, string requestingUserId)
        {
            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT p.*, (SELECT COUNT(*) FROM dbo.PollUserAnswer WHERE PollKey = p.PollKey) as TotalVotes, (SELECT COUNT(*) FROM dbo.PollUserAnswer WHERE PollKey = p.PollKey AND AnonUserId = @UserId) as UserAnswered FROM dbo.Poll p INNER JOIN dbo.PagePoll pp ON p.PollKey = pp.PollKey WHERE pp.PageKey = @PageKey", cn);
            // cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@PageKey", pageKey));
            cmd.Parameters.Add(new SqlParameter("@UserId", requestingUserId));
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            cn.Open();
            da.Fill(dt);
            cmd.Dispose();
            cn.Dispose();
            return dt;
        }

        public static DataTable GetResults(int pollKey)
        {
            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT Answer, COUNT(*) as votes  FROM dbo.PollUserAnswer WHERE PollKey = @PollKey GROUP BY Answer ORDER BY Answer ASC", cn);
            // cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@PollKey", pollKey));
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            cn.Open();
            da.Fill(dt);
            cmd.Dispose();
            cn.Dispose();
            return dt;
        }
        public static int Add(string question, string answers, string ownerUserId, int pageKey)
        {
            int tReturn = 0;

            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.rsp_Poll_Add", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Question", question));
            cmd.Parameters.Add(new SqlParameter("@Answers", answers));
            cmd.Parameters.Add(new SqlParameter("@AnonUserId", ownerUserId));
            cmd.Parameters.Add(new SqlParameter("@PageKey", pageKey));


            SqlParameter prm = cmd.Parameters.Add("@PollKey", SqlDbType.Int);
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

        public static bool SubmitAnswer(int pollKey, string userId, int answerIndex)
        {
            bool tReturn = false;

            SqlConnection cn = new SqlConnection(Common.ConnectionString);
            SqlCommand cmd = new SqlCommand("dbo.rsp_Poll_SubmitAnswer", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@PollKey", pollKey));
            cmd.Parameters.Add(new SqlParameter("@AnonUserId", userId));
            cmd.Parameters.Add(new SqlParameter("@Answer", answerIndex));



            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                tReturn = true;

            }
            catch (Exception ex)
            {
                //TODO audit
                //Audit.Event(0, "DAL.PageDb.AddPage", ex.Message);
                tReturn = false;
            }
            finally
            {
                cmd.Dispose();
                cn.Dispose();
            }

            return tReturn;
        }
    }
}