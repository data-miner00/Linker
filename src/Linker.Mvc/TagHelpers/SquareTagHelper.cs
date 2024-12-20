namespace Linker.Mvc.TagHelpers;

using Microsoft.AspNetCore.Razor.TagHelpers;

[HtmlTargetElement("square", TagStructure = TagStructure.WithoutEndTag)]
public sealed class SquareTagHelper : TagHelper
{
    public string ColorClass { get; set; } = "bg-gray-100";

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.Add("class", this.ColorClass + " w-4 h-4");
    }
}
