// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.Metrics.Health;
using App.Metrics.Internal;

namespace App.Metrics.DependencyInjection
{
    public sealed class StaticHealthCheckTypeProvider : IHealthCheckTypeProvider
    {
        /// <summary>
        /// Initializes a new instance of <see cref="StaticHealthCheckTypeProvider"/>.
        /// </summary>
        public StaticHealthCheckTypeProvider()
            : this(Enumerable.Empty<TypeInfo>())
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="StaticHealthCheckTypeProvider"/>.
        /// </summary>
        /// <param name="controllerTypes">The sequence of controller <see cref="TypeInfo"/>.</param>
        public StaticHealthCheckTypeProvider(IEnumerable<TypeInfo> controllerTypes)
        {
            if (controllerTypes == null)
            {
                throw new ArgumentNullException(nameof(controllerTypes));
            }

            HealthCheckTypes = new List<TypeInfo>(controllerTypes);
        }

        /// <summary>
        /// Gets the list of controller <see cref="TypeInfo"/>s.
        /// </summary>
        public IList<TypeInfo> HealthCheckTypes { get; }

        /// <inheritdoc />
        IEnumerable<TypeInfo> IHealthCheckTypeProvider.HealthCheckTypes => HealthCheckTypes;
    }
}