// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// ReSharper disable CheckNamespace

namespace System
// ReSharper restore CheckNamespace
{
    internal static class DateTimeExtensions
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime()
            .ToUniversalTime();

        internal static long ToUnixTime(this DateTime date)
        {
            return Convert.ToInt64((date.ToUniversalTime() - UnixEpoch).TotalSeconds);
        }
    }
}