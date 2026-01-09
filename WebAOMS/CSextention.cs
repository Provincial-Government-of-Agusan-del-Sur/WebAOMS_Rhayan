using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace WebAOMS
{
    public static class CSextentiond
    {
        public static MvcHtmlString ValidationSummaryJQuerys(this HtmlHelper htmlHelper, string message, IDictionary<string, object> htmlAttributes)
        {
            if (!htmlHelper.ViewData.ModelState.IsValid)
                return htmlHelper.ValidationSummary(message, htmlAttributes);


            StringBuilder sb = new StringBuilder(Environment.NewLine);

            var divBuilder = new TagBuilder("div");
            divBuilder.MergeAttributes<string, object>(htmlAttributes);
            divBuilder.AddCssClass(HtmlHelper.ValidationSummaryValidCssClassName); // intentionally add VALID css class

            if (!string.IsNullOrEmpty(message))
            {
                //--------------------------------------------------------------------------------
                // Build an EMPTY error summary message <span> tag
                //--------------------------------------------------------------------------------
                var spanBuilder = new TagBuilder("span");
                spanBuilder.SetInnerText(message);
                sb.Append(spanBuilder.ToString(TagRenderMode.Normal)).Append(Environment.NewLine);
            }

            divBuilder.InnerHtml = sb.ToString();
            return MvcHtmlString.Create(divBuilder.ToString(TagRenderMode.Normal));
        }
    }
}
