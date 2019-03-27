// <copyright file="StringExtensionTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Extensions
{
    public class StringExtensionTests
    {
        [Theory]
        [InlineData("localhost", "/localhost")]
        [InlineData("/localhost", "/localhost")]
        public void Can_ensure_leading_slash(string url, string expected)
        {
            url.EnsureLeadingSlash().Should().Be(expected);
        }

        [Theory]
        [InlineData("http://localhost", "http://localhost/")]
        [InlineData("http://localhost/", "http://localhost/")]
        public void Can_ensure_trailing_slash(string url, string expected)
        {
            url.EnsureTrailingSlash().Should().Be(expected);
        }

        [Theory]
        [InlineData("/localhost", "localhost")]
        [InlineData("localhost", "localhost")]
        [InlineData("//localhost", "/localhost")]
        public void Can_remove_leading_slash(string url, string expected)
        {
            url.RemoveLeadingSlash().Should().Be(expected);
        }

        [Fact]
        public void Can_safe_get_string()
        {
            Action action = () => StringExtensions.GetSafeString(() => throw new ArgumentNullException());

            action.Should().NotThrow();
        }
    }
}