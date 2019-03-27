// <copyright file="RandomClientIdForTesting.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace MetricsPrometheusSandboxMvc.JustForTesting
{
    public static class RandomClientIdForTesting
    {
        private static readonly Random Rnd = new Random();

        public static void SetTheFakeClaimsPrincipal(HttpContext context)
        {
            context.User =
                new ClaimsPrincipal(
                    new List<ClaimsIdentity>
                    {
                        new ClaimsIdentity(
                            new[]
                            {
                                new Claim("client_id", $"client-{Rnd.Next(1, 10)}")
                            })
                    });
        }
    }
}