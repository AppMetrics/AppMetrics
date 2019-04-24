// <copyright file="UserValueWrapper.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace App.Metrics
{
    public struct UserValueWrapper
    {
        public static readonly IComparer<UserValueWrapper> Comparer = new UserValueComparer();
        public static readonly UserValueWrapper Empty = default;

        public string UserValue { get; }

        public long Value { get; }

        public UserValueWrapper(long value, string userValue = null)
        {
            Value = value;
            UserValue = userValue;
        }

        public static bool operator ==(UserValueWrapper left, UserValueWrapper right) { return left.Equals(right); }

        public static bool operator !=(UserValueWrapper left, UserValueWrapper right) { return !left.Equals(right); }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is UserValueWrapper && Equals((UserValueWrapper)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((UserValue?.GetHashCode() ?? 0) * 397) ^ Value.GetHashCode();
            }
        }

        // ReSharper disable MemberCanBePrivate.Global
        public bool Equals(UserValueWrapper other) { return string.Equals(UserValue, other.UserValue) && Value == other.Value; }
        // ReSharper restore MemberCanBePrivate.Global

        private sealed class UserValueComparer : IComparer<UserValueWrapper>
        {
            public int Compare(UserValueWrapper x, UserValueWrapper y) { return Comparer<long>.Default.Compare(x.Value, y.Value); }
        }
    }
}