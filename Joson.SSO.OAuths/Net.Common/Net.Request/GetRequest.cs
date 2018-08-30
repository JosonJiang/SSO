using System;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Collections.Generic;

namespace Net.Common
{
    /// <summary>
    /// Request������
    /// </summary>
    public partial class GetRequest
    {
        #region  Static Property Get BaseUrl(��̬���Ի�ȡURL��ַ)
        /// <summary>
        /// �����̬���Եĵ��ñ��������´��뷽������
        /// �������:
        /// Response.Write(UIHelper.BaseUrl);
        /// </summary>
        public static string BaseUrl
        {
            get
            {
                //strBaseUrl���ڴ洢URL��ַ
                string strBaseUrl = "";
                //��ȡ��ǰHttpContext�µĵ�ַ
                strBaseUrl += "http://" + HttpContext.Current.Request.Url.Host;
                //����˿ڲ���80�Ļ�����ô��������˿�
                if (HttpContext.Current.Request.Url.Port.ToString() != "80")
                {
                    strBaseUrl += ":" + HttpContext.Current.Request.Url.Port;
                }
                strBaseUrl += HttpContext.Current.Request.ApplicationPath;

                return strBaseUrl + "/";
            }
        }
        #endregion



        #region �жϵ�ǰҳ���Ƿ���յ���Post����
        /// <summary>
        /// �жϵ�ǰҳ���Ƿ���յ���Post����
        /// </summary>
        /// <returns>�Ƿ���յ���Post����</returns>

        public static bool IsPost()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("POST");
        }

        #endregion

        #region �жϵ�ǰҳ���Ƿ���յ���Get����

        /// �жϵ�ǰҳ���Ƿ���յ���Get����
        /// </summary>
        /// <returns>�Ƿ���յ���Get����</returns>

        public static bool IsGet()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("GET");
        }

        #endregion

        #region �ж��Ƿ�����������������

        /// <summary>
        /// �ж��Ƿ�����������������
        /// </summary>
        /// <returns>�Ƿ�����������������</returns>
        public static bool IsSearchEnginesGet()
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
            {
                string[] strArray = new string[] { "google", "yahoo", "msn", "baidu", "sogou", "sohu", "sina", "163", "lycos", "tom", "yisou", "iask", "soso", "gougou", "zhongsou" };
                string str = HttpContext.Current.Request.UrlReferrer.ToString().ToLower();
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (str.IndexOf(strArray[i]) >= 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        #region �жϵ�ǰ�����Ƿ�������������

        /// <summary>
        /// �жϵ�ǰ�����Ƿ�������������
        /// </summary>
        /// <returns>��ǰ�����Ƿ�������������</returns>

        public static bool IsBrowserGet()
        {
            string[] strArray = new string[] { "ie", "opera", "netscape", "mozilla", "konqueror", "firefox" };
            string str = HttpContext.Current.Request.Browser.Type.ToLower();
            for (int i = 0; i < strArray.Length; i++)
            {
                if (str.IndexOf(strArray[i]) >= 0)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region ����ָ���ķ�����������Ϣ

        /// <summary>
        /// ����ָ���ķ�����������Ϣ
        /// </summary>
        /// <param name="strName">������������</param>
        /// <returns>������������Ϣ</returns>
        public static string GetServerString(string strName)
        {
            if (HttpContext.Current.Request.ServerVariables[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.ServerVariables[strName].ToString();
        }

        #endregion

        #region ������һ��ҳ��ĵ�ַ

        /// <summary>
        /// ������һ��ҳ��ĵ�ַ
        /// </summary>
        /// <returns>��һ��ҳ��ĵ�ַ</returns>
        public static string GetUrlReferrer()
        {
            string retVal = null;

            try
            {
                retVal = HttpContext.Current.Request.UrlReferrer.ToString();
            }
            catch { }

            if (retVal == null)
                return "";

            return retVal;
        }

        #endregion

        #region �ͻ���-��������

        /// <summary>
        /// �õ�����ͷ
        /// </summary>
        /// <returns></returns>
        public static string GetHost()
        {
            return HttpContext.Current.Request.Url.Host;
        }


        #region ��ȡ��վ����ʵ·��
        /// <summary>
        /// ��ȡ��վ����ʵ·��
        /// </summary>
        /// <returns></returns>
        public static string GetTrueWebSitePath()
        {
            string path = HttpContext.Current.Request.Path;
            if (path.LastIndexOf("/") != path.IndexOf("/"))
            {
                path = path.Substring(path.IndexOf("/"), path.LastIndexOf("/") + 1);
            }
            else
            {
                path = "/";
            }
            return path;

        }
        #endregion

        #region ��õ�ǰ����·��
        /// <summary>
        /// ��õ�ǰ����·��
        /// </summary>
        /// <param name="strPath">ָ����·��</param>
        /// <returns>����·��</returns>
        public static string GetMapPath(string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //��web��������
            {
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }
        #endregion

        #region  ��ȡ��ǰ������ַ�˿�
        /// <summary>
        /// ��ȡ��ǰ������ַ�˿�
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentDomainPort()
        {
            string DomainPort = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT"].ToString();

            return DomainPort;

        }

        #endregion

        #region ��ȡ��ǰ���� ����������������DNS��ַ��IP��ַ
        /// <summary>
        /// ��ȡ��ǰ���� ����������������DNS��ַ��IP��ַ
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentDomain()
        {
            string domain = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString();
            string Port = GetCurrentDomainPort() == "80" ? "" : ":" + GetCurrentDomainPort();
            return domain + Port;

            // "www.163.com"

            //int index = domain.IndexOf('.') + 1;
            //return domain.Substring(index, domain.Length - index);
            //return "163.com";
        }
        #endregion

        #region  ��ȡ��ǰ������ַ ����������������DNS��ַ��IP��ַ
        /// <summary>
        /// ��ȡ��ǰ������ַ ����������������DNS��ַ��IP��ַ
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentDomainURL()
        {
            string GetCurrentDomainURL = string.Empty;

            try { GetCurrentDomainURL = "http://" + GetCurrentDomain(); }

            catch (Exception e)
            {
                string Domain = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString();

                GetCurrentDomainURL = "http://" + Domain + (GetCurrentDomainPort() == "80" ? "" : ":" + GetCurrentDomainPort());
            }

            return GetCurrentDomainURL;

        }
        #endregion

        #region ��ȡ��ǰ������ַ �õ���ǰ��������ͷ
        /// <summary>
        /// ��ȡ��ǰ������ַ �õ���ǰ��������ͷ
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentFullHost()
        {
            HttpRequest request = HttpContext.Current.Request;
            if (!request.Url.IsDefaultPort)
            {
                return string.Format("{0}:{1}", request.Url.Host, request.Url.Port.ToString());
            }
            return request.Url.Host;
        }
        #endregion

        #region ��ȡ��ǰ���ط�������ַ
        /// <summary>
        /// ��ȡ��ǰ���ط�������ַ
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentHost()
        {
            string domain = System.Web.HttpContext.Current.Request.ServerVariables["Http_Host"].ToString();

            return domain;

        }
        #endregion

        #region ��ȡ��ǰ���ط�������ַ
        /// <summary>
        /// ��ȡ��ǰ���ط�������ַ
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentServer()
        {
            string domain = System.Web.HttpContext.Current.Request.ServerVariables["Url"].ToString();

            return domain;

        }
        #endregion

        #region ��ȡ��ǰ���ط�������Ӧ�ó���Ԫ���ݿ�·����Ӧ������·��
        /// <summary>
        /// ��ȡ��ǰ���ط�������Ӧ�ó���Ԫ���ݿ�·����Ӧ������·��
        /// </summary>
        /// <returns></returns>
        public static string GetServerPhysicalPath()
        {
            string domain = System.Web.HttpContext.Current.Request.ServerVariables["Appl_Physical_Path"].ToString();

            return domain;

        }

        #endregion

        #region ��ȡ��վ��ַ������
        /// <summary>
        /// ��ȡ��վ��ַ������
        /// </summary>
        public static string GetDomainByHost(string host)
        {
            host = host.Replace("http://", "").Split('/')[0];
            string[] arr = host.Split('.');
            return host;
        }

        #endregion

        #region ��ȡ��վ��ַ������
        /// <summary>
        /// ��ȡ��վ��ַ������
        /// </summary>
        public static string GetDomainByHostWithOutWWW(string host)
        {
            host = host.Replace("http://", "").Split('/')[0];
            string[] arr = host.Split('.');

            if (arr[0] == "www")
                return host.Substring(arr[0].Length + 1, host.Length - arr[0].Length - 1);
            else
                return host;
        }

        #endregion

        #region ����������ȡIP
        /// <summary>
        /// ����������ȡIP
        /// </summary>
        /// <param name="ToEmail"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static string GetIPByHost(string host)
        {
            host = GetDomainByHost(host);
            //Dns.GetHostAddresses(host);
            IPHostEntry dnstoip = new IPHostEntry();
            dnstoip = Dns.Resolve(host);
            string ip = dnstoip.AddressList[0].ToString();
            return ip;
        }
 
        #endregion

        #endregion

        #region ��������վ��ַ��IP��ѯ

        /// <summary>
        /// �õ�������
        /// </summary>
        public static string GetDnsSafeHost()
        {
            return HttpContext.Current.Request.Url.DnsSafeHost;
        }


        #region �ͻ���
        /// <summary>
        /// ���ؽ�������ķ�������ַ
        /// </summary>
        /// <returns></returns>
        public static string GetRemoteServer()
        {
            string domain = System.Web.HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"].ToString();

            return domain;

        }

        /// <summary>
        /// ��ȡԶ�̷��������Զ��������IP��ַ
        /// </summary>
        /// <returns></returns>
        public static string GetRemoteIP()
        {
            string domain = System.Web.HttpContext.Current.Request.ServerVariables["Remote_Addr"].ToString();

            return domain;

        }


        /// <summary>
        /// ��ȡԶ�̷��������Զ����������
        /// </summary>
        /// <returns></returns>
        public static string GetRemoteHostName()
        {
            string domain = System.Web.HttpContext.Current.Request.ServerVariables["Remote_Host"].ToString();

            return domain;

        }

        /// <summary>
        /// ��ȡ��ַ
        /// </summary>
        /// <param name="url">��ַ</param>
        /// <returns>��ʵ��ַ</returns>
        public static string GetResolvedUrl(  string url)
        {
            return ((System.Web.UI.Page)HttpContext.Current.Handler).ResolveUrl(url);
        }


        #endregion

        #region �����û�������ҳ�汣��Cookie
        /// <summary>
        /// �����û�������ҳ�汣��Cookie
        /// </summary>
        /// <param name="page"></param>
        public static void WriteLastViewPage(string page)
        {
            var cookie = HttpContext.Current.Request.Cookies["page"];
            if (cookie == null)
            {
                cookie = new HttpCookie("page");
            }
            cookie.Value = page;
            HttpContext.Current.Response.Cookies.Add(cookie);

        }
        #endregion

        #region ��ȡ�û���ʵIP��ַ
        /// <summary>
        /// ��ȡ�û���ʵIP��ַ
        /// </summary>
        /// <returns>�����û���ʵIP</returns>
        public static string GetUserRealIp()
        {
            string user_IP = "";

            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                user_IP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else
            {
                user_IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            return user_IP;
        }



        #endregion

        #region ��ȡ�û�IP
        /// <summary>
        /// ��ȡ�û�IP
        /// </summary>
        /// <returns></returns>
        public static List<string> IpAddress()
        {
            List<string> ipList = new List<string>();
            try
            {
                string localIP = "";
                System.Net.IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

                foreach (IPAddress ip in addressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        localIP = ip.ToString();
                        ipList.Add(localIP);
                    }
                }
            }
            catch (Exception)
            {
                ipList.Add("0.0.0.0");
            }

            return ipList;
        }

        #endregion

        #region ��ȡ�û�IP

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetHostIPAddress()
        {
            List<string> ipList = IpAddress();
            if (ipList.Count <= 0) return null;
            return string.Join("|", ipList.ToArray());
        }


        /// <summary>
        /// ��ȡ�û�IP
        /// </summary>
        /// <returns></returns>
        public static string GetIPAddress()
        {
            string strIP = string.Empty;

            List<string> ipList = IpAddress();
            for (int i = 0; i < ipList.Count; i++)
            {
                strIP += ipList[i] + ",";
            }

            return strIP.Substring(0, strIP.Length-1);

        }
        #endregion

        #region ��ȡ�ͻ���IP
        /// <summary>
        /// ��ȡ�ͻ���IP
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            string userHostAddress = string.Empty;
            userHostAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Current.Request.UserHostAddress;
            }
            if (!(!string.IsNullOrEmpty(userHostAddress) && JosonValidate.IsIP(userHostAddress)))
            {
                return "127.0.0.1";
            }
            return userHostAddress;
        }

        #endregion

        #region ȡ�ÿͻ�������IPv4λַ
        /// <summary>
        /// ȡ�ÿͻ������� IPv4 λַ(�ɻ�ȡ�� IPv6 λַ���� DNS ��¼)
        /// </summary>
        /// <returns></returns>
        public static string GetClientIPv4Address()
        {
            string ipv4 = String.Empty;

            foreach (IPAddress ip in Dns.GetHostAddresses(GetClientIP()))
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    ipv4 = ip.ToString();
                    break;
                }
            }

            if (ipv4 != String.Empty)
            {
                return ipv4;
            }

            // ԭ��ʹ�� Dns.GetHostName ����ȡ�ص��� Server ����Ϣ���� Client �ˡ�
            // ��дΪ���� Dns.GetHostEntry �������ɻ�ȡ�� IPv6 λַ���� DNS ��¼��
            // ����һ�жϺ���Ϊ IPv4 Э�飬����תΪ IPv4 λַ��
            foreach (IPAddress ip in Dns.GetHostEntry(GetClientIP()).AddressList)
            //foreach (IPAddress ip in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    ipv4 = ip.ToString();
                    break;
                }
            }

            return ipv4;
        }



        /// <summary>
        /// ȡ�ÿͻ�������λַ
        /// </summary>
        public static string GetClientIP()
        {
            if (null == HttpContext.Current.Request.ServerVariables["HTTP_VIA"])
            {
                return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
        }


        #endregion


        public static string GetRealIP()
        {
            return GetRequest.GetIP();
        }

        #endregion

        #region ·��ת��ΪURL��ַ
        /// <summary>
        /// ·��ת��ΪURL��ַ
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string UnrestrictedUrl(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }
            if (VirtualPathUtility.IsAppRelative(path))
            {
                path = VirtualPathUtility.ToAbsolute(path);
            }
            int num = 80;
            string host = HttpContext.Current.Request.Url.Host;
            string str2 = (num != 80) ? string.Format(":{0}", num) : "";
            Uri baseUri = new Uri(string.Format("http://{0}{1}", host, str2));
            return new Uri(baseUri, path).ToString();
        }

        #endregion

        //private static string GetDnsRealHost()
        //{
        //    string host = HttpContext.Current.Request.Url.DnsSafeHost;
        //    string ts = string.Format(GetUrl(), host, GetServerString("LOCAL_ADDR"), Utils.GetVersion());
        //    if (!string.IsNullOrEmpty(host) && host != "localhost")
        //    {
        //        Utils.GetDomainStr("dt_cache_domain_info", ts);
        //    }
        //    return host;
        //}

        #region ��ȡ��ǰ�����ԭʼ URL

        /// <summary>
        /// ��ȡ��ǰ�����ԭʼ URL(URL ������Ϣ֮��Ĳ���,������ѯ�ַ���(�������))
        /// </summary>
        /// <returns>ԭʼ URL</returns>
        public static string GetRawUrl()
        {
            return HttpContext.Current.Request.RawUrl;
        }
        #endregion


        /// <summary>
        /// ��ȡͼƬ���ļ��ĵ�ַ
        /// ����ֵ��Ҫ Server.MapPath(FilePath)
        /// </summary>
        /// <param name="ContentPath"></param>
        /// <param name="ServerMapPath"></param>
        /// <returns></returns>
        public static string GetFliePath(string ContentPath, string ServerMapPath)
        {
            string FilePath = ContentPath;

            FilePath = ContentPath.IndexOf("http://") > -1 || ContentPath.IndexOf("https://") > -1
                       ? ContentPath : (ServerMapPath + FilePath);

            return FilePath;

        }


        #region ��ȡͼƬ�ߴ�
        /// <summary>
        /// ��ȡͼƬ�ߴ�
        /// ��֧��Զ��ͼƬ
        /// Զ��ͼƬ��ͼƬ������ʱ 
        /// ���� Width=0 Height=0
        /// </summary>
        /// <param name="ImgPath"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        public static bool GetImgSize(string ImgPath, out int Width, out int Height)
        {
            Width = 0;
            Height = 0;

            if (ImgPath.IndexOf("http://") > -1)
            {
                // "��֧��Զ��ͼƬ��С";
                return false;
            }
            else
            {
                if (System.IO.File.Exists(ImgPath))
                {
                    System.Drawing.Image Img = System.Drawing.Image.FromFile(ImgPath);

                    Width = Img.Width;
                    Height = Img.Height;

                    return true;
                }
            }
            return false;

        }
        #endregion

        #region ��õ�ǰ����Url��ַ
        /// <summary>
        /// ��õ�ǰ����Url��ַ
        /// </summary>
        /// <returns>��ǰ����Url��ַ</returns>
        public static string GetUrl()
        {
            return HttpContext.Current.Request.Url.ToString();
        }

        #endregion

        #region ��ȡ��վ��ʵ·��
        /// <summary>
        /// ��ȡ��վ��ʵ·��
        /// </summary>
        /// <returns></returns>
        public static string GetTrueSitePath()
        {
            string path = HttpContext.Current.Request.Path;
            if (path.LastIndexOf("/") != path.IndexOf("/"))
            {
                return path.Substring(path.IndexOf("/"), path.LastIndexOf("/") + 1);
            }
            return "/";
        }
        #endregion

        #region ��ȡ�ʼ�����������
        /// <summary>
        /// ��ȡ�ʼ�����������
        /// </summary>
        /// <param name="strEmail"></param>
        /// <returns></returns>
        public static string GetEmailHostName(string strEmail)
        {
            if (strEmail.IndexOf("@") < 0)
            {
                return "";
            }
            return strEmail.Substring(strEmail.LastIndexOf("@")).ToLower();
        }

        #endregion

        #region ��ȡҳ��Դ����
        /// <summary>
        /// ��ȡҳ��Դ����
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetPageResouceCode(string url)
        {
            try
            {
                WebRequest wrt = WebRequest.Create(url);
                WebResponse wrse = wrt.GetResponse();
                Stream strM = wrse.GetResponseStream();
                StreamReader SR = new StreamReader(strM, Encoding.UTF8);
                string strallstrm = SR.ReadToEnd();
                return strallstrm;
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region ���Զ���ַ���
        ///// <summary>
        ///// ���Զ���ַ���
        ///// </summary>
        //public static string GetDomainString(string key, string uriPath)
        //{
        //    string result = JosonCache.GetCache(key) as string;
        //    if (result == null)
        //    {
        //        System.Net.WebClient client = new System.Net.WebClient();
        //        try
        //        {
        //            client.Encoding = System.Text.Encoding.UTF8;
        //            result = client.DownloadString(uriPath);
        //        }
        //        catch
        //        {
        //            result = "��ʱ�޷�����!";
        //        }
        //        JosonCache.SetCache(key, result, 60);
        //    }

        //    return result;
        //}
        #endregion



    }
}