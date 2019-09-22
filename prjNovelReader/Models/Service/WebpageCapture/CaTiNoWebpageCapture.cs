using System;
using System.Text.RegularExpressions;

namespace WebpageCapture
{
    class CaTiNoWebpageCapture : WebpageCapture
    {
        //直接使用基底類別的建構式      base.HtmlCapture(); 使用基底類別的method
        public CaTiNoWebpageCapture() : base() { }
        public CaTiNoWebpageCapture(string url) : base(url) { }      
        public override void HtmlCapture(string webHtml)
        {
            string str = webHtml;
            string line = "";
            str = Regex.Replace(str, "(<!D).+?(<!-- END -->)", string.Empty); //卡提諾第一頁 切頭
            str = Regex.Replace(str, "(<td class=\"t_f).+?(\">)", "--頭--頭--"); //卡提諾每章節開頭tag
            str = Regex.Replace(str, "</td></tr>", "~尾~~--"); //卡提諾每章節結尾tag
            str = "~--" + str + "--頭-"; //添加頭尾tag
            str = Regex.Replace(str, "(~--).+?(--頭-)", "\n\n"); //切除尾~頭之間的所有內容
            //str = Regex.Replace(str, "&nbsp;", " ");
            MatchCollection matches = Regex.Matches(str, "(?<=-頭--).+?(?=~尾~)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            foreach (Match match in matches)
            {
                line = Regex.Replace(match.Value, "<br />", "-換行-");
                line = Regex.Replace(line, "<.+?>", " ");
                line = Regex.Replace(line, "-換行-", "<br />");
                NovelText.Add(line);
            }
        }
    }
}
