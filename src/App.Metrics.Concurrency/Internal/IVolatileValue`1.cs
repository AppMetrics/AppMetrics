// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace App.Metrics.Concurrency.Internal
{
    // Written by Iulian Margarintescu and will retain the same license as the Java Version
    // Original .NET Source by Iulian Margarintescu: https://github.com/etishor/ConcurrencyUtilities/blob/master/Src/
    // Ported to a .NET Standard Project by Allan Hardy as the owner Iulian Margarintescu is unreachable and the source and packages are no longer maintained
    internal interface IVolatileValue<T> : IValueReader<T>, IValueWriter<T>
    {
    }
}