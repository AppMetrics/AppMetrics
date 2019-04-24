// <copyright file="ExcludeFromCodeCoverageAttribute.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

#if NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD1_6

// ReSharper disable CheckNamespace
namespace System.Diagnostics.CodeAnalysis
    // ReSharper restore CheckNamespace
{
    internal class ExcludeFromCodeCoverageAttribute : Attribute { }
}

#endif
