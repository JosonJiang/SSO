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
//using SSO.Passport.Class;

using Joson.SSOSite.OAuth;

using Net.LDAPHelper;
using Net.Common;

namespace Joson.SSO.Passport
{
    /// <summary>
    /// �û���¼
    /// ����Get������BackURL
    /// </summary>
    public partial class UserLogin : System.Web.UI.Page
    {
        public String strCookieDomain = String.Empty;
        public String OAuthLDAP = String.Empty;
        public String OAuthDefaultURL = String.Empty;
        public bool OAuthDefault = false;
        public bool OAuthByLDAP = false;
        public bool isValidUser = false;


        protected void Page_Load(object sender, EventArgs e)
        {
            //�ǳ���Ҫ������ ���Ҫ�����뱣֤����ȷ�ԣ����鲻Ҫ���� Ĭ�϶�ȡ����
            strCookieDomain = System.Configuration.ConfigurationManager.AppSettings["OAuthURL"];

            //�Ƿ�ʹ������֤
            OAuthByLDAP = System.Configuration.ConfigurationManager.AppSettings["OAuthByLDAP"] == "true";

            //��֤ͨ������תҳ�� SSO ��¼���ĵ���ҳ��
            OAuthDefaultURL = System.Configuration.ConfigurationManager.AppSettings["OAuthSucessfulURL"];

            OAuthDefault = String.IsNullOrWhiteSpace(System.Configuration.ConfigurationManager.AppSettings["OAuthDefault"]);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //�����û���¼��֤(�ʺš�������web.config��)
            //��ʵ�����˴�Ӧͨ�����ݿ������֤

            string strAccount = this.txtAccount.Text;
            string strPassport = this.txtPassport.Text;
            string strWebSiteName = String.Empty;


            JosonOAuth.OUser Entity = null;

            if (OAuthByLDAP)
            {
                if (OAuthDefault)
                {
                    OAuthLDAP = System.Configuration.ConfigurationManager.AppSettings["OAuthLDAP"];
                    isValidUser = ADHelper.TryAuthenticate(OAuthLDAP, strAccount, strPassport);

                }
                else
                {

                    JosonOAuth.WebServiceSoapClient O = new JosonOAuth.WebServiceSoapClient();

                    isValidUser = O.OAuth(out Entity, strAccount, strPassport);

                }

                //IdentityImpersonation Login = new IdentityImpersonation(strAccount, strPassport, OAuthLDAP);
                //Login.BeginImpersonate();

            }
            else
            {

                isValidUser = this.txtAccount.Text == System.Configuration.ConfigurationManager.AppSettings["Acount"]
                              && this.txtPassport.Text == System.Configuration.ConfigurationManager.AppSettings["PassWord"];

            }

            //JScript.Alert(this, OAuthDefaultURL);
            //JScript.Alert(this, isValidUser.ToStrings());

            //JScript.Alert(this, Request.QueryString["BackURL"]);
            //JScript.Alert(this, OAuthDefaultURL);

            if (isValidUser)
            {
                //��������
                string tokenValue = Guid.NewGuid().ToString().ToUpper(); //��������Ψһ�ַ�������������
                HttpCookie tokenCookie = new HttpCookie("Token");
                tokenCookie.Values.Add("Value", tokenValue);
                //��ȡ��֤վ������ "www.passport.com";
                tokenCookie.Domain = strCookieDomain ?? Net.Common.GetRequest.GetCurrentDomain();
                Response.AppendCookie(tokenCookie);

                //HttpCookie AccountCookie = new HttpCookie("AccountID");
                //AccountCookie.Values.Add("Value", strAccount);
                //Response.AppendCookie(AccountCookie);

                //HttpCookie PassportCookie = new HttpCookie("Passport");
                //PassportCookie.Values.Add("Value", strPassport);
                //Response.AppendCookie(PassportCookie);

                LabMsg.Text = String.Empty;

                OAuthToken OAuth = new OAuthToken
                {
                    ID = 0,
                    UserAgent = Request.UserAgent,
                    isLocked = false,
                    AccountID = strAccount,
                    AccountName = strAccount,

                    //sn = Entity?.SN,
                    //givenName = Entity?.GivenName,
                    //displayName = Entity?.DisplayName,
                    //initials = Entity?.initials,
                    //title = Entity?.Title,
                    //company = Entity?.Company,
                    //mail = Entity?.Mail,
                    //otherMailBox = Entity?.OtherMailBox,
                    //homePhone = Entity?.HomePhone,
                    //mobile = Entity?.Mobile,
                    //otherMobile = Entity?.OtherMobile,
                    //whenCreated = Entity.WhenCreated,
                    //whenChanged = Entity.WhenChanged,
                    //department = Entity?.Department,
                    //manager = Entity?.Manager,
                    //streetAddress=Entity?.streetAddress,
                    //physicalDeliveryOfficeName= Entity?.physicalDeliveryOfficeName,


                    sn = Entity.SN,
                    givenName = Entity.GivenName,
                    displayName = Entity.DisplayName,
                    initials = Entity.initials,
                    title = Entity.Title,
                    company = Entity.Company,
                    mail = Entity.Mail,
                    otherMailBox = Entity.OtherMailBox,
                    homePhone = Entity.HomePhone,
                    mobile = Entity.Mobile,
                    otherMobile = Entity.OtherMobile,
                    whenCreated = Entity.WhenCreated,
                    whenChanged = Entity.WhenChanged,
                    department = Entity.Department,
                    manager = Entity.Manager,
                    streetAddress=Entity.streetAddress,
                    physicalDeliveryOfficeName= Entity.physicalDeliveryOfficeName,


                    PassWords = strPassport,
                    TokenVal = tokenValue,
                    WebSiteName = strWebSiteName,
                    RedirectURL = "",
                    ReturnURL = "",
                    LogInDtime = DateTime.Now.ToStrings(),
                    LogOutDtime = DateTime.Now.AddMinutes(1).ToStrings(),
                    LogInIP = GetRequest.GetClientIP() // "172.0.0.1" + Request.UserAgent

                };

                //������վƾ֤
                object info = true;

                TokenCache.Insert(tokenValue, OAuth.Serializer(), DateTime.Now.AddMinutes(double.Parse(System.Configuration.ConfigurationManager.AppSettings["Timeout"])));


                //��ת�ط�վ
                if (Request.QueryString["BackURL"] != null)
                {
                    String RedirectURL = Server.UrlDecode(Request.QueryString["BackURL"]);
                    Response.Redirect(RedirectURL);
                }
                else
                {
                    if (OAuthDefaultURL.NotIsNullOrEmpty())
                        Response.Redirect(OAuthDefaultURL);
                }


            }
            else
            {
                LabMsg.Text = "��Ǹ���ʺŻ���������";
            }
        }

 


        bool CreateTicket(string userid, int timeOut, string username)
        {
            try
            {
                //����һ����֤Ʊ��
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(userid, true, timeOut);
                //����Ʊ��
                string cookieStr = FormsAuthentication.Encrypt(ticket);
                //����cookie
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieStr);
                //cookie���·��
                cookie.Path = FormsAuthentication.FormsCookiePath;
                Response.Cookies.Add(cookie);
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }




    }
}
