using System;

namespace App.Metrics.AspNetCore.AsynchronousReporting.Facts.Startup
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class PlainTextTestStartup : DefaultTestStartup
        // ReSharper restore ClassNeverInstantiated.Global
    {
        /// <inheritdoc />
        public PlainTextTestStartup() : base(builder => builder.AsPlainText()) { }
    }
}