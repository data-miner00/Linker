namespace Linker.Mvc.TagHelpers;

using Microsoft.AspNetCore.Razor.TagHelpers;

/// <summary>
/// Tag helper to create a bolded text within an element or an element itself.
/// </summary>
[HtmlTargetElement("bold")]
[HtmlTargetElement(Attributes = "bold")]
public class BoldTagHelper : TagHelper
{
    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.RemoveAll("bold");
        output.PreContent.SetHtmlContent("<strong class=\"font-bold\">");
        output.PostContent.SetHtmlContent("</strong>");
    }
}
