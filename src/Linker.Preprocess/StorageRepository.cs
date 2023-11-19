namespace Linker.Preprocess
{
    using System;
    using System.Data;
    using System.Threading.Tasks;
    using Dapper;
    using Linker.Preprocess.Models;
    using Microsoft.Extensions.Logging;

    internal class StorageRepository
    {
        private readonly IDbConnection connection;
        private readonly ILogger logger;

        public StorageRepository(IDbConnection connection, ILogger logger)
        {
            this.connection = connection;
            this.logger = logger;
        }

        public async Task AddAsync(Link link)
        {
            var insertOperation = @"INSERT INTO Links (Url, Tags) VALUES (@Url, @Tags);";

            try
            {
                await this.connection.ExecuteAsync(insertOperation, new
                {
                    link.Url,
                    link.Tags,
                });

                this.logger.LogInformation($"Item: {link.Url} has been added.");
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Item: {link.Url} insert failed. Error: {ex.Message}");
            }
        }
    }
}
