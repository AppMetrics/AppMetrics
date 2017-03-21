// <copyright file="IHideObjectMembers.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

// ReSharper disable CheckNamespace
namespace App.Metrics.Core.Interfaces
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Marker interface to cleanup editor visible members.
    /// </summary>
    /// <remarks>Created by Daniel Cazzulino http://www.clariusconsulting.net/blogs/kzu/archive/2008/03/10/58301.aspx</remarks>
    public interface IHideObjectMembers
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object obj);

        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();

        [EditorBrowsable(EditorBrowsableState.Never)]
        Type GetType();

        [EditorBrowsable(EditorBrowsableState.Never)]
        string ToString();
    }
}