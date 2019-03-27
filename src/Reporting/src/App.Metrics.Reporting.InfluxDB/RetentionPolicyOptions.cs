// <copyright file="RetentionPolicyOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Text;

namespace App.Metrics.Reporting.InfluxDB
{
    /// <summary>
    /// Single retention policy associated with the created database. If you do not specify one of the properties, the relevant behavior defaults to the autogen retention policy settings.
    /// </summary>
    public class RetentionPolicyOptions
    {
        /// <summary>
        /// Gets or sets the Duration determines how long InfluxDB keeps the data.
        /// </summary>
        /// <value>
        /// The retention policy duration. The minimum duration for a retention policy is one hour.
        /// </value>
        public TimeSpan? Duration { get; set; }

        /// <summary>
        /// Gets or sets the Name determines policy name.
        /// </summary>
        /// <value>
        /// The retention policy name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Replication determines how many independent copies of each point are stored in the cluster. Replication factors do not serve a purpose with single node instances.
        /// </summary>
        /// <value>
        /// The retention policy replication. Number of data nodes.
        /// </value>
        public int? Replication { get; set; }

        /// <summary>
        /// Gets or sets the ShardDuration determines the time range covered by a shard group.
        /// </summary>
        /// <value>
        /// The retention policy shard duration. The minimum allowable SHARD GROUP DURATION is 1h
        /// </value>
        public TimeSpan? ShardDuration { get; set; }

        public bool TryApply(out string clause)
        {
            clause = null;
            bool hasName = !string.IsNullOrWhiteSpace(Name);
            bool hasNonDefaultValues = Duration.HasValue || Replication.HasValue || ShardDuration.HasValue || hasName;
            StringBuilder stringBuilder = new StringBuilder("WITH", 100);

            if (hasNonDefaultValues)
            {
                if (Duration.HasValue)
                {
                    stringBuilder.Append(" DURATION ");
                    stringBuilder.Append((int)Math.Floor(Duration.Value.TotalMinutes));
                    stringBuilder.Append("m");
                }

                if (Replication.HasValue)
                {
                    stringBuilder.Append(" REPLICATION ");
                    stringBuilder.Append(Replication.Value);
                }

                if (ShardDuration.HasValue)
                {
                    stringBuilder.Append(" SHARD DURATION ");
                    stringBuilder.Append((int)Math.Floor(ShardDuration.Value.TotalMinutes));
                    stringBuilder.Append("m");
                }

                if (hasName)
                {
                    stringBuilder.Append(" NAME \"");
                    stringBuilder.Append(Uri.EscapeDataString(Name));
                    stringBuilder.Append("\"");
                }

                clause = stringBuilder.ToString();
            }

            return hasNonDefaultValues;
        }
    }
}