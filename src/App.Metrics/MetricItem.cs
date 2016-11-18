using System.Collections.Generic;
using System.Linq;
using App.Metrics.Utils;

namespace App.Metrics
{
    /// <summary>
    ///     <para>
    ///         Metric items can be used with <see cref="Counter" /> or <see cref="Meter" /> <see cref="MetricType" />s
    ///     </para>
    ///     <para>
    ///         They provide the ability to track either a count or rate for each item in a counters or meters finite set
    ///         respectively. They also track the overall percentage of each item in the set.
    ///     </para>
    ///     <para>
    ///         This is useful for example if we needed to track the total number of emails sent but also the count of each
    ///         type of emails sent or The total rate of emails sent but also the rate at which type of email was sent.
    ///     </para>
    /// </summary>
    /// <seealso cref="App.Metrics.Metric" />
    public sealed class MetricItem : IHideObjectMembers
    {
        private static readonly Dictionary<string, string> Empty = new Dictionary<string, string>();
        private Dictionary<string, string> _tags;

        public MetricItem(Dictionary<string, string> tags)
        {
            _tags = tags ?? new Dictionary<string, string>();
        }

        public MetricItem()
        {
            _tags = new Dictionary<string, string>();
        }

        public static bool operator ==(MetricItem left, MetricItem right)
        {
            return left != null && left.Equals(right);
        }

        public static implicit operator MetricItem(Dictionary<string, string> tags)
        {
            return new MetricItem(tags);
        }

        public static bool operator !=(MetricItem left, MetricItem right)
        {
            return left != null && !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MetricItem))
            {
                return false;
            }

            var tags = (MetricItem)obj;

            return tags.ToDictionary().OrderBy(kvp => kvp.Key)
                .SequenceEqual(_tags.OrderBy(kvp => kvp.Key));
        }

        public override int GetHashCode()
        {
            return _tags?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return string.Join("|", _tags.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Key + ":" + kvp.Value));
        }

        public bool Equals(MetricItem other)
        {
            return Equals(_tags, other._tags);
        }

        public Dictionary<string, string> ToDictionary()
        {
            return _tags ?? Empty;
        }

        public MetricItem With(string tag, string value)
        {
            if (_tags == null)
            {
                _tags = new Dictionary<string, string>();
            }

            _tags.Add(tag, value);
            return _tags;
        }
    }
}