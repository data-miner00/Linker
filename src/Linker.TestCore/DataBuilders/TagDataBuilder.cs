namespace Linker.TestCore.DataBuilders
{
    using System;
    using Linker.Core.Models;

    public sealed class TagDataBuilder
    {
        private string id = "tag_id";
        private string name = "mytag";
        private DateTime created = new(2020, 01, 01, 0, 0, 0, DateTimeKind.Utc);
        private DateTime modified = new(2020, 01, 02, 0, 0, 0, DateTimeKind.Utc);

        public TagDataBuilder WithId(string value)
        {
            this.id = value;
            return this;
        }

        public TagDataBuilder WithName(string value)
        {
            this.name = value;
            return this;
        }

        public TagDataBuilder WithCreatedAt(DateTime value)
        {
            this.created = value;
            return this;
        }

        public TagDataBuilder WithModifiedAt(DateTime value)
        {
            this.modified = value;
            return this;
        }

        public Tag Build()
        {
            return new Tag
            {
                Id = this.id,
                Name = this.name,
                CreatedAt = this.created,
                ModifiedAt = this.modified,
            };
        }
    }
}
