// <copyright file="IValueReader{T}.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

namespace App.Metrics.Concurrency.Internal
{
    // Written by Iulian Margarintescu and will retain the same license as the Java Version
    // Original .NET Source by Iulian Margarintescu: https://github.com/etishor/ConcurrencyUtilities/blob/master/Src/
    // Ported to a .NET Standard Project by Allan Hardy as the owner Iulian Margarintescu is unreachable and the source and packages are no longer maintained
    internal interface IValueReader<out T>
    {
        T GetValue();

        T NonVolatileGetValue();
    }
}