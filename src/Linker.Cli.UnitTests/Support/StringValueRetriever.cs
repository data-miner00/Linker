namespace Linker.Cli.UnitTests.Support;

using System;
using System.Collections.Generic;
using TechTalk.SpecFlow.Assist;

public class StringValueRetriver : IValueRetriever
{
    public bool CanRetrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
    {
        return propertyType == typeof(string);
    }

    public object Retrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
    {
        return string.IsNullOrEmpty(keyValuePair.Value) || string.Equals(keyValuePair.Value, "null", StringComparison.OrdinalIgnoreCase)
            ? null
            : keyValuePair.Value;
    }
}
