namespace Linker.TestCore.Attributes;

using System;
using System.Runtime.CompilerServices;
using Xunit;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class PrefixedFactAttribute : FactAttribute
{
    private const char Delimiter = '_';

    public PrefixedFactAttribute(string prefix, [CallerMemberName]string methodName = null)
    {
        this.DisplayName = string.Concat(prefix, Delimiter, methodName);
    }
}
