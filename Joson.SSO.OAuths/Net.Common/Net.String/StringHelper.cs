using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Web;
using Microsoft.VisualBasic;

namespace Net.Common
{
    public static partial class JosonString
    {
        // Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Base64StringDecode(string input)
        {
            byte[] bytes = Convert.FromBase64String(input);
            return Encoding.UTF8.GetString(bytes);
        }

        public static string Base64StringEncode(string input)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
        }

        public static bool CheckNodePurview(string arrstr1, string arrstr2)
        {
            if (!string.IsNullOrEmpty(arrstr1) && !string.IsNullOrEmpty(arrstr2))
            {
                string[] strArray = arrstr1.Split(new char[] { Convert.ToChar(",") });
                string[] strArray2 = arrstr2.Split(new char[] { Convert.ToChar(",") });
                foreach (string str in strArray)
                {
                    foreach (string str2 in strArray2)
                    {
                        if (!string.IsNullOrEmpty(str2.Trim()) && (str2.Trim() == str.Trim()))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static string CollectionFilter(string conStr, string tagName, int fType)
        {
            string input = conStr;
            switch (fType)
            {
                case 1:
                    return Regex.Replace(input, "<" + tagName + "([^>])*>", "", RegexOptions.IgnoreCase);

                case 2:
                    return Regex.Replace(input, "<" + tagName + "([^>])*>.*?</" + tagName + "([^>])*>", "", RegexOptions.IgnoreCase);

                case 3:
                    return Regex.Replace(Regex.Replace(input, "<" + tagName + "([^>])*>", "", RegexOptions.IgnoreCase), "</" + tagName + "([^>])*>", "", RegexOptions.IgnoreCase);
            }
            return input;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string DecodeIP(long ip)
        {
            string[] strArray = new string[] { ((ip >> 0x18) & 0xffL).ToString(), ".", ((ip >> 0x10) & 0xffL).ToString(), ".", ((ip >> 8) & 0xffL).ToString(), ".", (ip & 0xffL).ToString() };
            return string.Concat(strArray);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lockIP"></param>
        /// <returns></returns>
        public static string DecodeLockIP(string lockIP)
        {
            StringBuilder builder = new StringBuilder(0x100);
            if (!string.IsNullOrEmpty(lockIP))
            {
                try
                {
                    string[] strArray = lockIP.Split(new string[] { "$$$" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        string[] strArray2 = strArray[i].Split(new string[] { "----" }, StringSplitOptions.RemoveEmptyEntries);
                        builder.Append(DecodeIP(Convert.ToInt64(strArray2[0])) + "----" + DecodeIP(Convert.ToInt64(strArray2[1])) + "\n");
                    }
                    return builder.ToString().TrimEnd(new char[] { '\n' });
                }
                catch (IndexOutOfRangeException)
                {
                    return builder.ToString();
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sip"></param>
        /// <returns></returns>
        public static double EncodeIP(string sip)
        {
            if (string.IsNullOrEmpty(sip))
            {
                return 0.0;
            }
            string[] strArray = sip.Split(new char[] { '.' });
            long num = 0L;
            foreach (string str in strArray)
            {
                byte num2;
                if (byte.TryParse(str, out num2))
                {
                    num = (num << 8) | num2;
                }
                else
                {
                    return 0.0;
                }
            }
            return (double)num;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipList"></param>
        /// <returns></returns>
        public static string EncodeLockIP(string ipList)
        {
            StringBuilder builder = new StringBuilder(0x100);
            if (!string.IsNullOrEmpty(ipList.Trim()))
            {
                string[] strArray = ipList.Split(new char[] { '\n' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (!string.IsNullOrEmpty(strArray[i]) && strArray[i].Contains("----"))
                    {
                        string[] strArray2 = strArray[i].Split(new string[] { "----" }, StringSplitOptions.RemoveEmptyEntries);
                        if (strArray2.Length < 2)
                        {
                            throw new ArgumentException("请填写正确网站黑白名单中的IP地址！");
                        }
                        if (!JosonValidate.IsIP(strArray2[0]) || !JosonValidate.IsIP(strArray2[1]))
                        {
                            throw new ArgumentException("请填写正确网站黑白名单中的IP地址！");
                        }
                        if (i == 0)
                        {
                            builder.Append(EncodeIP(strArray2[0]) + "----" + EncodeIP(strArray2[1]));
                        }
                        else
                        {
                            builder.Append(string.Concat(new object[] { "$$$", EncodeIP(strArray2[0]), "----", EncodeIP(strArray2[1]) }));
                        }
                    }
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="filterItem"></param>
        /// <returns></returns>
        public static string FilterScript(string conStr, string filterItem)
        {
            string str = conStr.Replace("\r", "{$Chr13}").Replace("\n", "{$Chr10}");
            foreach (string str2 in filterItem.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                switch (str2)
                {
                    case "Iframe":
                        str = CollectionFilter(str, str2, 2);
                        break;

                    case "Object":
                        str = CollectionFilter(str, str2, 2);
                        break;

                    case "Script":
                        str = CollectionFilter(str, str2, 2);
                        break;

                    case "Style":
                        str = CollectionFilter(str, str2, 2);
                        break;

                    case "Div":
                        str = CollectionFilter(str, str2, 3);
                        break;

                    case "Span":
                        str = CollectionFilter(str, str2, 3);
                        break;

                    case "Table":
                        str = CollectionFilter(CollectionFilter(CollectionFilter(CollectionFilter(CollectionFilter(str, str2, 3), "Tbody", 3), "Tr", 3), "Td", 3), "Th", 3);
                        break;

                    case "Img":
                        str = CollectionFilter(str, str2, 1);
                        break;

                    case "Font":
                        str = CollectionFilter(str, str2, 3);
                        break;

                    case "A":
                        str = CollectionFilter(str, str2, 3);
                        break;

                    case "Html":
                        //str = RemoveTag(str);
                        break;
                }
            }
            return str.Replace("{$Chr13}", "\r").Replace("{$Chr10}", "\n");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkStr"></param>
        /// <param name="findStr"></param>
        /// <returns></returns>
        public static bool FoundCharInArr(string checkStr, string findStr)
        {
            return FoundCharInArr(checkStr, findStr, ",");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkStr"></param>
        /// <param name="findStr"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static bool FoundCharInArr(string checkStr, string findStr, string split)
        {
            bool flag = false;
            if (string.IsNullOrEmpty(split))
            {
                split = ",";
            }
            if (string.IsNullOrEmpty(checkStr))
            {
                return false;
            }
            if (checkStr.IndexOf(split) != -1)
            {
                if (findStr.IndexOf(split) != -1)
                {
                    string[] strArray = checkStr.Split(new char[] { Convert.ToChar(split) });
                    string[] strArray2 = findStr.Split(new char[] { Convert.ToChar(split) });
                    foreach (string str in strArray)
                    {
                        foreach (string str2 in strArray2)
                        {
                            if (string.Compare(str, str2) == 0)
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (flag)
                        {
                            return flag;
                        }
                    }
                    return flag;
                }
                foreach (string str3 in checkStr.Split(new char[] { Convert.ToChar(split) }))
                {
                    if (string.Compare(str3, findStr) == 0)
                    {
                        return true;
                    }
                }
                return flag;
            }
            if (string.Compare(checkStr, findStr) == 0)
            {
                flag = true;
            }
            return flag;
        }



        /// <summary>
        /// 检查一个数组中所有的元素是否有包含于指定字符串的元素
        /// </summary>
        /// <param name="arr">存储数据数据的字串</param>
        /// <param name="toFind">要查找的字符串</param>
        /// <param name="separator">数组的分隔符</param>
        /// <returns></returns>
        public static bool FoundStringInArr(string arr, string toFind, char separator)
        {
            if (arr.IndexOf(separator) >= 0)
            {
                string[] arrTemp = arr.Split('|');
                for (int i = 0; i < arrTemp.Length; i++)
                {
                    if ((toFind.ToLower().IndexOf(arrTemp[i].ToLower()) >= 0) && (arrTemp[i].ToLower() != ""))
                        return true;
                }

            }
            else
            {
                if ((toFind.ToLower().IndexOf(arr.ToLower())) >= 0 && (arr.ToLower() != ""))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="testTxt"></param>
        /// <returns></returns>
        private static string GetGbkX(string testTxt)
        {
            if (testTxt.CompareTo("吖") >= 0)
            {
                if (testTxt.CompareTo("八") < 0)
                {
                    return "A";
                }
                if (testTxt.CompareTo("嚓") < 0)
                {
                    return "B";
                }
                if (testTxt.CompareTo("咑") < 0)
                {
                    return "C";
                }
                if (testTxt.CompareTo("妸") < 0)
                {
                    return "D";
                }
                if (testTxt.CompareTo("发") < 0)
                {
                    return "E";
                }
                if (testTxt.CompareTo("旮") < 0)
                {
                    return "F";
                }
                if (testTxt.CompareTo("铪") < 0)
                {
                    return "G";
                }
                if (testTxt.CompareTo("讥") < 0)
                {
                    return "H";
                }
                if (testTxt.CompareTo("咔") < 0)
                {
                    return "J";
                }
                if (testTxt.CompareTo("垃") < 0)
                {
                    return "K";
                }
                if (testTxt.CompareTo("嘸") < 0)
                {
                    return "L";
                }
                if (testTxt.CompareTo("拏") < 0)
                {
                    return "M";
                }
                if (testTxt.CompareTo("噢") < 0)
                {
                    return "N";
                }
                if (testTxt.CompareTo("妑") < 0)
                {
                    return "O";
                }
                if (testTxt.CompareTo("七") < 0)
                {
                    return "P";
                }
                if (testTxt.CompareTo("亽") < 0)
                {
                    return "Q";
                }
                if (testTxt.CompareTo("仨") < 0)
                {
                    return "R";
                }
                if (testTxt.CompareTo("他") < 0)
                {
                    return "S";
                }
                if (testTxt.CompareTo("哇") < 0)
                {
                    return "T";
                }
                if (testTxt.CompareTo("夕") < 0)
                {
                    return "W";
                }
                if (testTxt.CompareTo("丫") < 0)
                {
                    return "X";
                }
                if (testTxt.CompareTo("帀") < 0)
                {
                    return "Y";
                }
                if (testTxt.CompareTo("咗") < 0)
                {
                    return "Z";
                }
            }
            return testTxt;
        }

        public static string GetInitial(string str)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                builder.Append(GetOneIndex(str.Substring(i, 1)));
            }
            return builder.ToString();
        }

        private static string GetOneIndex(string testTxt)
        {
            if ((Convert.ToChar(testTxt) >= '\0') && (Convert.ToChar(testTxt) < 'Ā'))
            {
                return testTxt;
            }
            return GetGbkX(testTxt);
        }

        /// <summary>
        /// 转换为简体中文
        /// </summary>
        public static string ToSChinese(string str)
        {
            return Strings.StrConv(str, VbStrConv.SimplifiedChinese, 0);
        }

        public static bool IsIncludeChinese(string inputData)
        {
            Regex regex = new Regex("[一-龥]");
            return regex.Match(inputData).Success;
        }

        /// <summary>
        /// 转换为繁体中文
        /// </summary>
        public static string ToTChinese(string str)
        {
            return Strings.StrConv(str, VbStrConv.TraditionalChinese, 0);
        }


        /// <summary>
        /// MD5函数
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>MD5结果</returns>
        public static string MD5(string str)
        {
            byte[] b = Encoding.Default.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');
            return ret;
        }
        public static int MD5D(string strText)
        {
            using (MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider())
            {
                byte[] bytes = Encoding.Default.GetBytes(strText);
                bytes = provider.ComputeHash(bytes);
                StringBuilder builder = new StringBuilder();
                foreach (byte num in bytes)
                {
                    builder.Append(num.ToString("D").ToLower());
                }
                string input = builder.ToString();
                if (input.Length >= 9)
                {
                    input = "9" + input.Substring(1, 8);
                }
                else
                {
                    input = "9" + input;
                }
                provider.Clear();
                return Convert.ToInt32(input);
            }
        }

        public static string MD5gb2312(string input)
        {
            using (MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider())
            {
                return BitConverter.ToString(provider.ComputeHash(Encoding.GetEncoding("gb2312").GetBytes(input))).Replace("-", "").ToLower();
            }
        }

        public static string RemoveXss(string input)
        {
            string str;
            string str2;
            do
            {
                str = input;
                input = Regex.Replace(input, @"(&#*\w+)[\x00-\x20]+;", "$1;");
                input = Regex.Replace(input, "(&#x*[0-9A-F]+);*", "$1;", RegexOptions.IgnoreCase);
                input = Regex.Replace(input, "&(amp|lt|gt|nbsp|quot);", "&amp;$1;");
                input = HttpUtility.HtmlDecode(input);
            }
            while (str != input);
            do
            {
                str = input;
                input = Regex.Replace(input, @"(<[^>]+style[\x00-\x20]*=[\x00-\x20]*[^>]*?)\\([^>]*>)", "$1/$2", RegexOptions.IgnoreCase);
            }
            while (str != input);
            input = Regex.Replace(input, @"[\x00-\x08\x0b-\x0c\x0e-\x19]", "");
            input = Regex.Replace(input, "(<[^>]+[\\x00-\\x20\"'/])(on|xmlns)[^>]*>", "$1>", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, "([a-z]*)[\\x00-\\x20]*=[\\x00-\\x20]*([`'\"]*)[\\x00-\\x20]*j[\\x00-\\x20]*a[\\x00-\\x20]*v[\\x00-\\x20]*a[\\x00-\\x20]*s[\\x00-\\x20]*c[\\x00-\\x20]*r[\\x00-\\x20]*i[\\x00-\\x20]*p[\\x00-\\x20]*t[\\x00-\\x20]*:", "$1=$2nojavascript...", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, "([a-z]*)[\\x00-\\x20]*=[\\x00-\\x20]*([`'\"]*)[\\x00-\\x20]*v[\\x00-\\x20]*b[\\x00-\\x20]*s[\\x00-\\x20]*c[\\x00-\\x20]*r[\\x00-\\x20]*i[\\x00-\\x20]*p[\\x00-\\x20]*t[\\x00-\\x20]*:", "$1=$2novbscript...", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"(<[^>]+style[\x00-\x20]*=[\x00-\x20]*[^>]*?)/\*[^>]*\*/([^>]*>)", "$1$2", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, "(<[^>]+)style[\\x00-\\x20]*=[\\x00-\\x20]*([`'\"]*).*expression[\\x00-\\x20]*\\([^>]*>", "$1>", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, "(<[^>]+)style[\\x00-\\x20]*=[\\x00-\\x20]*([`'\"]*).*behaviour[^>]*>", "$1>", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, "(<[^>]+)style[\\x00-\\x20]*=[\\x00-\\x20]*([`'\"]*).*behavior[^>]*>", "$1>", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, "(<[^>]+)style[\\x00-\\x20]*=[\\x00-\\x20]*([`'\"]*).*s[\\x00-\\x20]*c[\\x00-\\x20]*r[\\x00-\\x20]*i[\\x00-\\x20]*p[\\x00-\\x20]*t[\\x00-\\x20]*:*[^>]*>", "$1>", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"</*\w+:\w[^>]*>", "noxss");
            do
            {
                str2 = input;
                input = Regex.Replace(input, "</*(applet|meta|xml|blink|link|style|script|embed|object|iframe|frame|frameset|ilayer|layer|bgsound|title|base)[^>]*>?", "no$1", RegexOptions.IgnoreCase);
            }
            while (str2 != input);
            return input;
        }


        /// <summary>
        /// <summary>
        /// 自定义的替换字符串函数
        /// </summary>
        /// </summary>
        /// <param name="SourceString"></param>
        /// <param name="SearchString"></param>
        /// <param name="ReplaceString"></param>
        /// <param name="IsCaseInsensetive">true 为指定不区分大小写的匹配。</param>
        /// <returns></returns>
        public static string ReplaceString(string SourceString, string SearchString, string ReplaceString, bool IsCaseInsensetive)
        {
            return Regex.Replace(SourceString, Regex.Escape(SearchString), ReplaceString, IsCaseInsensetive ? RegexOptions.IgnoreCase : RegexOptions.None);
        }


        public static string SHA1(string input)
        {
            using (SHA1CryptoServiceProvider provider = new SHA1CryptoServiceProvider())
            {
                return BitConverter.ToString(provider.ComputeHash(Encoding.UTF8.GetBytes(input))).Replace("-", "").ToLower();
            }
        }


        public static string SubString(string demand, int length, string substitute)
        {
            if (Encoding.Default.GetBytes(demand).Length <= length)
            {
                return demand;
            }
            ASCIIEncoding encoding = new ASCIIEncoding();
            length -= Encoding.Default.GetBytes(substitute).Length;
            int num = 0;
            StringBuilder builder = new StringBuilder();
            byte[] bytes = encoding.GetBytes(demand);
            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] == 0x3f)
                {
                    num += 2;
                }
                else
                {
                    num++;
                }
                if (num > length)
                {
                    break;
                }
                builder.Append(demand.Substring(i, 1));
            }
            builder.Append(substitute);
            return builder.ToString();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <param name="md5Value"></param>
        /// <returns></returns>
        public static bool ValidateMD5(string password, string md5Value)
        {
            if (string.Compare(password, md5Value) != 0)
            {
                return (string.Compare(password, md5Value.Substring(8, 0x10)) == 0);
            }
            return true;
        }

 

    }
}
