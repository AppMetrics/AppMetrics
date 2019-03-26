// <copyright file="StaticHealthCheckTypeProvider.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace App.Metrics.Health.Extensions.DependencyInjection.Internal
{
    internal sealed class StaticHealthCheckTypeProvider : IHealthCheckTypeProvider
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="StaticHealthCheckTypeProvider" /> class.
        /// </summary>
        public StaticHealthCheckTypeProvider()
            : this(Enumerable.Empty<TypeInfo>())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="StaticHealthCheckTypeProvider" /> class.
        /// </summary>
        /// <param name="controllerTypes">The controller types.</param>
        /// <exception cref="System.ArgumentNullException">if controller types is null.</exception>
        // ReSharper disable MemberCanBePrivate.Global
        public StaticHealthCheckTypeProvider(IEnumerable<TypeInfo> controllerTypes)
            // ReSharper restore MemberCanBePrivate.Global
        {
            if (controllerTypes == null)
            {
                throw new ArgumentNullException(nameof(controllerTypes));
            }

            HealthCheckTypes = new List<TypeInfo>(controllerTypes);
        }

        /// <summary>
        ///     Gets the list of controller <see cref="TypeInfo" />s.
        /// </summary>
        /// <value>
        ///     The health check types.
        /// </value>
        public IList<TypeInfo> HealthCheckTypes { get; }

        /// <inheritdoc />
        IEnumerable<TypeInfo> IHealthCheckTypeProvider.HealthCheckTypes => HealthCheckTypes;
    }
}