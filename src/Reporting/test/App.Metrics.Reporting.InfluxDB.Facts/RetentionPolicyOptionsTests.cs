// <copyright file="RetentionPolicyOptionsTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using Xunit;

namespace App.Metrics.Reporting.InfluxDB.Facts
{
    public class RetentionPolicyOptionsTests
    {
        [Fact]
        public void TryApplyDefaultRetentionPolicy()
        {
            RetentionPolicyOptions options = new RetentionPolicyOptions();

            bool result = options.TryApply(out var clause);

            Assert.False(result);
            Assert.Null(clause);
        }

        [Theory]
        [InlineData(null,null,null,null,false,null)]
        [InlineData(65, null, null, null, true, "WITH DURATION 65m")]
        [InlineData(null, 3, null, null, true, "WITH REPLICATION 3")]
        [InlineData(null, null, 60, null, true, "WITH SHARD DURATION 60m")]
        [InlineData(null, null, null, "any Name", true, "WITH NAME \"any%20Name\"")]
        [InlineData(65, 3, 60, "any Name", true, "WITH DURATION 65m REPLICATION 3 SHARD DURATION 60m NAME \"any%20Name\"")]

        public void TryApplyRetentionPolicy(
            int? duration,
            int? replication,
            int? shardDuration,
            string name,
            bool expected,
            string expectedClause)
        {
            RetentionPolicyOptions options =
                new RetentionPolicyOptions
                {
                    Name = name,
                    Duration = duration.HasValue ? TimeSpan.FromMinutes(duration.Value) : (TimeSpan?)null ,
                    Replication = replication,
                    ShardDuration = shardDuration.HasValue ? TimeSpan.FromMinutes(shardDuration.Value) : (TimeSpan?)null
                };

            bool result = options.TryApply(out var clause);

            Assert.Equal(expected, result);
            Assert.Equal(expectedClause, clause);
        }
    }
}