using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TutsUniversity.Infrastructure
{
    public static class TaskExtensions
    {
        public static async Task WaitForAllAndThenAggregateResults<TSource>(this IEnumerable<TSource> source, Func<TSource, Task> selector)
        {
            var tasks = source.Select(selector);
            await tasks.WaitForAllAndThenAggregateResults();
        }

        private static async Task WaitForAllAndThenAggregateResults(this IEnumerable<Task> tasks)
        {
            var compositeTask = Task.WhenAll(tasks);
            await compositeTask.ContinueWith(_ => { }, TaskContinuationOptions.ExecuteSynchronously).ConfigureAwait(false);
            compositeTask.Wait();
        }
    }
}