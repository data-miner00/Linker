namespace Linker.Common.UnitTests.Helpers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Linker.Common.Helpers;
    using Xunit;

    public sealed class GuardTests
    {
        internal sealed class TestSubject
        {
            [Required]
            [Range(0, 5)]
            public int TestProperty { get; set; }
        }

        [Fact]
        public void ThrowIfNull_NullParameter_Throws()
        {
            object mockObj = null;
            Assert.Throws<ArgumentNullException>(() => Guard.ThrowIfNull(mockObj));
        }

        [Fact]
        public void ThrowIfNull_ValidParameter_Pass()
        {
            object mockObj = new object();
            var result = Guard.ThrowIfNull(mockObj);
            Assert.Equal(result, mockObj);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ThrowIfNullOrWhitespace_NullOrEmptyString_Throws(string param)
        {
            Assert.Throws<ArgumentException>(() => Guard.ThrowIfNullOrWhitespace(param));
        }

        [Fact]
        public void ThrowIfValidationFailed_BadAttribute_Throws()
        {
            var testObj = new TestSubject { TestProperty = 42, };
            Assert.Throws<ValidationException>(() => Guard.ThrowIfValidationFailed(testObj));
        }

        [Fact]
        public void ThrowIfValidationFailed_ValidAttribute_Pass()
        {
            var testObj = new TestSubject { TestProperty = 3, };
            Guard.ThrowIfValidationFailed(testObj);
        }

        [Fact]
        public void ThrowIfDefault_DefaultValue_Throws()
        {
            var defaultGuid = Guid.Empty;
            Assert.Throws<ArgumentException>(() => Guard.ThrowIfDefault(defaultGuid));
        }
    }
}
