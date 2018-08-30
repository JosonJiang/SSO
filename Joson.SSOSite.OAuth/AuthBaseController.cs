using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using System.Collections;
using System.Text.RegularExpressions;

namespace Joson.SSOSite.OAuth
{
    #region ֪ʶ��

    /*
     
        1. OnPreInit 
        2. OnInit 
        3. OnInitComplete 
        4. OnPreLoad 
        5. Page_Load 
        6. OnLoad 
        7. Button_Click 
        8. OnLoadComplete 
        9. OnPreRender 
  
        1.  OnActionExecuted    ��ִ�в����������� MVC ��ܵ��á�
        2.  OnActionExecuting   ��ִ�в�������֮ǰ�� MVC ��ܵ��á�
        3.  OnResultExecuted    ��ִ�в���������� MVC ��ܵ��á�
        4.  OnResultExecuting   ��ִ�в������֮ǰ�� MVC ��ܵ��á�
     * 
     * 

     
     */

    /**********************************************************************************************************************************

    ���ض�ӦLoad�¼���OnLoad��������������¼������Ŵ�������Ѷ���Ƚ���Ϥ����VS.Net���ɵ�ҳ���е�Page_Load����������ӦLoad�¼��ķ�����
    ����ÿһ������Load�¼����ᴥ����Page_Load����Ҳ�ͻ�ִ�У�������Ҳ�Ǵ�������˽�ASP.Net�ĵ�һ����   
   
    Page_Load������Ӧ��Load�¼�������¼�����System.Web.WebControl.Control���ж���ģ��������Page�����з������ؼ������ڣ���������OnLoad�����б�������  
   
     
   
    �ܶ��˿������������������飬д��һ��PageBase�࣬Ȼ����Page_Load������֤�û���Ϣ��������ֲ�����֤�Ƿ�ɹ�������ҳ���Page_Load���ǻ���ִ��
    �����ʱ��ܿ�������һЩ��ȫ�Ե��������û�������û�еõ���֤������¾�ִ���������е�Page_Load������  
   
    ������������ԭ��ܼ򵥣���ΪPage_Load��������OnInit�б���ӵ�Load�¼��еģ��������OnInit���������������Load�¼���Ȼ���ٵ���base.OnInit��
    ����������������Page_Load������ӣ���ô��ִ���ˡ�  
   
    Ҫ����������Ҳ�ܼ򵥣������ַ�����  
   
   1�� ��PageBase������OnLoad������Ȼ����OnLoad����֤�û���Ȼ�����base.OnLoad����ΪLoad�¼�����OnLoad�д������������ǾͿ��Ա�֤�ڴ���Load�¼�֮ǰ��֤�û���  
   
   2�� �������OnInit�������ȵ���base.OnInit����������֤������ִ��Page_Load  

     
   * * */

    #endregion

    using Net.Common;
    using System.Collections.Generic;
    //using System.Xml;

    //<add key="OAuthURL" value="http://www.passport.com/"/>
    //<add key="GetToken" value="http://www.passport.com/gettoken.aspx"/>
    //<add key="AccountLogin" value="http://www.passport.com/userlogin.aspx"/>
    //<add key="TokenService" value="http://www.passport.com/TokenService.asmx"/>

    //<add key="OAuthCorrectMsg" value="��ϲ����վƾ֤���ڣ�������Ȩ���ʸ�ҳ�棡"/>
    //<add key="TokenCorrectMsg" value="��ϲ�����ƴ��ڣ�������Ȩ���ʸ�ҳ�棡" />


    //<add key="TokenCorrectRedirectURL" value="http://ehr.SDT.com/App/Index.aspx" />
    //<add key="LogOutRedirect" value="http://ehome.sdt.com/Index.aspx" />


    /// <summary>
    /// ��Ȩҳ�����
    /// </summary>
    public class BaseController : Controller
    {
        protected String OAuthURL = String.Empty;
        protected String GetToken = String.Empty;

        protected String AccountLogin = String.Empty;
        protected String TokenService = String.Empty;
        public String OAuthCorrectMsg = String.Empty;
        public String TokenCorrectMsg = String.Empty;
        public String TokenCorrectRedirectURL = String.Empty;


        protected string HostUrl = String.Empty;

        /// <summary>
        /// Actionִ��ǰ�ж�
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            #region OnActionExecuting

            OAuthURL = Net.Common.ConfigHelper.GetConfig("OAuthURL");
            GetToken = Net.Common.ConfigHelper.GetConfig("GetToken");

            AccountLogin = Net.Common.ConfigHelper.GetConfig("AccountLogin");
            TokenService = Net.Common.ConfigHelper.GetConfig("TokenService");

            OAuthCorrectMsg = Net.Common.ConfigHelper.GetConfig("OAuthCorrectMsg");
            TokenCorrectMsg = Net.Common.ConfigHelper.GetConfig("TokenCorrectMsg");
            TokenCorrectRedirectURL = Net.Common.ConfigHelper.GetConfig("TokenCorrectRedirectURL");



            if (Session["Token"] != null)
            {
                //��վƾ֤����

                //Response.Write("��ϲ����վƾ֤���ڣ�������Ȩ���ʸ�ҳ�棡");
                Response.Write(OAuthCorrectMsg);

            }
            else
            {
                //��վƾ֤������

                //������֤���
                if (Request.QueryString["Token"] != null)
                {
                    #region ���Ƽ��

                    if (Request.QueryString["Token"] != "$Token$")
                    {
                        //��������
                        string tokenValue = Request.QueryString["Token"];
                        string strRedirect = Request.QueryString["Redirect"];
                        //����WebService��ȡ��վƾ֤

                        object result = null;

                        if (tokenValue.NotIsNullOrEmpty())
                        {
                            #region ���ƴ��ڴ����ݿ� ��ȡ����

                            #region Joson Test
                            //string strURL = "http://www.webxml.com.cn/WebServices/ChinaZipSearchWebService.asmx";
                            //string ServiceName = "getAddressByZipCode";
                            //string[] args = new string[2];

                            //args[0] = "723112";
                            //args[1] = "";

                            //Net.Common.ResponseWebServices.InvokeWebService(strURL, ServiceName, args);

                            #endregion

                            //string JosonURL ="http://www.passport.com/TokenService.asmx";

                            string JosonURL = TokenService;
                            string JosonServiceName = "TokenGetCredences";
                            string[] JosonArgs = new string[1];
                            JosonArgs[0] = tokenValue;


                            JosonServiceName = "TokenGetCredencDt";
                            //JosonServiceName = "TokenGetCredence";  


                            try
                            {
                                result = Net.Common.ResponseWebServices.InvokeWebService(JosonURL, JosonServiceName, JosonArgs);



                                //Hashtable HashTab = new Hashtable();
                                //HashTab.Add("tokenValue", tokenValue);

                                #region Joson  Test

                                //JosonURL = "http://www.webxml.com.cn/WebServices/ChinaZipSearchWebService.asmx";
                                //JosonServiceName = "getAddressByZipCode";

                                //HashTab.Add("theZipCode", "723000");
                                //HashTab.Add("userID", "");

                                #endregion

                                //XmlDocument XMLResult =ResponseWebServices.QueryGetWebService(JosonURL, JosonServiceName, HashTab);
                                //DataSet Ds = DataSetXML.XmlDocumentToDataSet(XMLResult);
                            }
                            catch (Exception ex) 
                            { 
                                //throw new Exception(String.Format("��¼��֤����{0}", ex.Message));
                            }

                            #endregion

                        }

                        #region ������֤

                        if (result != null)
                        {

                            //������ȷ
                            Session["Token"] = result;
                            Response.Write(TokenCorrectMsg);
                            // Response.Write("��ϲ�����ƴ��ڣ�������Ȩ���ʸ�ҳ�棡");

                            if (strRedirect != null || strRedirect.NotIsNullOrEmpty())
                                Response.Redirect(strRedirect);
                            else
                                if (TokenCorrectRedirectURL.NotIsNullOrEmpty())
                                    Response.Redirect(TokenCorrectRedirectURL);

                        }
                        else
                        {
                            //���ƴ��� ���µ�¼��֤
                            Response.Redirect(this.replaceToken());
                        }

                        #endregion

                    }
                    else
                    {
                        //δ�������� ���µ�¼��֤
                        Response.Redirect(this.replaceToken());
                    }

                    #endregion

                }
                else
                {
                    //δ����������֤��ȥ��վ��֤

                    Response.Redirect(this.getTokenURL());
                }
            }
            #endregion


            //this.HostUrl = "http://" + this.Request.Url.Host;
            //this.HostUrl += this.Request.Url.Port.ToString() == "80" ? "" : ":" + this.Request.Url.Port;
            //this.HostUrl += this.Request.ApplicationPath;

            //List<string> ActionNameList = new List<string>();
            //ActionNameList.Add("GetWebName");
            //ActionNameList.Add("Login");

            //// �ж��Ƿ��¼
            //if (!this.checkLogin() && !ActionNameList.Contains(filterContext.ActionDescriptor.ActionName))
            //{
            //    filterContext.Result = RedirectToRoute("Home", new { Controller = "Login", Action = "Index" });
            //}



            base.OnActionExecuting(filterContext);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {

            base.OnResultExecuting(filterContext);
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

            return "{0}?BackURL={1}".WithFormat(GetToken, Server.UrlEncode(url));
            //http://www.passport.com/gettoken.aspx?BackURL=http://www.ehr.com/index.aspx?Token=$Token$
            //return "http://www.passport.com/gettoken.aspx?BackURL=" + Server.UrlEncode(url);
        }

        /// <summary>
        /// ȥ��URL�е�����
        /// ���µ�¼��֤
        /// </summary>
        /// <returns></returns>
        private string replaceToken()
        {
            string url = Request.Url.AbsoluteUri;
            url = Regex.Replace(url, @"(\?|&)Token=.*", "", RegexOptions.IgnoreCase);

            return "{0}?BackURL={1}".WithFormat(AccountLogin, Server.UrlEncode(url));
            //return "http://www.passport.com/userlogin.aspx?BackURL=" + Server.UrlEncode(url);
        }

    }


    #region BaseLogOut

    /// <summary>
    /// BaseLogOut
    /// </summary>
    public class BaseLogOutController : Controller
    {
        protected String GetToken = String.Empty;
        protected String TokenService = String.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            GetToken = Net.Common.ConfigHelper.GetConfig("GetToken");
            TokenService = Net.Common.ConfigHelper.GetConfig("TokenService");


            base.OnActionExecuting(filterContext);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ClearToken();
            base.OnActionExecuted(filterContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            ClearToken();
            base.OnResultExecuted(filterContext);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            ClearToken();
            base.OnResultExecuting(filterContext);
        }


        /// <summary>
        /// ClearToken
        /// </summary>
        /// <param name="RedirectURL"></param>
        public virtual Boolean ClearToken()
        {
            Boolean ClearToken = false;

            Session["Token"] = null;


            //�����վƾ֤
            if (Request.QueryString["Token"] == null)
            {
                //String RedirectURL = "http://www.passport.com/gettoken.aspx?BackURL=" + Server.UrlEncode(Request.Url.AbsoluteUri + "?Token=$Token$");
                String RedirectURL = "{0}?BackURL=" + Server.UrlEncode(Request.Url.AbsoluteUri + "?Token=$Token$");
                //Response.Write(RedirectURL + "<BR>");
                //Response.Write(GetToken.FormatWith(RedirectURL));
                Response.Redirect(GetToken.FormatWith(RedirectURL));
            }
            else
            {
                if (Request.QueryString["Token"] != "$Token$")
                {
                    string token = Request.QueryString["Token"];
                    //����WebService����

                    //String strURL = "http://www.passport.com/TokenService.asmx";
                    String strURL = TokenService;
                    String ServiceName = "ClearToken";

                    string[] args = new string[1];
                    args[0] = token;

                    object o = Net.Common.ResponseWebServices.InvokeWebService(strURL, ServiceName, args);

                    ClearToken = true;

                }
            }

            //��ձ���ƾ֤
            Session.Abandon();
            ClearCookies();

            return ClearToken;


        }



        /// <summary>
        /// ClearCookies
        /// </summary>
        public virtual void ClearCookies()
        {

            HttpCookie myCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (myCookie != null)
            {
                DateTime now = DateTime.Now;
                myCookie.Expires = now.AddYears(-2);
                Response.Cookies.Add(myCookie);
            }

        }



    }

    #endregion



}
