// <copyright file="TemplateExtensionsTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.AspNetCore.Routing.Template;
using Moq;
using Xunit;

namespace App.Metrics.AspNetCore.Routing.Facts
{
    public class TemplateExtensionsTests
    {
        private static readonly RequestDelegate NullHandler = c => Task.FromResult(0);
        private static readonly IInlineConstraintResolver InlineConstraintResolver = GetInlineConstraintResolver();

        [Fact]
        public void Can_format_template_segements()
        {
            var expected = "{p}";
            var segments = new List<TemplateSegment> { new TemplateSegment() };
            segments[0].Parts.Add(TemplatePart.CreateParameter("p", false, false, defaultValue: null, inlineConstraints: null));

            var result = segments[0].ToTemplateSegmentString();

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("{p}", "p", false, false)]
        [InlineData("{*p}", "p", true, false)]
        [InlineData("{*p?}", "p", true, true)]
        [InlineData("{p?}", "p", false, true)]
        public void Formats_template_parameters_correctly(string expected, string template, bool isCatchAll, bool isOptional)
        {
            var templatePart = TemplatePart.CreateParameter(template, isCatchAll, isOptional, defaultValue: null, inlineConstraints: null);

            var result = templatePart.ToTemplatePartString();

            result.Should().Be(expected);
        }

        [Fact]
        public void If_not_parameter_return_text()
        {
            var expected = "test";
            var templatePart = TemplatePart.CreateLiteral(expected);

            var result = templatePart.ToTemplatePartString();

            result.Should().Be(expected);
        }

        [Fact]
        public void Can_get_template_from_route()
        {
            var expected = "test/action";

            var route = CreateRoute("{controller}/{action}");

            var result = route.ToTemplateString("Test", "Action", string.Empty);

            result.Should().Be(expected);
        }

        private static Route CreateRoute(string template, bool handleRequest = true)
        {
            return new Route(CreateTarget(handleRequest), template, InlineConstraintResolver);
        }

        private static IRouter CreateTarget(bool handleRequest = true)
        {
            var target = new Mock<IRouter>(MockBehavior.Strict);
            target
                .Setup(e => e.GetVirtualPath(It.IsAny<VirtualPathContext>()))
                .Returns<VirtualPathContext>(rc => null);

            target
                .Setup(e => e.RouteAsync(It.IsAny<RouteContext>()))
                .Callback<RouteContext>((c) => c.Handler = handleRequest ? NullHandler : null)
                .Returns(Task.FromResult<object>(null));

            return target.Object;
        }

        private static IInlineConstraintResolver GetInlineConstraintResolver()
        {
            var resolverMock = new Mock<IInlineConstraintResolver>();
            resolverMock.Setup(o => o.ResolveConstraint("int")).Returns(new IntRouteConstraint());
            resolverMock.Setup(o => o.ResolveConstraint("range(1,20)")).Returns(new RangeRouteConstraint(1, 20));
            resolverMock.Setup(o => o.ResolveConstraint("alpha")).Returns(new AlphaRouteConstraint());
            resolverMock.Setup(o => o.ResolveConstraint(@"regex(^\d{3}-\d{3}-\d{4}$)")).Returns(
                new RegexInlineRouteConstraint(@"^\d{3}-\d{3}-\d{4}$"));
            resolverMock.Setup(o => o.ResolveConstraint(@"regex(^\d{1,2}\/\d{1,2}\/\d{4}$)")).Returns(
                new RegexInlineRouteConstraint(@"^\d{1,2}\/\d{1,2}\/\d{4}$"));
            return resolverMock.Object;
        }
    }
}