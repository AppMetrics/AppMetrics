using System;

namespace App.Metrics.AspNetCore.AsynchronousReporting.Facts.Startup
{
    public class JsonTestStartup : DefaultTestStartup
    {
        public JsonTestStartup() : base(builder => builder.AsJson())
        {
        }
    }
}