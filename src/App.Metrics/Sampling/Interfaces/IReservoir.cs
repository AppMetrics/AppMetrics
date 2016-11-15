// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy

namespace App.Metrics.Sampling.Interfaces
{
    public interface IReservoir
    {
        ISnapshot GetSnapshot(bool resetReservoir = false);

        void Reset();

        void Update(long value, string userValue = null);
    }
}