namespace Linker.TestCore.DataBuilders
{
    using Linker.Core.ApiModels;
    using Linker.Core.Models;

    public sealed class UpdateArticleRequestDataBuilder
    {
        private string title = "title";
        private string url = "https://google.com/article.html";
        private Category category = Category.Health;
        private string description = "description";
        private string author = "Martin Luther";
        private Language language = Language.English;
        private Grammar grammar = Grammar.Unknown;
        private int year = 1997;
        private bool watchLater = false;

        public UpdateArticleRequestDataBuilder WithTitle(string title)
        {
            this.title = title;
            return this;
        }

        public UpdateArticleRequestDataBuilder WithUrl(string url)
        {
            this.url = url;
            return this;
        }

        public UpdateArticleRequestDataBuilder WithCategory(Category category)
        {
            this.category = category;
            return this;
        }

        public UpdateArticleRequestDataBuilder WithDescription(string description)
        {
            this.description = description;
            return this;
        }

        public UpdateArticleRequestDataBuilder WithAuthor(string author)
        {
            this.author = author;
            return this;
        }

        public UpdateArticleRequestDataBuilder WithLanguage(Language language)
        {
            this.language = language;
            return this;
        }

        public UpdateArticleRequestDataBuilder WithGrammar(Grammar grammar)
        {
            this.grammar = grammar;
            return this;
        }

        public UpdateArticleRequestDataBuilder WithYear(int year)
        {
            this.year = year;
            return this;
        }

        public UpdateArticleRequestDataBuilder WithWatchLater(bool watchLater)
        {
            this.watchLater = watchLater;
            return this;
        }

        public UpdateArticleRequest Build()
        {
            return new UpdateArticleRequest
            {
                Title = this.title,
                Url = this.url,
                Category = this.category,
                Description = this.description,
                Author = this.author,
                Language = this.language,
                Grammar = this.grammar,
                Year = this.year,
                WatchLater = this.watchLater,
            };
        }
    }
}
