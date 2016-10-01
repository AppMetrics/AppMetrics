
using System.Collections.Generic;

namespace App.Metrics.Sampling
{
    public struct UserValueWrapper
    {
        public static readonly UserValueWrapper Empty = new UserValueWrapper();
        public static readonly IComparer<UserValueWrapper> Comparer = new UserValueComparer();

        public readonly long Value;
        public readonly string UserValue;

        public UserValueWrapper(long value, string userValue = null)
        {
            this.Value = value;
            this.UserValue = userValue;
        }

        private class UserValueComparer : IComparer<UserValueWrapper>
        {
            public int Compare(UserValueWrapper x, UserValueWrapper y)
            {
                return Comparer<long>.Default.Compare(x.Value, y.Value);
            }
        }
    }
}
