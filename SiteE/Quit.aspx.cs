using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace SSO.SiteE
{

    using Net.Common;

    /// <summary>
    /// �˳�
    /// </summary>
    public partial class Quit : Joson.SSOSite.OAuth.BaseLogOut   //System.Web.UI.Page  //
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region �����վƾ֤
            ////�����վƾ֤
            //if (Request.QueryString["Token"] == null)
            //{
            //    Response.Redirect("http://www.passport.com/gettoken.aspx?BackURL=" + Server.UrlEncode(Request.Url.AbsoluteUri + "?Token=$Token$"));
            //}
            //else
            //{
            //    if (Request.QueryString["Token"] != "$Token$")
            //    {
            //        string token = Request.QueryString["Token"];
            //        //����WebService����


            //        String strURL = " http://www.passport.com/TokenService.asmx";
            //        String ServiceName = "ClearToken";

            //        string[] args = new string[1];
            //        args[0] = token;

            //        object o = Net.Common.ResponseWebServices.InvokeWebService(strURL, ServiceName, args);


            //        //SiteA.RefPassport.TokenService myRef = new SSO.SiteA.RefPassport.TokenService();
            //        //myRef.ClearToken(token);

            //    }
            //}

            ////��ձ���ƾ֤
            //Session.Abandon(); 
            #endregion

            //base.OnInit(e);
           // base.OnLoadComplete(e);

            base.ClearToken();

            //this.ClearToken();

          
        }



        public void ClearToken()
        {

            //�����վƾ֤
            if (Request.QueryString["Token"] == null)
            {
                Response.Redirect("http://www.passport.com/gettoken.aspx?BackURL=" + Server.UrlEncode(Request.Url.AbsoluteUri + "?Token=$Token$"));
            }
            else
            {
                if (Request.QueryString["Token"] != "$Token$")
                {
                    string token = Request.QueryString["Token"];
                    //����WebService����

                    String strURL = "http://www.passport.com/TokenService.asmx";
                    String ServiceName = "ClearToken";

                    string[] args = new string[1];
                    args[0] = token;

                    object o = Net.Common.ResponseWebServices.InvokeWebService(strURL, ServiceName, args);
                }
            }

            //��ձ���ƾ֤
            Session.Abandon();

            Response.Write("���˳���");
        }

    }
}
