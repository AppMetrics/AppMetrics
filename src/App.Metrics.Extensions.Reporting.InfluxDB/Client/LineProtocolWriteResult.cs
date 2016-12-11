namespace App.Metrics.Extensions.Reporting.InfluxDB.Client
{
    public struct LineProtocolWriteResult
    {
        public LineProtocolWriteResult(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }

        public bool Success { get; }
    }
}