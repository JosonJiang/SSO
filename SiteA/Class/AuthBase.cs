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

namespace SSO.SiteA.Class
{
    /// <summary>
    /// ��Ȩҳ�����
    /// </summary>
    public class AuthBase : System.Web.UI.Page
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
                    #region ���Ƽ��

                    if (Request.QueryString["Token"] != "$Token$")
                    {
                        //��������

                        string tokenValue = Request.QueryString["Token"];
                        //����WebService��ȡ��վƾ֤
                        SSO.SiteA.RefPassport.TokenService tokenService = new SSO.SiteA.RefPassport.TokenService();
                        object o = tokenService.TokenGetCredence(tokenValue);

                        #region ������֤

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

                        #endregion
                    }
                    else
                    {
                        //δ��������
                        Response.Redirect(this.replaceToken());
                    } 

                    #endregion
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
        /// �ڵ�ǰURL�и����������������
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
        /// �ڵ�ǰURL��ȥ�����Ʋ���
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
