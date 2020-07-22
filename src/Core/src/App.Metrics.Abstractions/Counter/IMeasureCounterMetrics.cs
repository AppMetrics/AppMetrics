// <copyright file="IMeasureCounterMetrics.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Counter
{
    /// <summary>
    ///     Provides access to the API allowing Counter Metrics to be measured/recorded.
    /// </summary>
    public interface IMeasureCounterMetrics
    {
        /// <summary>
        ///     Decrements a <see cref="ICounterMetric" />
        /// </summary>
        /// <param name="options">The details of the counter that is being decremented</param>
        void Decrement(CounterOptions options);

        /// <summary>
        ///     Decrements a <see cref="ICounterMetric" />
        /// </summary>
        /// <param name="options">The details of the counter that is being decremented</param>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique <see cref="MetricTags"/>
        /// </param>
        void Decrement(CounterOptions options, MetricTags tags);

        /// <summary>
        ///     Decrements a <see cref="ICounterMetric" /> by the specificed amount
        /// </summary>
        /// <param name="options">The details of the counter that is being decremented</param>
        /// <param name="amount">The amount to decrement the counter.</param>
        void Decrement(CounterOptions options, long amount);

        /// <summary>
        ///     Decrements a <see cref="ICounterMetric" /> by the specificed amount
        /// </summary>
        /// <param name="options">The details of the counter that is being decremented</param>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique <see cref="MetricTags"/>
        /// </param>
        /// <param name="amount">The amount to decrement the counter.</param>
        void Decrement(CounterOptions options, MetricTags tags, long amount);

        /// <summary>
        ///     Decrements the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="item">The item.</param>
        void Decrement(CounterOptions options, string item);

        /// <summary>
        ///     Decrements the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique <see cref="MetricTags"/>
        /// </param>
        /// <param name="item">The item.</param>
        void Decrement(CounterOptions options, MetricTags tags, string item);

        /// <summary>
        ///     Decrements the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="item">The item.</param>
        void Decrement(CounterOptions options, long amount, string item);

        /// <summary>
        ///     Decrements the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique <see cref="MetricTags"/>
        /// </param>
        /// <param name="amount">The amount.</param>
        /// <param name="item">The item.</param>
        void Decrement(CounterOptions options, MetricTags tags, long amount, string item);

        /// <summary>
        ///     Decrements a <see cref="ICounterMetric" /> as well as the specified item within the counter's set
        /// </summary>
        /// <remarks>
        ///     The counter value is decremented as is the specified <see cref="MetricSetItem" />'s counter within the set.
        ///     The <see cref="MetricSetItem" /> within the set will also keep track of it's percentage from the total sets count.
        /// </remarks>
        /// <param name="options">The details of the counter that is being decremented</param>
        /// <param name="setItem">The item within the set to decrement.</param>
        void Decrement(CounterOptions options, MetricSetItem setItem);

        /// <summary>
        ///     Decrements a <see cref="ICounterMetric" /> by the specified amount as well as the specified item within the
        ///     counter's set
        /// </summary>
        /// <param name="options">The details of the counter that is being decremented</param>
        /// <param name="amount">The amount to decrement the counter.</param>
        /// <param name="setItem">The item within the set to decrement.</param>
        /// <remarks>
        ///     The counter value is decremented as is the specified <see cref="MetricSetItem" />'s counter within the set.
        ///     The <see cref="MetricSetItem" /> within the set will also keep track of it's percentage from the total sets count.
        /// </remarks>
        void Decrement(CounterOptions options, long amount, MetricSetItem setItem);

        /// <summary>
        ///     Increments a <see cref="ICounterMetric" />
        /// </summary>
        /// <param name="options">The details of the counter that is being incremented</param>
        void Increment(CounterOptions options);

        /// <summary>
        ///     Increments a <see cref="ICounterMetric" />
        /// </summary>
        /// <param name="options">The details of the counter that is being incremented</param>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique <see cref="MetricTags"/>
        /// </param>
        void Increment(CounterOptions options, MetricTags tags);

        /// <summary>
        ///     Increments a <see cref="ICounterMetric" />
        /// </summary>
        /// <param name="options">The details of the counter that is being incremented</param>
        /// <param name="amount">The amount to increment the counter.</param>
        void Increment(CounterOptions options, long amount);

        /// <summary>
        ///     Increments a <see cref="ICounterMetric" />
        /// </summary>
        /// <param name="options">The details of the counter that is being incremented</param>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique <see cref="MetricTags"/>
        /// </param>
        /// <param name="amount">The amount to increment the counter.</param>
        void Increment(CounterOptions options, MetricTags tags, long amount);

        /// <summary>
        ///     Increments a <see cref="ICounterMetric" /> as well as the specified item within the counter's set
        /// </summary>
        /// <remarks>
        ///     The counter value is incremented as is the specified <see cref="MetricTags" />'s counter within the set.
        ///     The <see cref="MetricTags" /> within the set will also keep track of it's percentage from the total sets count.
        /// </remarks>
        /// <param name="options">The details of the counter that is being incremented</param>
        /// <param name="item">The item within the set to increment.</param>
        void Increment(CounterOptions options, string item);

        /// <summary>
        ///     Increments a <see cref="ICounterMetric" /> as well as the specified item within the counter's set
        /// </summary>
        /// <param name="options">The details of the counter that is being incremented</param>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique <see cref="MetricTags"/>
        /// </param>
        /// <param name="item">The item within the set to increment.</param>
        /// <remarks>
        ///     The counter value is incremented as is the specified <see cref="MetricTags" />'s counter within the set.
        ///     The <see cref="MetricTags" /> within the set will also keep track of it's percentage from the total sets count.
        /// </remarks>
        void Increment(CounterOptions options, MetricTags tags, string item);

        /// <summary>
        ///     Increments a <see cref="ICounterMetric" /> as well as the specified item within the counter's set
        /// </summary>
        /// <param name="options">The details of the counter that is being incremented</param>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique <see cref="MetricTags"/>
        /// </param>
        /// <param name="amount">The amount to increment the counter.</param>
        /// <param name="item">The item within the set to increment.</param>
        /// <remarks>
        ///     The counter value is incremented as is the specified <see cref="MetricTags" />'s counter within the set.
        ///     The <see cref="MetricTags" /> within the set will also keep track of it's percentage from the total sets count.
        /// </remarks>
        void Increment(CounterOptions options, MetricTags tags, long amount, string item);

        /// <summary>
        ///     Increments a <see cref="ICounterMetric" /> as well as the specified item within the counter's set
        /// </summary>
        /// <remarks>
        ///     The counter value is incremented as is the specified <see cref="MetricTags" />'s counter within the set.
        ///     The <see cref="MetricTags" /> within the set will also keep track of it's percentage from the total sets count.
        /// </remarks>
        /// <param name="options">The details of the counter that is being incremented</param>
        /// <param name="amount">The amount to increment the counter.</param>
        /// <param name="item">The item within the set to increment.</param>
        void Increment(CounterOptions options, long amount, string item);

        /// <summary>
        ///     Increment a <see cref="ICounterMetric" /> as well as the specified item within the counter's set
        /// </summary>
        /// <remarks>
        ///     The counter value is incremented as is the specified <see cref="MetricSetItem" />'s counter within the set.
        ///     The <see cref="MetricSetItem" /> within the set will also keep track of it's percentage from the total sets count.
        /// </remarks>
        /// <param name="options">The details of the counter that is being incremented</param>
        /// <param name="setItem">The item within the set to increment.</param>
        void Increment(CounterOptions options, MetricSetItem setItem);

        /// <summary>
        ///     Increment a <see cref="ICounterMetric" /> by the specified amount as well as the specified item within the
        ///     counter's set
        /// </summary>
        /// <param name="options">The details of the counter that is being incremented</param>
        /// <param name="amount">The amount to increment the counter.</param>
        /// <param name="setItem">The item within the set to increment.</param>
        /// <remarks>
        ///     The counter value is incremented as is the specified <see cref="MetricSetItem" />'s counter within the set.
        ///     The <see cref="MetricSetItem" /> within the set will also keep track of it's percentage from the total sets count.
        /// </remarks>
        void Increment(CounterOptions options, long amount, MetricSetItem setItem);
    }
}