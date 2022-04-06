﻿namespace Linker.Core.Models
{
    using System;
    using System.Collections.Generic;

    public class Link
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public Category Category { get; set; }

        public string Domain { get; set; }

        public string Description { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public Language MainLanguage { get; set; } = default;

        public Aesthetics Aesthetics { get; set; }

        public bool IsSubdomain { get; set; }

        public bool IsMultilingual { get; set; }

        public DateTime LastVisitAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }
    }
}
