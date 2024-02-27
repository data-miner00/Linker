namespace Linker.Mvc.TagHelpers;

using Microsoft.AspNetCore.Razor.TagHelpers;

/// <summary>
/// Tag helper to create an email anchor.
/// </summary>
[HtmlTargetElement("email", TagStructure = TagStructure.NormalOrSelfClosing)]
public class EmailTagHelper : TagHelper
{
    private const string EmailDomain = "gmail.com";

    /// <summary>
    /// Gets or sets the email recipient.
    /// </summary>
    [HtmlAttributeName("recipient")]
    required public string MailTo { get; set; }

    /// <summary>
    /// Gets the full email address.
    /// </summary>
    public string EmailAddress => $"{this.MailTo}@{EmailDomain}";

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        ConvertToAnchorLink(output);

        output.Attributes.SetAttribute("href", "mailto:" + this.EmailAddress);
        output.Content.SetContent(this.EmailAddress);
    }

    private static void ConvertToAnchorLink(TagHelperOutput output)
    {
        output.TagName = "a";
    }
}
