using System;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace WebpageCapture
{
    class WebpageCapture : AbWebpageCapture
    {
        public WebpageCapture(){}
        public WebpageCapture(string url)
        {
            GetHtml(url);
            HtmlCapture(HtmlText);
        }
        // 將方法密封，之後的衍生類別中，GetHtml不會再是虛擬也無法覆蓋
        public override void GetHtml(string url)
        {
            String str = "";
            var webRequest = WebRequest.Create(url);
            var webResponse = webRequest.GetResponse();
            var streamReader = new StreamReader(webResponse.GetResponseStream(), Encoding.GetEncoding(Encode));
            var stringBuilder = new StringBuilder();
            while ((str = streamReader.ReadLine()) != null)
            {
                stringBuilder.Append(str);
            }
            HtmlText = stringBuilder.ToString();
        }
        public override void HtmlCapture(string webHtml) {

        }
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int LCMapString(int locale, int dwMapFlags, string lpSrcStr, int cchSrc, [Out] string lpDestStr, int cchDest);
        public static string ToTraditional(string source)
        {
            var t = new string(' ', source.Length);
            LCMapString(0x0800, 0x04000000, source, source.Length, t, source.Length);
            return t;
        }
    }
}
