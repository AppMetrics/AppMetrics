# Resevoir Sampling

Histograms measure the statistical distribution of a set of values. In high performance applications it is not possible to keep the entire data stream of a histogram in memory. To work around this resevoir sampling algorithms allow us to maintain a small, manageable resevoir which is statistically representative of an entire data stream.

[Resevoir sampling](https://en.wikipedia.org/wiki/Reservoir_sampling) is a family of randomized algorithms for randomly choosing a sample of `k` items from a list `S` containing `n` items, where `n` is either a very large or unknown number. Typically `n` is large enough that it cannot be stored in memory. This type of sampling allows us to measure descriptive statisitcs including the *min*, *max*, *mean*, *median*, *standard deviation* and [quantiles](https://en.wikipedia.org/wiki/Quantile) i.e. the *75th percentile*, *90th percentile*, *95th percentile*, *99th percentile* and *99.9th percentile* on a stream of data.

**App Metrics** supports four types of sampling such data streams:

## Uniform Resevoir Sampling

A histogram with a [uniform reservoir](../../api/App.Metrics.Sampling.UniformReservoir.html) produces [quantiles](https://en.wikipedia.org/wiki/Quantile) which are valid for the entirely of the histogram’s lifetime.

This sampling resevoir can be used when you are interested in long-term measurements, it does not offer a sense of recency over the stream of data being measured.

The default sample size is 1028.

**App Metrics** uses [Algorithm R](http://www.cs.umd.edu/~samir/498/vitter.pdf) for uniform resevoir sampling.

## Exponentially Decaying Resevoir Sampling

A histogram with an [exponentially decaying reservoir](../../api/App.Metrics.Sampling.ExponentiallyDecayingReservoir.html) produces [quantiles](https://en.wikipedia.org/wiki/Quantile) which are representative of (roughly) the last five minutes of data, providing a sense of recency unlike Unifor Resevoir Sampling.

This sampling resevoir can be used when you are interested in recent changes to the distribution of data rather than a median on the lifetime of the histgram.

The default sample size of 1028 and alpha value of 0.015, offers a 99.9% confidence level with a 5% margin of error assuming a normal distribution and heavily biases the reservoir to the past 5 mins of measurements. The higher the alpha, the more biased the reservoir will be towards newer values.

**App Metrics** uses a [forward-decaying](http://dimacs.rutgers.edu/~graham/pubs/papers/fwddecay.pdf) resevoir with an exponential weighting towards recent samples.

## Sliding Window Resevoir Sampling

A Reservoir [implementation](../../api/App.Metrics.Sampling.SlidingWindowReservoir.html) backed by a [fixed-size sliding window](http://web.cs.ucla.edu/~rafail/PUBLIC/100.pdf) that stores only the measurements made in the last N seconds (or other time unit) and therefore like an Exponentially Decaying Resevoir provides a sense of recency. Statistical descritption with the type of resevoir are deterministic, so there is no danger that unfortunate random selections will produce bad approximations.

The default sample size is 1028.

## High Dynamic Range Histogram

A [HdrHistogram](http://hdrhistogram.org/) provides resevoir sampling that supports recording and analyzing sampled data value counts across a configurable integer value range with configurable value precision within the range.

HdrHistograms are designed for recoding histograms of value measurements in latency and performance sensitive applications. Measurements show value recording times as low as 3-6 nanoseconds on modern Intel CPUs

[Implementation](../../api/App.Metrics.Sampling.HdrHistogramReservoir.html) is based on [hdrhistogram-metrics-reservoir](https://bitbucket.org/marshallpierce/hdrhistogram-metrics-reservoir/src/83a8ec568a1e?at=master).


> [!NOTE]
> ### Just a Placeholder for now..
> Provide sample [Grafana](http://grafana.org/) screen shots show the difference over time between each type of resevoir sampling.

> [!NOTE]
> ### Original Implementation
> Resevoir Sampling was originally implemented by Iulian Margarintescu in the [Metrics.NET](https://github.com/etishor/Metrics.NET/tree/master/Src/Metrics/Sampling) library.

## Next Steps

- [How to use Resevoir Sampling](../metric-types/histograms.md)