// Written by Iulian Margarintescu
// 
// Ported to .NET Standard Library by Allan Hardy
// Original repo: https://github.com/etishor/Metrics.NET

using System.Collections.Generic;

namespace App.Metrics.Sampling
{
    public struct UserValueWrapper
    {
        public static readonly IComparer<UserValueWrapper> Comparer = new UserValueComparer();
        public static readonly UserValueWrapper Empty = new UserValueWrapper();
        public readonly string UserValue;

        public readonly long Value;

        public UserValueWrapper(long value, string userValue = null)
        {
            Value = value;
            UserValue = userValue;
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