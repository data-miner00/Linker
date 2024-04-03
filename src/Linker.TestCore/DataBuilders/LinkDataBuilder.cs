#nullable enable
namespace Linker.TestCore.DataBuilders;

using System;
using System.Collections.Generic;
using Linker.Core.V2.Models;

public sealed class LinkDataBuilder : ITestDataBuilder<Link>
{
    private string id = Guid.NewGuid().ToString();
    private string name = "Link name";
    private string url = "www.google.com";
    private LinkType linkType = LinkType.None;
    private Category category;
    private string? description = "Description";
    private IEnumerable<string> tags = ["tag1", "tag2"];
    private Language language;
    private Rating rating;
    private string addedBy = Guid.NewGuid().ToString();
    private string domain = "google.com";
    private Aesthetics aesthetics;
    private bool isSubdomain;
    private bool isMultilingual;
    private bool isResource;
    private string? country;
    private string? keyPersonName;
    private Grammar grammar;
    private Visibility visibility;
    private DateTime createdAt = DateTime.Now;
    private DateTime modifiedAt = DateTime.Now;

    public LinkDataBuilder WithId(string value)
    {
        this.id = value;
        return this;
    }

    public LinkDataBuilder WithName(string value)
    {
        this.name = value;
        return this;
    }

    public LinkDataBuilder WithUrl(string value)
    {
        this.url = value;
        return this;
    }

    public LinkDataBuilder WithType(LinkType value)
    {
        this.linkType = value;
        return this;
    }

    public LinkDataBuilder WithCategory(Category value)
    {
        this.category = value;
        return this;
    }

    public LinkDataBuilder WithRating(Rating value)
    {
        this.rating = value;
        return this;
    }

    public LinkDataBuilder WithDescription(string value)
    {
        this.description = value;
        return this;
    }

    public LinkDataBuilder WithTags(IEnumerable<string> value)
    {
        this.tags = value;
        return this;
    }

    public LinkDataBuilder WithLanguage(Language value)
    {
        this.language = value;
        return this;
    }

    public LinkDataBuilder WithAddedBy(string value)
    {
        this.addedBy = value;
        return this;
    }

    public LinkDataBuilder WithDomain(string value)
    {
        this.domain = value;
        return this;
    }

    public LinkDataBuilder WithAesthetics(Aesthetics value)
    {
        this.aesthetics = value;
        return this;
    }

    public LinkDataBuilder WithIsSubdomain(bool value)
    {
        this.isSubdomain = value;
        return this;
    }

    public LinkDataBuilder WithIsMultilingual(bool value)
    {
        this.isMultilingual = value;
        return this;
    }

    public LinkDataBuilder WithIsResource(bool value)
    {
        this.isResource = value;
        return this;
    }

    public LinkDataBuilder WithCountry(string value)
    {
        this.country = value;
        return this;
    }

    public LinkDataBuilder WithKeyPersonName(string value)
    {
        this.keyPersonName = value;
        return this;
    }

    public LinkDataBuilder WithGrammar(Grammar value)
    {
        this.grammar = value;
        return this;
    }

    public LinkDataBuilder WithVisibility(Visibility value)
    {
        this.visibility = value;
        return this;
    }

    public LinkDataBuilder WithCreatedAt(DateTime value)
    {
        this.createdAt = value;
        return this;
    }

    public LinkDataBuilder WithModifiedAt(DateTime value)
    {
        this.modifiedAt = value;
        return this;
    }

    public Link Build()
    {
        return new()
        {
            Id = this.id,
            Name = this.name,
            Url = this.url,
            Type = this.linkType,
            Category = this.category,
            Description = this.description,
            Tags = this.tags,
            Language = this.language,
            Rating = this.rating,
            AddedBy = this.addedBy,
            Domain = this.domain,
            Aesthetics = this.aesthetics,
            IsSubdomain = this.isSubdomain,
            IsMultilingual = this.isMultilingual,
            IsResource = this.isResource,
            Country = this.country,
            KeyPersonName = this.keyPersonName,
            Grammar = this.grammar,
            Visibility = this.visibility,
            CreatedAt = this.createdAt,
            ModifiedAt = this.modifiedAt,
        };
    }
}
