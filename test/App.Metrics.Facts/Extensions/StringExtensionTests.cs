using System;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Extensions
{
    public class StringExtensionTests
    {
        [Theory]
        [InlineData("http://localhost", "http://localhost/")]
        [InlineData("http://localhost/", "http://localhost/")]
        public void can_ensure_trailing_slash(string url, string expected)
        {
            url.EnsureTrailingSlash().Should().Be(expected);
        }

        [Theory]
        [InlineData("localhost", "/localhost")]
        [InlineData("/localhost", "/localhost")]
        public void can_ensure_leading_slash(string url, string expected)
        {
            url.EnsureLeadingSlash().Should().Be(expected);
        }


        [Theory]
        [InlineData("/localhost", "localhost")]
        [InlineData("localhost", "localhost")]
        [InlineData("//localhost", "/localhost")]
        public void can_remove_leading_slash(string url, string expected)
        {
            url.RemoveLeadingSlash().Should().Be(expected);
        }

        [Fact]
        public void can_safe_get_string()
        {
            Action action = () => StringExtensions.GetSafeString(() => throw new ArgumentNullException());

            action.ShouldNotThrow();
        }
    }
}