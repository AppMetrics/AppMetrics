public class DatabaseHealthCheck : HealthCheck
{
    private readonly IDatabase _database;

    public DatabaseHealthCheck(IDatabase database)
        : base("DatabaseCheck")
    {
        _database = database;
    }

    protected override Task<HealthCheckResult> CheckAsync()
    {
        // exceptions will be caught and the result will be un-healthy
        _database.Ping();

        return Task.FromResult(HealthCheckResult.Healthy());
    }
}