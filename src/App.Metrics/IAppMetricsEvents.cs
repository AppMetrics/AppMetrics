using System;
using System.Threading.Tasks;

namespace App.Metrics
{
    public interface IAppMetricsEvents
    {
        Task Error(Exception context);
    }
}