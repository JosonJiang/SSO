using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace Joson.SSO.Passport
{
    using System.Data;
    using System.Data.SqlClient;
    using DigitalHR.DBUtility;
    using System.Timers;

    public class Global : System.Web.HttpApplication
    {
        Boolean OAuthDefault = String.IsNullOrWhiteSpace(System.Configuration.ConfigurationManager.AppSettings["OAuthDefault"]);

        protected void Application_Start(object sender, EventArgs e)
        {
            Application["online"] = 0;

            string sqlString = "select isnull(max(Total),0) as Total from SYS_Home_Total(NOLOCK)";


            if (OAuthDefault)
            {
                using (SqlDataReader reader = SqlServerHelper.ExecuteReader(SqlServerHelper.ConnectionMP, System.Data.CommandType.Text, sqlString))
                {
                    if (reader.Read())
                    {
                        Application["total"] = Convert.ToInt32(reader[0]);
                    }
                    else
                    {
                        Application["total"] = 0;
                    }
                }

                //���һ��ʱ��ִ��һ��
                System.Timers.Timer objTimer = new Timer();
                objTimer.Interval = 600000; //���ʱ�䵥λ����,����10�룬��д10000 
                objTimer.Enabled = true;
                objTimer.Elapsed += new ElapsedEventHandler(UpdateTotal);

            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            if (OAuthDefault)
            {
                // ���»Ự����ʱ���еĴ���
                Application.Lock();
                Session.Timeout = 20;
                Application["total"] = (int)Application["total"] + 1;
                Application["online"] = (int)Application["online"] + 1;
                Application.UnLock();
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            if (OAuthDefault)
            {
                // �ڻỰ����ʱ���еĴ��롣 
                Application.Lock();
                Application["online"] = (int)Application["online"] - 1;
                Application.UnLock();
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// �����ܷ��ʴ���
        /// </summary>
        protected void UpdateTotal(object sender, ElapsedEventArgs e)
        {
            string sqlString = "DECLARE @MAX int; SELECT @MAX = ISNULL(MAX(Total),0) FROM SYS_Home_Total(NOLOCK); IF @Total-@MAX>200 INSERT INTO SYS_Home_Total(Total) VALUES(@Total)";

            SqlParameter[] prams = { new SqlParameter("@Total", SqlDbType.Int) };

            prams[0].Value = (int)Application["total"];

            SqlServerHelper.ExecuteNonQuery(SqlServerHelper.ConnectionMP, System.Data.CommandType.Text, sqlString, prams);
        }
    }
}