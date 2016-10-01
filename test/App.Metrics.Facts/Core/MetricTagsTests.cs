using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Core
{
    public class MetricTagsTests
    {
        [Fact]
        public void MetricTags_CanUseDefaultValue()
        {
            var tags = default(MetricTags);
            tags.Tags.Should().NotBeNull();
            tags.Tags.Should().BeEmpty();
        }

        [Fact]
        public void MetricTags_CanCreateFromString()
        {
            MetricTags tags = "tag";
            tags.Tags.Should().Equal(new[] { "tag" });

            tags = new MetricTags("tag");
            tags.Tags.Should().Equal(new[] { "tag" });
        }

        [Fact]
        public void MetricTags_CanCreateFromCSV()
        {
            MetricTags tags = "tag1,tag2";
            tags.Tags.Should().Equal(new[] { "tag1", "tag2" });

            tags = new MetricTags("tag1,tag2");
            tags.Tags.Should().Equal(new[] { "tag1", "tag2" });
        }

        [Fact]
        public void MetricTags_CanCreateFromCSVAndTrimValues()
        {
            MetricTags tags = "tag1 , tag2";
            tags.Tags.Should().Equal(new[] { "tag1", "tag2" });
        }

        [Fact]
        public void MetricTags_CanCreateFromStringArray()
        {
            MetricTags tags = new[] { "tag1", "tag2" };
            tags.Tags.Should().Equal(new[] { "tag1", "tag2" });

            tags = new MetricTags(new[] { "tag1", "tag2" });
            tags.Tags.Should().Equal(new[] { "tag1", "tag2" });
        }

        [Fact]
        public void MetricTags_CanCreateFromParams()
        {
            MetricTags tags = new MetricTags("tag1", "tag2");
            tags.Tags.Should().Equal(new[] { "tag1", "tag2" });
        }
    }
}
