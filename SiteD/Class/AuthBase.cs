using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

namespace Joson.SSOSite.OAuth
{
    /// <summary>
    /// ��Ȩҳ�����
    /// </summary>
    public class BaseClass : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            if (Session["Token"] != null)
            {
                //��վƾ֤����
                Response.Write("��ϲ����վƾ֤���ڣ�������Ȩ���ʸ�ҳ�棡");
            }
            else
            {
                //������֤���
                if (Request.QueryString["Token"] != null)
                {
                    if (Request.QueryString["Token"] != "$Token$")
                    {
                        //��������
                        string tokenValue = Request.QueryString["Token"];
                        //����WebService��ȡ��վƾ֤


                        #region Joson Test
                        //string strURL = "http://www.webxml.com.cn/WebServices/ChinaZipSearchWebService.asmx";
                        //string ServiceName = "getAddressByZipCode";
                        //string[] args = new string[2];

                        //args[0] = "723112";
                        //args[1] = "";

                        //Net.Common.ResponseWebServices.InvokeWebService(strURL, ServiceName, args);
                        
                        #endregion
                       
                        string JosonURL = " http://www.passport.com/TokenService.asmx";
                        string JosonServiceName = "TokenGetCredence";
                        string[] JosonArgs = new string[1];
                        JosonArgs[0] = tokenValue;

                        object o = Net.Common.ResponseWebServices.InvokeWebService(JosonURL, JosonServiceName, JosonArgs);
                        if (o != null)
                        {
                            //������ȷ
                            Session["Token"] = o;
                            Response.Write("��ϲ�����ƴ��ڣ�������Ȩ���ʸ�ҳ�棡");
                        }
                        else
                        {
                            //���ƴ���
                            Response.Redirect(this.replaceToken());
                        }
                    }
                    else
                    {
                        //δ��������
                        Response.Redirect(this.replaceToken());
                    }
                }
                //δ����������֤��ȥ��վ��֤
                else
                {
                    Response.Redirect(this.getTokenURL());
                }
            }

            base.OnLoad(e);
        }

        /// <summary>
        /// ��ȡ�����������URL
        /// </summary>
        /// <returns></returns>
        private string getTokenURL()
        {
            string url = Request.Url.AbsoluteUri;
            Regex reg = new Regex(@"^.*\?.+=.+$");
            if (reg.IsMatch(url))
                url += "&Token=$Token$";
            else
                url += "?Token=$Token$";

            return "http://www.passport.com/gettoken.aspx?BackURL=" + Server.UrlEncode(url);
        }

        /// <summary>
        /// ȥ��URL�е�����
        /// </summary>
        /// <returns></returns>
        private string replaceToken()
        {
            string url = Request.Url.AbsoluteUri;
            url = Regex.Replace(url, @"(\?|&)Token=.*", "", RegexOptions.IgnoreCase);
            return "http://www.passport.com/userlogin.aspx?BackURL=" + Server.UrlEncode(url);
        }

    }
}
