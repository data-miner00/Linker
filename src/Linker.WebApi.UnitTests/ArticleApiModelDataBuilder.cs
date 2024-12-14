namespace Linker.WebApi.UnitTests;

using Linker.WebApi.ApiModels;
using System;
using System.Collections.Generic;

public sealed class ArticleApiModelDataBuilder
{
    private string id = Guid.NewGuid().ToString();
    private string url = "https://google.com";
    private string category = "Education";
    private string description = "hello world";
    private IEnumerable<string> tags = new List<string>() { "tag1", "tag2" };
    private string language = "en";
    private DateTime lastVisitAt = DateTime.Now;
    private DateTime createdAt = DateTime.Now;
    private DateTime modifiedAt = DateTime.Now;
    private string createdBy = Guid.NewGuid().ToString();

    private string title = "title";
    private string author = "Author";
    private int year = 2004;
    private bool watchLater;
    private string domain = "google.com";
    private string grammar = "Normal";

    public ArticleApiModelDataBuilder WithId(string value)
    {
        this.id = value;
        return this;
    }

    public ArticleApiModelDataBuilder WithUrl(string value)
    {
        this.url = value;
        return this;
    }

    public ArticleApiModelDataBuilder WithCategory(string value)
    {
        this.category = value;
        return this;
    }

    public ArticleApiModelDataBuilder WithDescription(string value)
    {
        this.description = value;
        return this;
    }

    public ArticleApiModelDataBuilder WithTags(IEnumerable<string> value)
    {
        this.tags = value;
        return this;
    }

    public ArticleApiModelDataBuilder WithLanguage(string value)
    {
        this.language = value;
        return this;
    }

    public ArticleApiModelDataBuilder WithLastVisitAt(DateTime value)
    {
        this.lastVisitAt = value;
        return this;
    }

    public ArticleApiModelDataBuilder WithCreatedAt(DateTime value)
    {
        this.createdAt = value;
        return this;
    }

    public ArticleApiModelDataBuilder WithModifiedAt(DateTime value)
    {
        this.modifiedAt = value;
        return this;
    }

    public ArticleApiModelDataBuilder WithCreatedBy(string value)
    {
        this.createdBy = value;
        return this;
    }

    public ArticleApiModelDataBuilder WithTitle(string value)
    {
        this.title = value;
        return this;
    }

    public ArticleApiModelDataBuilder WithAuthor(string value)
    {
        this.author = value;
        return this;
    }

    public ArticleApiModelDataBuilder WithYear(int value)
    {
        this.year = value;
        return this;
    }

    public ArticleApiModelDataBuilder WithWatchLater(bool value)
    {
        this.watchLater = value;
        return this;
    }

    public ArticleApiModelDataBuilder WithDomain(string value)
    {
        this.domain = value;
        return this;
    }

    public ArticleApiModelDataBuilder WithGrammar(string value)
    {
        this.grammar = value;
        return this;
    }

    public ArticleApiModel Build()
    {
        return new ArticleApiModel
        {
            Id = this.id,
            Url = this.url,
            Category = this.category,
            Description = this.description,
            Tags = this.tags,
            Language = this.language,
            LastVisitAt = this.lastVisitAt,
            CreatedAt = this.createdAt,
            ModifiedAt = this.modifiedAt,
            CreatedBy = this.createdBy,
            Title = this.title,
            Author = this.author,
            Year = this.year,
            WatchLater = this.watchLater,
            Domain = this.domain,
            Grammar = this.grammar,
        };
    }
}
