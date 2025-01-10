using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bachelin_Merchant
{
    class SScraping
    {
        public string GetHtmlSource(string _strUrl, string _strEncode, string _strProxyServer, string _strReferer, ref CookieCollection _cookies, ref CookieContainer _cookieContainer)
        {
            string str = string.Empty;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_strUrl);
                request.Accept = "*/*";
                request.AllowAutoRedirect = true;
                request.CookieContainer = _cookieContainer;
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "GET";
                request.UserAgent = "and1_12.15.0";
                request.Timeout = 0xea60;
                if (!string.IsNullOrEmpty(_strProxyServer))
                {
                    request.Proxy = new WebProxy(_strProxyServer);
                }
                if (!string.IsNullOrEmpty(_strReferer))
                {
                    request.Referer = _strReferer;
                }
                else
                {
                    request.Referer = "http://www.google.com";
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                response.Cookies = request.CookieContainer.GetCookies(request.RequestUri);
                _cookies.Add(response.Cookies);
                StreamReader reader = null;
                if (string.IsNullOrEmpty(_strEncode))
                {
                    reader = new StreamReader(response.GetResponseStream());
                }
                else
                {
                    Encoding encoding = Encoding.GetEncoding(_strEncode);
                    reader = new StreamReader(response.GetResponseStream(), encoding);
                }
                str = reader.ReadToEnd().Replace("\n", "").Replace("\t", "").Replace("\r", "").ToString();
                reader.Close();
            }
            catch (Exception exception)
            {
                exception.HelpLink = _strUrl;
                throw;
            }
            return str;
        }

        #region -스크래핑_bin
        public byte[] ScrapData_Bin(string purl, string pprams, string pReferer, string pMethod, ref CookieCollection _col, ref CookieContainer _con)
        {
            byte[] strReturn;
            byte[] buffer = new byte[4096];
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                delegate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                        System.Security.Cryptography.X509Certificates.X509Chain chain,
                                        System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true; // **** Always accept
                };

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(purl);
                //request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request.Method = pMethod;
                byte[] byteArray = Encoding.UTF8.GetBytes(pprams);

                request.Headers.Add("Pragma", "no-cache");
                request.Accept = "text/html, application/xhtml+xml, */*";
                //request.Headers.Add("Accept-Encoding:gzip, deflate");
                request.Headers.Add("Accept-Language: ko-KR");
                request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0)";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Referer = pReferer;
                request.KeepAlive = true;

                request.ServicePoint.Expect100Continue = false;

                request.CookieContainer = _con;

                Stream dataStream;
                if (pMethod == "POST")
                {
                    request.ContentLength = byteArray.Length;

                    dataStream = request.GetRequestStream();

                    dataStream.Write(byteArray, 0, byteArray.Length);

                    dataStream.Close();
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                response.Cookies = request.CookieContainer.GetCookies(request.RequestUri);
                _col.Add(response.Cookies);
                request.CookieContainer.Add(response.Cookies);

                dataStream = response.GetResponseStream();

                //StreamReader reader = new StreamReader(dataStream, Encoding.Default);

                MemoryStream memoryStream = new MemoryStream();

                int count = 0;
                do
                {
                    count = dataStream.Read(buffer, 0, buffer.Length);
                    memoryStream.Write(buffer, 0, count);

                } while (count != 0);

                strReturn = memoryStream.ToArray();


                //reader.Close();
                dataStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                ex.HelpLink = purl;
                throw;
            }
            return strReturn;
        }
        #endregion
    }
}
