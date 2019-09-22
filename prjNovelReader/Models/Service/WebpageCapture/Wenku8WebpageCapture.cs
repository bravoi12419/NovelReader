using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WebpageCapture
{

    class Wenku8WebpageCapture : WebpageCapture
    {
        public Wenku8WebpageCapture(){
            Encode = "GBK";
        }
        public Wenku8WebpageCapture(string url){
            Encode = "GBK";
            GetHtml(url);
            HtmlCapture(HtmlText);
        }

        public override void HtmlCapture(string webHtml)
        {
            string str = ToTraditional(webHtml);
            str = Regex.Replace(str, "(<!D).+?(<div id=\"contentmain)", "<"); //文庫去頭
            str = Regex.Replace(str, "(</ul></div>).+?(html>)", "</ul>");   //文庫去尾
            str = Regex.Replace(str, "&nbsp;", " ");
            str = Regex.Replace(str, "<br />", "\n");
            str = Regex.Replace(str, "(<ul).+?(</ul>)", string.Empty); //細切
            str = Regex.Replace(str, "<.+?>", " "); //去除其餘tag
            NovelText.Add(str);
        }
    }
}
