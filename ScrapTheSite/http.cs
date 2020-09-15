using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SeasideResearch.LibCurlNet;
using System.IO;

namespace ScrapTheSite
{
    class http
    {
        private static Easy easy;
        private static Random rand = new Random();
        private static string SockBuff;
        private static string CookieFile = AppDomain.CurrentDomain.BaseDirectory + "cookie" + rand.Next(0, 9) + rand.Next(0, 9) + rand.Next(0, 9) + rand.Next(0, 9) + rand.Next(0, 9) + rand.Next(0, 9) + rand.Next(0, 9) + ".txt";
        public static string UserAgent = "Mozilla/5.0 (Windows NT 6.3; Trident/7.0; rv:11.0) like Gecko";
        public static string Proxy = "";
        public string referer = "";

        public void Dispose()
        {
            ClearCookies();
        }
        public string getCookieFile()
        {
            return CookieFile;
        }

        public void CurlInit()
        {
            Curl.GlobalInit((int)CURLinitFlag.CURL_GLOBAL_ALL);
        }

        public void ClearCookies()
        {
            if (File.Exists(CookieFile))
            {
                File.Delete(CookieFile);
            }

        }

        public string HTTPGet(string URL, string Proxy)
        {
            easy = new Easy();
            SockBuff = "";

            try
            {
                Easy.WriteFunction wf = new Easy.WriteFunction(OnWriteData);

                easy.SetOpt(CURLoption.CURLOPT_URL, URL);
                easy.SetOpt(CURLoption.CURLOPT_TIMEOUT, "60");
                easy.SetOpt(CURLoption.CURLOPT_WRITEFUNCTION, wf);
                easy.SetOpt(CURLoption.CURLOPT_USERAGENT, UserAgent);
                easy.SetOpt(CURLoption.CURLOPT_COOKIEFILE, CookieFile);
                easy.SetOpt(CURLoption.CURLOPT_COOKIEJAR, CookieFile);
                easy.SetOpt(CURLoption.CURLOPT_FOLLOWLOCATION, true);

                if (URL.Contains("https"))
                {
                    easy.SetOpt(CURLoption.CURLOPT_SSL_VERIFYHOST, 1);
                    easy.SetOpt(CURLoption.CURLOPT_SSL_VERIFYPEER, 0);
                }

                if (Proxy != "")
                {
                    easy.SetOpt(CURLoption.CURLOPT_PROXY, Proxy);
                    easy.SetOpt(CURLoption.CURLOPT_PROXYTYPE, CURLproxyType.CURLPROXY_HTTP);
                }

                easy.Perform();
                easy.Cleanup();

            }
            catch
            {
                Console.WriteLine("Get Request Error");
            }

            return SockBuff;
        }

        public string HTTPPost(string URL, string http_headers, string Content)
        {
            easy = new Easy();
            SockBuff = "";
            String proxy;

            try
            {
                Easy.WriteFunction wf = new Easy.WriteFunction(OnWriteData);

                easy.SetOpt(CURLoption.CURLOPT_URL, URL);
                easy.SetOpt(CURLoption.CURLOPT_TIMEOUT, "60");
                easy.SetOpt(CURLoption.CURLOPT_WRITEFUNCTION, wf);
                easy.SetOpt(CURLoption.CURLOPT_USERAGENT, UserAgent);
                easy.SetOpt(CURLoption.CURLOPT_COOKIEFILE, CookieFile);
                easy.SetOpt(CURLoption.CURLOPT_COOKIEJAR, CookieFile);
                easy.SetOpt(CURLoption.CURLOPT_HTTPHEADER, http_headers);
                easy.SetOpt(CURLoption.CURLOPT_REFERER, referer);
                //easy.SetOpt(CURLoption.CURLOPT_POSTFIELDSIZE, Content.Length); 
                easy.SetOpt(CURLoption.CURLOPT_FOLLOWLOCATION, true);
                easy.SetOpt(CURLoption.CURLOPT_STDERR, "test.txt");

                easy.SetOpt(CURLoption.CURLOPT_HTTPHEADER, 1);

                easy.SetOpt(CURLoption.CURLOPT_POST, true);
                easy.SetOpt(CURLoption.CURLOPT_POSTFIELDS, Content);


                if (URL.Contains("https"))
                {
                    easy.SetOpt(CURLoption.CURLOPT_SSL_VERIFYHOST, 1);
                    easy.SetOpt(CURLoption.CURLOPT_SSL_VERIFYPEER, 0);
                }

                if (Proxy != "")
                {
                    easy.SetOpt(CURLoption.CURLOPT_PROXY, Proxy);
                    easy.SetOpt(CURLoption.CURLOPT_PROXYTYPE, CURLproxyType.CURLPROXY_HTTP);
                }

                easy.Perform();
                easy.Cleanup();

            }
            catch
            {

            }
            return SockBuff;

        }

        public string SafeString(string data)
        {
            return Curl.Escape(data, data.Length);
        }

        public string UnSafeString(string data)
        {
            return Curl.Unescape(data, data.Length);
        }

        public static Int32 OnWriteData(Byte[] buf, Int32 size, Int32 nmemb, Object extraData)
        {
            // Console.Write(System.Text.Encoding.UTF8.GetString(  buf)); 
            SockBuff = SockBuff + System.Text.Encoding.UTF8.GetString(buf);

            return size * nmemb;
        }
    }
}
