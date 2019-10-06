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
                var text = ToTraditional(webHtml);
                text = Regex.Replace(text, "(<!D).+?(<div id=\"contentmain)", "<"); //文庫去頭
                text = Regex.Replace(text, "(</ul></div>).+?(html>)", "</ul>");   //文庫去尾
                text = Regex.Replace(text, "&nbsp;", "-空白-");
                text = Regex.Replace(text, "<ul id=\"contentdp\">", "-換行-");
                text = Regex.Replace(text, "<br />", "-換行-");
                text = Regex.Replace(text, "(<ul).+?(</ul>)", string.Empty); //細切
                text = Regex.Replace(text, "<.+?>", " "); //去除其餘tag
                text = Regex.Replace(text,"-空白-", "&nbsp;");
                text = Regex.Replace(text, "-換行-", "<br />");
            NovelText.Add(text);
            
        }
    }
}
