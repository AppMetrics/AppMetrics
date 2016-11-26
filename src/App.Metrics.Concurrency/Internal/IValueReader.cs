// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

//Written by Iulian Margarintescu and will retain the same license as the Java Version
//Original .NET Source by Iulian Margarintescu: https://github.com/etishor/ConcurrencyUtilities/blob/master/Src/
//Ported to a .NET Standard Project by Allan Hardy as the owner Iulian Margarintescu is unreachable and the source and packages are no longer maintained

namespace App.Metrics.Concurrency.Internal
{
    internal interface IValueReader<out T>
    {
        T GetValue();

        T NonVolatileGetValue();
    }
}