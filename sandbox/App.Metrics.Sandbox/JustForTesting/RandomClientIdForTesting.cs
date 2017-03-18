using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace App.Metrics.Sandbox.JustForTesting
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