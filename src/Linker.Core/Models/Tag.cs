namespace Linker.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// The model for a tag.
    /// </summary>
    public class Tag
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }
    }
}
