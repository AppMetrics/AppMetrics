// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.Metrics.Health.Abstractions;

namespace App.Metrics.DependencyInjection.Internal
{
    internal sealed class StaticHealthCheckTypeProvider : IHealthCheckTypeProvider
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="StaticHealthCheckTypeProvider" /> class.
        /// </summary>
        public StaticHealthCheckTypeProvider()
            : this(Enumerable.Empty<TypeInfo>()) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="StaticHealthCheckTypeProvider" /> class.
        /// </summary>
        /// <param name="controllerTypes">The controller types.</param>
        /// <exception cref="System.ArgumentNullException">if controller types is null.</exception>
        public StaticHealthCheckTypeProvider(IEnumerable<TypeInfo> controllerTypes)
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