// ReSharper disable CheckNamespace
namespace System.Threading.Tasks
// ReSharper restore CheckNamespace
{
    internal static class TaskExtensions
    {
        public static async Task WithAggregateException(this Task source)
        {
            try
            {
                await source.ConfigureAwait(false);
            }
            catch
            {
                throw source.Exception;
            }
        }
    }
}