// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy

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