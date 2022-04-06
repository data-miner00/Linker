namespace Linker.Core.CsvModels
{
    using System;

    public class CsvLink
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public int Category { get; set; }

        public string Domain { get; set; }

        public string Description { get; set; }

        public string Tags { get; set; }

        public int MainLanguage { get; set; }

        public int Aesthetics { get; set; }

        public bool IsSubdomain { get; set; }

        public bool IsMultilingual { get; set; }

        public DateTime LastVisitAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }
    }
}
