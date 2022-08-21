using CsvHelper.Configuration;
using Linker.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Linker.Core.CsvModels
{
    /// <summary>
    /// The base mapper class for <see cref="CsvLink"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="CsvLink"/> derivatives.</typeparam>
    public abstract class BaseClassMap<T> : ClassMap<T>
        where T : CsvLink
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseClassMap{T}"/> class.
        /// </summary>
        protected BaseClassMap()
        {
            this.Map(m => m.Id).Name("Id");
            this.Map(m => m.Description).Name("Description");
            this.Map(m => m.Url).Name("Url");
            this.Map(m => m.Category).Name("Category");
            this.Map(m => m.Tags).Name("Tags");
            this.Map(m => m.Language).Name("Language");
            this.Map(m => m.LastVisitAt).Name("LastVisitAt");
            this.Map(m => m.CreatedAt).Name("CreatedAt");
            this.Map(m => m.ModifiedAt).Name("ModifiedAt");
        }
    }
}
