using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;

namespace MapleDreams.HtmlGenerator
{
    // Source code
    // https://github.com/aspnet/Mvc/blob/release/2.2/src/Microsoft.AspNetCore.Mvc.ViewFeatures/ViewFeatures/DefaultHtmlGenerator.cs
    public class AlertHtmlGenerator : DefaultHtmlGenerator
    {
        public AlertHtmlGenerator(IAntiforgery antiforgery, IOptions<MvcViewOptions> optionsAccessor, IModelMetadataProvider metadataProvider, IUrlHelperFactory urlHelperFactory, HtmlEncoder htmlEncoder, ValidationHtmlAttributeProvider validationAttributeProvider)
            : base(antiforgery, optionsAccessor, metadataProvider, urlHelperFactory, htmlEncoder, validationAttributeProvider)
        {
        }

        public override TagBuilder GenerateValidationMessage(ViewContext viewContext, ModelExplorer modelExplorer, string expression, string message, string tag, object htmlAttributes) =>
            base.GenerateValidationMessage(viewContext, modelExplorer, expression, message, tag, htmlAttributes);

        public override TagBuilder GenerateValidationSummary(ViewContext viewContext, bool excludePropertyErrors, string message, string headerTag, object htmlAttributes)
        {
            var htmlSummary = new TagBuilder("div");
            if (!viewContext.ModelState.IsValid)
                foreach (var modelState in viewContext.ModelState)
                    if (modelState.Value.ValidationState != ModelValidationState.Valid)
                        foreach (var error in modelState.Value.Errors)
                            switch (modelState.Key.Split(new char[] { '_' })[0])
                            {
                                case "Info":
                                    {
                                        TagBuilder div = new TagBuilder("div");
                                        TagBuilder icon = new TagBuilder("i");
                                        icon.AddCssClass("fas fa-info-circle fa-fw");
                                        div.InnerHtml.AppendHtml(icon);
                                        div.InnerHtml.AppendLine(" " + error.ErrorMessage);
                                        div.AddCssClass("alert alert-info");
                                        htmlSummary.InnerHtml.AppendHtml(div);
                                        break;
                                    }
                                case "Success":
                                    {
                                        TagBuilder div = new TagBuilder("div");
                                        TagBuilder icon = new TagBuilder("i");
                                        icon.AddCssClass("fas fa-check-circle fa-fw");
                                        div.InnerHtml.AppendHtml(icon);
                                        div.InnerHtml.AppendLine(" " + error.ErrorMessage);
                                        div.AddCssClass("alert alert-success");
                                        htmlSummary.InnerHtml.AppendHtml(div);
                                        break;
                                    }
                                case "Warning":
                                    {
                                        TagBuilder div = new TagBuilder("div");
                                        TagBuilder icon = new TagBuilder("i");
                                        icon.AddCssClass("fas fa-exclamation-triangle fa-fw");
                                        div.InnerHtml.AppendHtml(icon);
                                        div.InnerHtml.AppendLine(" " + error.ErrorMessage);
                                        div.AddCssClass("alert alert-warning");
                                        htmlSummary.InnerHtml.AppendHtml(div);
                                        break;
                                    }
                                default:
                                    {
                                        TagBuilder div = new TagBuilder("div");
                                        TagBuilder icon = new TagBuilder("i");
                                        icon.AddCssClass("fas fa-exclamation-circle fa-fw");
                                        div.InnerHtml.AppendHtml(icon);
                                        div.InnerHtml.AppendLine(" " + error.ErrorMessage);
                                        div.AddCssClass("alert alert-danger");
                                        htmlSummary.InnerHtml.AppendHtml(div);
                                        break;
                                    }
                            }
                    else
                        htmlSummary.MergeAttribute("display", "none");
            return htmlSummary;
        }
    }
}
