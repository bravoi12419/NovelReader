using System;
using System.Collections.Generic;

namespace WebpageCapture
{
    abstract class AbWebpageCapture
    {
        public List<string> NovelText { get; protected set; } = new List<string>();
        public string HtmlText { get; protected set; }
        public string Encode { get; protected set; } = "utf-8";
        //取得網址url 存取Html文檔
        public abstract void GetHtml(string url);
        public abstract void HtmlCapture(string webHtml);
    }
}
