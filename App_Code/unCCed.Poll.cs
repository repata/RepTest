using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace unCCed
{
    /// <summary>
    /// Summary description for unCCed
    /// </summary>
    public class Poll
    {
        public Poll()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public Poll(int pollKey)
        {
            DataTable dt = DAL.PollDb.Get(pollKey);
            if (dt.Rows.Count == 1)
            {
                DataRow dr = dt.Rows[0];
                _question = dr["Question"].ToString();
                _answers = Regex.Split(dr["Answers"].ToString(), "\n");
                _id = pollKey;
                _totalVotes = (int)dr["TotalVotes"];
            }
        }

        string _question = String.Empty;
        string[] _answers = null;
        int _id = 0;
        int _totalVotes = 0;
        PollResultItem[] _results = null;

        public string Question
        {
            get { return _question; }
        }
        public string[] Answers
        {
            get { return _answers; }
        }
        public int ID { get { return _id; } }
        public int TotalVotes
        {
            get {return _totalVotes;}
        }

        public PollResultItem[] Results
        {
            get {

                if (_totalVotes > 0)
                {
                    DataTable dt = DAL.PollDb.GetResults(_id);
                    PollResultItem[] t = new PollResultItem[_answers.Length];
                    for (int i = 0; i < _answers.Length; i++)
                    {
                        t[i] = new PollResultItem(i, _answers[i], 0,0);
                    }
                    foreach (DataRow dr in dt.Rows)
                    {
                        int a = Convert.ToInt32(dr["Answer"].ToString());
                        t[a - 1].Percent = Convert.ToDouble(dr["Votes"].ToString()) / _totalVotes;
                        t[a - 1].Votes = (int)dr["Votes"];
                    }
                    return t;
                }
                else
                    return null;
                
            }
        }


    }

    public class PollResultItem
    {
        public PollResultItem(int id, string answer, double pct, int votes)
        {
            AnswerIndex = id;
            Answer = answer;
            Percent = pct;
            Votes = votes;

        }
        public int AnswerIndex = 0;
        public string Answer = String.Empty;
        public double Percent = 0;
        public int Votes = 0;
    }
}