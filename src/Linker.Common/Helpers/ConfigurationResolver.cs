namespace Linker.Common.Helpers
{
    using System;
    using System.Configuration;

    /// <summary>
    /// A helper class that resolves configuration value from its key in App.config file.
    /// </summary>
    public static class ConfigurationResolver
    {
        /// <summary>
        /// Wrapper method to get the configuration with its provided key.
        /// </summary>
        /// <param name="key">The key of the configuration.</param>
        /// <returns>The value of the configuration key.</returns>
        /// <exception cref="System.ArgumentException">Throws when the provided key is not found.</exception>
        public static string GetConfig(string key)
        {
            var configValue = ConfigurationManager.AppSettings.Get(key);

            if (configValue == null)
            {
                throw new ArgumentException($"The configuration {key} is invalid and cannot be found.");
            }

            return configValue;
        }
    }
}
