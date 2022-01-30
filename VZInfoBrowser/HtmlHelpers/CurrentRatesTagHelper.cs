using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Configuration;
using VZInfoBrowser.ApplicationCore.Model;
using VZInfoBrowser.Infrastructure;
using VZInfoBrowser.Requests;

namespace VZInfoBrowser.HtmlHelpers
{
    public class CurrentRatesTagHelper : TagHelper
    {
        readonly ISettings<CurrencyRatesInfo> _data;
        public CurrentRatesTagHelper(ISettings<CurrencyRatesInfo> data) => (_data) = (data);

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string listContent = "<h3> Параметры </h3>";
            foreach (var item in  _data.Data?.Rates)
                listContent += "<li>" + item.Key + " : " + item.Value + "</li>";
            output.Content.SetHtmlContent(listContent);
        }
    }

    public static class CurrentRatesTagHelper2
    {
        public static HtmlString Table(this IHtmlHelper context, CurrencyRatesInfo data)
        {
            if(data == null || data.Rates?.Count == 0)   
                return null;

            string listContent = $"<table class=\"table_blur\">" +
                $"<tr><th> Currency </th><th> Rate </th> </tr>";
            foreach (var item in data.Rates)
            {
                listContent += $"<tr><td> {item.Key} </td><td> {item.Value} </td> </tr>"; 
            }
            listContent += $"</div>";
            return new HtmlString(listContent);
        }
    }
}
